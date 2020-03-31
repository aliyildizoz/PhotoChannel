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
        private CloudinaryDotNet.Cloudinary _cloudinary;
        public CloudinaryHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _cloudinaryOptions = _configuration.GetSection("CloudinaryOptions").Get<CloudinaryOptions>();
            Account account = new Account(_cloudinaryOptions.CloudName, _cloudinaryOptions.ApiKey, _cloudinaryOptions.ApiSecret);
            _cloudinary = new CloudinaryDotNet.Cloudinary(account);
        }

        public ImageUploadResult ImageUpload(IFormFile formFile)
        {
            ImageUploadResult result;
           
            using (var stream = formFile.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(formFile.FileName, stream)
                };
                result = _cloudinary.Upload(uploadParams);
            }
            return result;
        }

        public DeletionResult ImageDelete(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var deletionResult = _cloudinary.Destroy(deletionParams);
            return deletionResult;
        }
    }
}
