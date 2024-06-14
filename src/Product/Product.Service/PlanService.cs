using Microsoft.Extensions.Logging;
using Product.Domain.DTO.Plan;
using Product.Domain.Entities;
using Product.Domain.Interfaces.Repositories;
using Product.Domain.Interfaces.Services;
using Product.Service.Base;

namespace Product.Service
{
    public class PlanService : BaseService<Plan, PlanDTO>, IPlanService
    {
        private readonly IPlanRepository _repository;

        public PlanService(ILogger<PlanService> logger, IPlanRepository repository) : base(logger, repository)
        {
            _repository = repository;
        }

        public async Task<IList<PlanDTO>> GetPlans()
        {
            return await _repository.GetPlans();
        }
    }
}
