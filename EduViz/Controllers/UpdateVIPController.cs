using EduViz.Services;
using Microsoft.AspNetCore.Mvc;

namespace EduViz.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UpdateVIPController :ControllerBase
{
    public UpdateVIPController(UpgradeOrderDetailService upgradeService)
    {
        
    }
}