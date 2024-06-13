using Product.Domain.Entities.Base;

namespace Product.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public string Message { get; set; }
        public bool Read { get; set; }
    }
}
