using Microsoft.Extensions.Logging;
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
        private readonly IDriverRepository _driverRepository;
        private readonly IPlanRepository _planRepository;

        public RentalService(ILogger<RentalService> logger, IRentalRepository repository, IDriverRepository driverRepository, IPlanRepository planRepository) : base(logger, repository)
        {
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
            var entity = await _repository.GetById(dto.Id);
            if (entity is null)
                throw new RecordNotFoundException();

            if (entity.ReturnDate.HasValue)
                throw new EntityConstraintException("Rental already completed");

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
            if (!entity.TotalFines.HasValue)
                entity.TotalExtras = CalculateExtras(entity, plan);
        }

        private double CalculateExtras(Rental entity, Plan plan)
        {
            var days = (entity.ReturnDate.Value - entity.WithdrawDate).Days;
            if (!plan.Extra.HasValue || days <= 0)
                return 0D;

            return days * plan.Extra.Value;
        }

        private double CalculateFines(Rental entity, Plan plan)
        {
            var days = (entity.ReturnDate.Value - entity.WithdrawDate).Days;
            if (!plan.Fine.HasValue || days >= plan.Period)
                return 0D;

            return (plan.Period - days) * (plan.Fine.Value / 100) * plan.Price;

        }

        private double CalculateTotalRental(Rental entity, Plan plan)
        {
            var days = Math.Ceiling((entity.ReturnDate.Value - entity.WithdrawDate).TotalDays);
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
                messages.Add("VehicleId is invalid");

            if (entity.WithdrawDate.Equals(new DateTime()) || entity.WithdrawDate >= DateTime.Now)
                messages.Add("Withdraw date is invalid");

            if (entity.EstimatedReturnDate.Equals(new DateTime()) || entity.EstimatedReturnDate <= DateTime.Now || entity.EstimatedReturnDate <= entity.WithdrawDate)
                messages.Add("Estimated Return date is invalid");

            if (entity.DriverId > 0)
            {
                var driver = await _driverRepository.GetById(entity.DriverId);
                if (!EnumHelper<CNHCategory>.GetEnums(driver.CNHCategory).Contains(CNHCategory.A))
                    messages.Add($"Driver CNH Cagetory must be {CNHCategory.A}");
            }

            if (messages.Any())
                throw new EntityConstraintException(messages);
        }
    }
}
