namespace Product.Domain.Interfaces.Services
{
    public interface INotificationService
    {
        Task Register(object dto);
    }
}
