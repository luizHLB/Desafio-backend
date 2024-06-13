using Product.Domain.DTO;
using Product.Domain.DTO.Notification;
using Product.Domain.Entities;

namespace Product.Domain.Interfaces.Services
{
    public interface INotificationService : IBaseService<Notification, NotificationDTO>
    {
        Task<PagedListDTO<NotificationDTO>> PagedListAsync(int page, int pageSize);
        Task Register(Vehicle dto);
    }
}
