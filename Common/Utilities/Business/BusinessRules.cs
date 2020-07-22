using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;

namespace Core.Utilities.Business
{
    public class BusinessRules
    {
        public static IResult Run(params IResult[] logics)
        {
            foreach (IResult result in logics)
            {
                if (!result.IsSuccessful)
                {
                    return result;
                }
            }

            return new SuccessResult();
        }
        public static IResult ImageExtensionValidate(string imageExtension)
        {
            string[] extensions = { "png", "jpg", "jpeg" };
            foreach (string extension in extensions)
            {
                if (extension == imageExtension)
                {
                    return new ErrorResult();
                }
            }
            return new SuccessResult();
        }
    }
}
