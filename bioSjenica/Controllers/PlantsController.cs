using bioSjenica.CustomMappers;
using Microsoft.AspNetCore.Mvc;

namespace bioSjenica.Controllers {
  [Controller]
  public class PlantsController:ControllerBase {
    public PlantsController(IPlantMapper plantMapper)
    {
      
    }
  }
}