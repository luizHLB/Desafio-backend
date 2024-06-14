namespace Product.Domain.Secutiry
{
    public class JwtContextVO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public JwtContextVO(long id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
