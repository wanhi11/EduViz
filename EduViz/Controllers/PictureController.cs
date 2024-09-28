using EduViz.Common.Payloads;
using EduViz.Common.Payloads.Request;
using EduViz.Common.Payloads.Response;
using EduViz.Exceptions;
using EduViz.Services;
using Microsoft.AspNetCore.Http.HttpResults;
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

    [HttpPost("test-upload-image-1")]
    public async Task<IActionResult> TestImage([FromForm] TestFileRequest1 req)
    {
        var uploadResult = await _cloudinaryService.UploadImageAsync(req.image);

        if (uploadResult.Error != null)
        {
            throw new BadRequestException("Failed to upload image.");
        }

        return Ok(ApiResult<TestFileResponse1>.Succeed(new TestFileResponse1()
        {
            name = req.name,
            url = uploadResult.SecureUrl.ToString()
        }));
    }
    

    [HttpPost("test-upload-image-2")]
    public async Task<IActionResult> TestImage2( IFormFile req)
    {
        var uploadResult = await _cloudinaryService.UploadImageAsync(req);

        if (uploadResult.Error != null)
        {
            throw new BadRequestException("Failed to upload image.");
        }

        return Ok(ApiResult<TestFileResponse1>.Succeed(new TestFileResponse1()
        {
            url = uploadResult.SecureUrl.ToString()
        }));
    }
}