using Microsoft.EntityFrameworkCore;
using Product.Data.Contexts;
using Product.Data.Repositories.Base;
using Product.Domain.DTO.Notification;
using Product.Domain.Entities;
using Product.Domain.Interfaces.Repositories;

namespace Product.Data.Repositories
{
    public class NotificationRepository : BaseRepository<Notification, NotificationDTO>, INotificationRepository
    {
        public NotificationRepository(ProductContext context) : base(context) { }

        public override List<NotificationDTO> Cast(List<Notification> itens)
        {
            return itens.Select(s => new NotificationDTO(s)).ToList();
        }

        public void SetRead(IEnumerable<long> ids)
        {
            var notifications = _context.Notifications.Where(w => ids.Contains(w.Id));
            if (notifications.Count() > 0)
            {
                notifications.ExecuteUpdate(b => b.SetProperty(u => u.Read, true));
                _context.SaveChanges();
            }
        }
    }
}
