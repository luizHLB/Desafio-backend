using Microsoft.Extensions.Logging;
using Product.Domain.DTO;
using Product.Domain.DTO.Vehicle;
using Product.Domain.Entities;
using Product.Domain.Exceptions;
using Product.Domain.Helpers;
using Product.Domain.Interfaces.Repositories;
using Product.Domain.Interfaces.Services;
using Product.Domain.Interfaces.Utils;
using Product.Domain.Settings;
using Product.Service.Base;
using System.Text.RegularExpressions;

namespace Product.Service
{
    public class VehicleService : BaseService<Vehicle, VehicleDTO>, IVehicleService
    {
        private readonly IRabbitMQManager _rabbitMQManager;

        public VehicleService(ILogger<VehicleService> logger, IVehicleRepository respository, IRabbitMQManager rabbitMQManager) : base(logger, respository)
        {
            _rabbitMQManager = rabbitMQManager;
        }

        public async Task<VehicleDTO> Create(CreateVehicleDTO dto)
        {
            var entity = new Vehicle
            {
                Identifier = dto.Identifier,
                LicensePlate = dto.LicensePlate,
                Model = dto.Model,
                Year = dto.Year
            };

            await Add(entity);
            _rabbitMQManager.BasicPublish(ExecutionQueue.Notification, entity, $"{entity.Year}");
            return new VehicleDTO(entity);
        }

        public async Task<VehicleDTO> GetDtoById(long id)
        {
            var response = await GetById(id);
            if (response is null)
                throw new RecordNotFoundException();

            return new VehicleDTO(response);
        }

        public async Task<PagedListDTO<VehicleDTO>> PagedListAsync(string licensePlate = "", int page = 1, int pageSize = 10)
        {
            if (!string.IsNullOrEmpty(licensePlate))
                licensePlate = Regex.Replace(licensePlate, @"(\W+)?", "").ToLower();
            return await _repository.PagedListAsync(w => w.LicensePlate.ToLower().Contains(licensePlate), page, pageSize);
        }

        public async Task<VehicleDTO> Update(PatchVehicleDTO dto)
        {
            var entity = await GetById(dto.Id);
            entity.LicensePlate = dto.LicensePlate;
            await Update(entity);

            return new VehicleDTO(entity);

        }

        protected override async Task Validate(Vehicle entity)
        {
            var messages = new List<string>();
            if (string.IsNullOrEmpty(entity.Identifier))
                messages.Add("Identifier is required");

            if (string.IsNullOrEmpty(entity.LicensePlate))
                messages.Add("LicensePlate is required");

            entity.LicensePlate = entity.LicensePlate.OnlyAlphaNumeric();

            if (entity.LicensePlate.Length == 0 || entity.LicensePlate.Length > 7)
                messages.Add("LicensePlate is invalid");

            if (string.IsNullOrEmpty(entity.Model))
                messages.Add("Model is required");

            if (entity.Year <= 0 || entity.Year > DateTime.Now.Year)
                messages.Add("Year is invalid");

            if (messages.Any())
                throw new EntityConstraintException(messages);
        }
    }
}
