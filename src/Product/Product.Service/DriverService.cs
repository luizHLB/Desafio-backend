using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Product.Domain.DTO;
using Product.Domain.DTO.Driver;
using Product.Domain.Entities;
using Product.Domain.Entities.Enums;
using Product.Domain.Exceptions;
using Product.Domain.Helpers;
using Product.Domain.Interfaces.Repositories;
using Product.Domain.Interfaces.Services;
using Product.Domain.Interfaces.Utils;
using Product.Domain.Settings;
using Product.Service.Base;

namespace Product.Service
{
    public class DriverService : BaseService<Driver, DriverDTO>, IDriverService
    {
        private readonly IAzureStorage _azureStorage;
        private readonly string _baseURI;

        public DriverService(ILogger<DriverService> logger, IDriverRepository respository, IAzureStorage azureStorage, IConfiguration config) : base(logger, respository)
        {
            _azureStorage = azureStorage;

            var section = config.GetSection(nameof(AzureStorageSettings));
            var azuresettings = string.IsNullOrEmpty(section.Value)
                ? section.Get<AzureStorageSettings>()
                : JsonConvert.DeserializeObject<AzureStorageSettings>(section.Value);
            _baseURI = $"{azuresettings.BaseURI}/{azuresettings.ContainerName}/";
        }

        public async Task<DriverDTO> Create(CreateDriverDTO dto)
        {
            ValidateFile(dto.CNHImage);
            var fileName = await _azureStorage.UploadFile(dto.CNHImage);
            var entity = new Driver
            {
                Identifier = dto.Identifier,
                Name = dto.Name,
                CNPJ = dto.CNPJ,
                BirthDate = dto.BirthDate,
                CNH = dto.CNH,
                CNHCategory = EnumHelper<CNHCategory>.GetValue(dto.CNHCategory),
                CNHImage = fileName
            };

            await Add(entity);
            return new DriverDTO(entity)
            {
                CNHImage = $"{_baseURI}{entity.CNHImage}"
            };
        }

        public async Task<DriverDTO> GetDtoById(long id)
        {
            var response = await GetById(id);
            if (response is null)
                throw new RecordNotFoundException();

            return new DriverDTO(response)
            {
                CNHImage = $"{_baseURI}{response.CNHImage}"
            };
        }

        public async Task<PagedListDTO<DriverDTO>> PagedListAsync(string name = "", string cnpj = "", string cnh = "", int page = 1, int pageSize = 10)
        {
            if (!string.IsNullOrEmpty(name))
                name = name.ToLower();

            if (!string.IsNullOrEmpty(cnpj))
                cnpj = cnpj.OnlyDigits().ToLower();

            if (!string.IsNullOrEmpty(cnh))
                cnh = cnh.OnlyDigits().ToLower();

            return await _repository.PagedListAsync(w => w.Name.ToLower().Contains(name) && w.CNPJ.Contains(cnpj) && w.CNH.Contains(cnh), page, pageSize);
        }

        public async Task<DriverDTO> Update(long id, UpdateDriverDTO  dto)
        {
            var entity = await GetById(id);
            if (entity is null)
                throw new RecordNotFoundException();

            ValidateFile(dto.CNHImage);
            var fileName = await _azureStorage.UploadFile(dto.CNHImage);
            await _azureStorage.DeleteFile(entity.CNHImage);
            entity.CNHImage = fileName;

            await Update(entity);
            return new DriverDTO(entity)
            {
                CNHImage = $"{_baseURI}{entity.CNHImage}"
            };

        }

        protected override async Task Validate(Driver entity)
        {
            var messages = new List<string>();
            if (string.IsNullOrEmpty(entity.Name))
                messages.Add("Name is required");

            if (string.IsNullOrEmpty(entity.CNPJ))
                messages.Add("CNPJ is required");

            entity.CNPJ = entity.CNPJ.OnlyDigits();
            if (entity.CNPJ.Length == 0)
                messages.Add("CNPJ is invalid");

            if (string.IsNullOrEmpty(entity.CNH))
                messages.Add("CNPJ is required");

            entity.CNH = entity.CNH.OnlyDigits();
            if (entity.CNH.Length == 0)
                messages.Add("CNH is invalid");

            var cnhAllowed = new CNHCategory[] { CNHCategory.A, CNHCategory.B };
            if (!EnumHelper<CNHCategory>.GetEnums(entity.CNHCategory).Intersect(cnhAllowed).Any())
                messages.Add($"CNH Category must be {string.Join(",", cnhAllowed.Select(s => $"{(int)s} - {s}"))}");

            if (entity.BirthDate == new DateTime() || DateTime.Now.AddYears(-18) < entity.BirthDate)
                messages.Add("Birth Date invalid, Driver must be at least 18 years old");

            if (messages.Any())
                throw new EntityConstraintException(messages);
        }

        private void ValidateFile(IFormFile file)
        {
            if (file is null)
                throw new EntityConstraintException("CNH Image is required");

            var extensions = new string[] { ImageFormat.PNG.ToString(), ImageFormat.BMP.ToString() };

            if (!extensions.Contains(file.FileName.ToUpper().Split('.').LastOrDefault()))
                throw new EntityConstraintException($"CNH Image format not allowed. Must be ({string.Join(",", extensions)})");

            if (file.Length <= 0)
                throw new EntityConstraintException($"CNH Image corrupted");
        }

        public override async Task Remove(long id)
        {
            var entity = await GetById(id);
            if (entity is null)
                throw new RecordNotFoundException();

            await _azureStorage.DeleteFile(entity.CNHImage);
            await Remove(entity);
        }
    }
}
