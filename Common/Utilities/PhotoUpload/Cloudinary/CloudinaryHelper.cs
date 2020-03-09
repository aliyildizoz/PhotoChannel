using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;

namespace Core.Utilities.PhotoUpload.Cloudinary
{
    public class CloudinaryHelper : IPhotoUpload
    {
        private IConfiguration _configuration;
        private CloudinaryOptions _cloudinaryOptions;
        public CloudinaryHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _cloudinaryOptions = _configuration.GetSection("CloudinaryOptions").Get<CloudinaryOptions>();
        }

        public ImageUploadResult ImageUpload(IFormFile formFile)
        {
            ImageUploadResult result;
            Account account = new Account(_cloudinaryOptions.CloudName, _cloudinaryOptions.ApiKey, _cloudinaryOptions.ApiSecret);
            CloudinaryDotNet.Cloudinary cloudinary = new CloudinaryDotNet.Cloudinary(account);
            using (var stream = formFile.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(formFile.FileName, stream)
                };
                result = cloudinary.Upload(uploadParams);
            }
            return result;
        }
    }
}
