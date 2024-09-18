using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EduViz.Exceptions;

namespace EduViz.Services;

public class CloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IConfiguration configuration)
    {
        var account = new Account(configuration["Cloudinary:CloudName"],
            configuration["Cloudinary:ApiKey"],
            configuration["Cloudinary:ApiSecret"]);
        _cloudinary = new Cloudinary(account);
        _cloudinary.Api.Secure = true;
    }

    public async Task<ImageUploadResult> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0 ||
            (file.ContentType != "image/png" && file.ContentType != "image/jpeg"))
        {
            throw new BadRequestException("File is null, empty, or not in PNG or JPEG format.");
        }

        using (var stream = file.OpenReadStream())
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                UploadPreset = "EduViz"
            };

            try
            {
                return await _cloudinary.UploadAsync(uploadParams);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to upload image to Cloudinary.", ex);
            }
        }
    }


    public async Task<List<ImageUploadResult>> UploadImagesAsync(List<IFormFile> files)
    {
        var uploadResults = new List<ImageUploadResult>();

        foreach (var file in files)
        {
            uploadResults.Add(await UploadImageAsync(file));
        }

        return uploadResults;
    }
}