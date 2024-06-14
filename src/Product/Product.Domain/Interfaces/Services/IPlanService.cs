using Product.Domain.DTO.Plan;
using Product.Domain.Entities;

namespace Product.Domain.Interfaces.Services
{
    public interface IPlanService : IBaseService<Plan, PlanDTO>
    {
        Task<IList<PlanDTO>> GetPlans();
    }
}
