using Product.Domain.DTO.Notification;
using Product.Domain.Entities;

namespace Product.Domain.Interfaces.Repositories
{
    public interface INotificationRepository : IBaseRepository<Notification, NotificationDTO>
    {
        void SetRead(IEnumerable<long> ids);
    }
}
