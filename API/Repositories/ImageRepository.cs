using API.Repositories.Interface;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Net;

namespace API.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly IConfiguration configuration;
        private readonly Account account;
        public ImageRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            account = new Account(configuration.GetSection("Cloudinary")["CloudName"],
                configuration.GetSection("Cloudinary")["APIKey"],
                configuration.GetSection("Cloudinary")["APISecret"]);

        }
        public async Task<string> UploadAsync(IFormFile file)
        {
            var client = new Cloudinary(account);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                DisplayName = file.FileName,
            };

            var uploadResult = await client.UploadAsync(uploadParams);

            if (uploadResult != null && uploadResult.StatusCode == HttpStatusCode.OK)
            {
                return uploadResult.Url.ToString();
            }

            return null;
        }
    }
}
