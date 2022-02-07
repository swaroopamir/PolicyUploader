using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace PolicyUploader.Extensions
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AllowedFileExtensionsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            List<string> acceptedExtension = ConfigurationManager.AppSettings["PolicyFileExtension"].Split(',').ToList();
            var policyFile = value as HttpPostedFileBase;
            if (policyFile == null)
            {
                return new ValidationResult("Please select a file to Upload.");
            }
            else if (!acceptedExtension.Contains(policyFile.FileName.Substring(policyFile.FileName.LastIndexOf("."))))
            {
                return new ValidationResult("Please upload of type " + string.Join(", ", acceptedExtension));
            }
            return ValidationResult.Success;

        }
    }
}