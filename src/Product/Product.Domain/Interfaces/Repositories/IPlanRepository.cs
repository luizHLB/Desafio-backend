using Product.Domain.DTO.Plan;
using Product.Domain.Entities;

namespace Product.Domain.Interfaces.Repositories
{
    public interface IPlanRepository : IBaseRepository<Plan, PlanDTO>
    {
        Task<IList<PlanDTO>> GetPlans();
    }
}
