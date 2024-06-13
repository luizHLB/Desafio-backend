namespace Product.Domain.Exceptions
{
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException() : base ("Record not found")
        {
        }
    }
}
