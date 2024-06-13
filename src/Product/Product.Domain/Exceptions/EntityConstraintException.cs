namespace Product.Domain.Exceptions
{
    public class EntityConstraintException : Exception
    {
        public EntityConstraintException(string message) : base($"Cannot duplicate {message.Split('_').LastOrDefault()}") { }
        public EntityConstraintException(IList<string> messages) : base(string.Join(" | ", messages)) { }
    }
}
