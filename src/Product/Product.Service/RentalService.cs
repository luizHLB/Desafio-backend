using Microsoft.Extensions.Logging;
using Product.Domain.DTO;
using Product.Domain.DTO.Rental;
using Product.Domain.Entities;
using Product.Domain.Entities.Enums;
using Product.Domain.Exceptions;
using Product.Domain.Helpers;
using Product.Domain.Interfaces.Repositories;
using Product.Domain.Interfaces.Services;
using Product.Service.Base;

namespace Product.Service
{
    public class RentalService : BaseService<Rental, RentalDTO>, IRentalService
    {
        private readonly IRentalRepository _repository;
        private readonly IDriverRepository _driverRepository;
        private readonly IPlanRepository _planRepository;

        public RentalService(ILogger<RentalService> logger, IRentalRepository repository, IDriverRepository driverRepository, IPlanRepository planRepository) : base(logger, repository)
        {
            _repository = repository;
            _driverRepository = driverRepository;
            _planRepository = planRepository;
        }

        public async Task<RentalDTO> Create(CreateRentalDTO dto)
        {
            var entity = new Rental
            {
                DriverId = dto.DriverId,
                PlanId = dto.PlanId,
                VehicleId = dto.VehicleId,
                WithdrawDate = dto.WithdrawDate,
                EstimatedReturnDate = dto.EstimatedReturnDate,
            };

            await Add(entity);

            return new RentalDTO(entity);
        }

        public async Task<RentalDTO> Complete(UpdateRentalDTO dto)
        {
            var entity = await base._repository.GetById(dto.Id);
            if (entity is null)
                throw new RecordNotFoundException();

            if (entity.ReturnDate.HasValue)
                throw new EntityConstraintException("Rental already completed");

            if (dto.ReturnalDate.Date.Equals(new DateTime().Date) || dto.ReturnalDate.Date < entity.WithdrawDate.Date)
                throw new EntityConstraintException("Returnal date is invalid");

            entity.ReturnDate = dto.ReturnalDate;
            await CalculateTotals(entity);

            await Update(entity);
            return new RentalDTO(entity);

        }

        private async Task CalculateTotals(Rental entity)
        {
            var plan = await _planRepository.GetById(entity.PlanId);

            entity.TotalRental = CalculateTotalRental(entity, plan);
            entity.TotalFines = CalculateFines(entity, plan);
            entity.TotalExtras = CalculateExtras(entity, plan);
        }

        private double CalculateExtras(Rental entity, Plan plan)
        {
            var days = (entity.ReturnDate.GetValueOrDefault().Date - entity.EstimatedReturnDate.Date).Days;

            if (!plan.Extra.HasValue || days <= 0)
                return 0D;

            return days * plan.Extra.Value;
        }

        private double CalculateFines(Rental entity, Plan plan)
        {
            var days = (entity.EstimatedReturnDate.Date - entity.ReturnDate.GetValueOrDefault().Date).Days;

            if (!plan.Fine.HasValue || days <= 0)
                return 0D;

            return days * (plan.Fine.Value / 100) * plan.Price;
        }

        private double CalculateTotalRental(Rental entity, Plan plan)
        {
            var baseDate = entity.ReturnDate.GetValueOrDefault().Date > entity.EstimatedReturnDate.Date
                ? entity.EstimatedReturnDate.Date
                : entity.ReturnDate.GetValueOrDefault().Date;

            var days = (baseDate - entity.WithdrawDate.Date).Days;
            return days * plan.Price;
        }

        protected override async Task Validate(Rental entity)
        {
            var messages = new List<string>();
            if (entity.PlanId <= 0)
                messages.Add("Plan is invalid");

            if (entity.DriverId <= 0)
                messages.Add("Driver is invalid");

            if (entity.VehicleId <= 0)
                messages.Add("Vehicle is invalid");

            if (entity.WithdrawDate.Equals(new DateTime()) || entity.WithdrawDate >= DateTime.Now)
                messages.Add("Withdraw date is invalid");

            if (entity.EstimatedReturnDate.Date.Equals(new DateTime().Date) || entity.EstimatedReturnDate.Date <= entity.WithdrawDate.Date)
                messages.Add("Estimated Return date is invalid");

            if (entity.DriverId > 0)
            {
                var driver = await _driverRepository.GetById(entity.DriverId);
                if (!EnumHelper<CNHCategory>.GetEnums(driver.CNHCategory).Contains(CNHCategory.A))
                    messages.Add($"Driver CNH Cagetory must be {CNHCategory.A}");
            }

            if (messages.Any())
                throw new EntityConstraintException(messages);

            if (await _repository.CheckVehicleDisponibilty(entity.WithdrawDate, entity.EstimatedReturnDate, entity.VehicleId))
                throw new EntityConstraintException("Vehicle not available on this period");
        }

        public async Task<RentalDTO> GetDtoById(long id)
        {
            var response = await _repository.GetById(id);
            return new RentalDTO(response);
        }

        public async Task<PagedListDTO<RentalDTO>> PagedListAsync(long? driverId, int page, int pageSize)
        {
            var validate = driverId.GetValueOrDefault() > 0;
            return await _repository.PagedListAsync(w => validate ? w.DriverId == driverId.Value : true, page, pageSize);
        }
    }
}
