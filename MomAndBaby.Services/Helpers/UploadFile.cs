using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace MomAndBaby.Services.Helpers
{
    public class UploadFile
    {
        private readonly Cloudinary _cloudinary;

        public UploadFile(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<string> UploadImageAsync(Stream fileStream, string fileName)
        {
            if (fileStream == null || fileStream.Length == 0)
            {
                throw new Exception("File is invalid!");
            }

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, fileStream),
                PublicId = $"uploads/{Guid.NewGuid()}"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult == null || uploadResult.SecureUrl == null)
            {
                throw new Exception("Upload fail, check API Key or file upload again!");
            }

            return uploadResult.SecureUrl.AbsoluteUri;
        }
    }
}
