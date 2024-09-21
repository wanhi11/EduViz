using EduViz.Services;
using Microsoft.AspNetCore.Mvc;

namespace EduViz.Controllers;

[ApiController]
[Route("api/upgrade-vip")]
public class UpdateVIPController :ControllerBase
{
    public UpdateVIPController(UpgradeOrderDetailService upgradeService)
    {
        
    }
}