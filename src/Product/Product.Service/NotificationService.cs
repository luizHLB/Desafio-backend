using Microsoft.Extensions.Logging;
using Product.Domain.DTO;
using Product.Domain.DTO.Notification;
using Product.Domain.Entities;
using Product.Domain.Interfaces.Repositories;
using Product.Domain.Interfaces.Services;
using Product.Service.Base;

namespace Product.Service
{
    public class NotificationService : BaseService<Notification, NotificationDTO>, INotificationService
    {
        private readonly INotificationRepository _repository;

        public NotificationService(ILogger<NotificationService> logger, INotificationRepository repository) : base(logger, repository)
        {
            _repository = repository;
        }

        public async Task<PagedListDTO<NotificationDTO>> PagedListAsync(int page, int pageSize)
        {
            var response = await _repository.PagedListAsync(w => !w.Read, page, pageSize);
            _repository.SetRead(response.Items.Select(s => s.Id));
            return response;
        }

        public async Task Register(Vehicle dto)
        {
            var entity = new Notification
            {
                Message = $"New Vehicle Year: {dto.Year}, License Plage: {dto.LicensePlate}"
            };

            await Add(entity);
        }
    }
}
