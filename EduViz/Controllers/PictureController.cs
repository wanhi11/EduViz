using EduViz.Common.Payloads;
using EduViz.Common.Payloads.Response;
using EduViz.Exceptions;
using EduViz.Services;
using Microsoft.AspNetCore.Mvc;

namespace EduViz.Controllers;
[ApiController]
[Route("api/picture")]
public class PictureController: ControllerBase
{
    private readonly CloudinaryService _cloudinaryService;

    public PictureController(CloudinaryService cloudinaryService)
    {
        _cloudinaryService = cloudinaryService;
    }

    [HttpPost("upload-image")]
    public async Task<IActionResult> UploadImage( IFormFile? image)
    {
        string returnUrl = "";
        if (image is not null)
        {
            var uploadResult = await _cloudinaryService.UploadImageAsync(image);

            if (uploadResult.Error != null)
            {
                throw new BadRequestException("Failed to upload image.");
            }

            returnUrl =uploadResult.SecureUrl.ToString();
        }

        return Ok(ApiResult<UploadImageResponse>.Succeed(new UploadImageResponse()
        {
            imageUrl = returnUrl
        }));
    }

}