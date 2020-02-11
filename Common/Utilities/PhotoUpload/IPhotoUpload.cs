using System;
using System.Collections.Generic;
using System.Text;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Core.Utilities.PhotoUpload
{
    public interface IPhotoUpload
    {
        ImageUploadResult ImageUpload(IFormFile formFile);
    }
}
