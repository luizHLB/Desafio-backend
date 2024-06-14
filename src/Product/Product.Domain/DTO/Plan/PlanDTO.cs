namespace Product.Domain.DTO.Plan
{
    public class PlanDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public PlanDTO(Entities.Plan entity)
        {
            Id = entity.Id;
            Name = entity.Name;
        }
        public PlanDTO()
        {
            
        }
    }
}
