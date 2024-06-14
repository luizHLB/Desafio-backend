using Microsoft.EntityFrameworkCore;
using Product.Data.Contexts;
using Product.Data.Repositories.Base;
using Product.Domain.DTO.Plan;
using Product.Domain.Entities;
using Product.Domain.Interfaces.Repositories;

namespace Product.Data.Repositories
{
    public class PlanRepository : BaseRepository<Plan, PlanDTO>, IPlanRepository
    {
        public PlanRepository(ProductContext context) : base(context) { }

        public override List<PlanDTO> Cast(List<Plan> itens) => itens.Select(s => new PlanDTO(s)).ToList();

        public async Task<IList<PlanDTO>> GetPlans() => 
            await _context.Plans.AsNoTracking().Select(s => new PlanDTO { Id = s.Id, Name = s.Name }).ToListAsync();
    }
}
