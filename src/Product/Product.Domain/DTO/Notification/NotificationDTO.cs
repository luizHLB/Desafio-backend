namespace Product.Domain.DTO.Notification
{
    public class NotificationDTO
    {
        public long Id { get; set; }
        public string Message { get; set; }

        public NotificationDTO(Entities.Notification entity)
        {
            Id = entity.Id;
            Message = entity.Message;
        }

        public NotificationDTO()
        {
        }
    }
}
