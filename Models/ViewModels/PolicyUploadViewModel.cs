using PolicyUploader.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PolicyUploader.Models.ViewModels
{
    public class PolicyUploadViewModel
    {
        public PolicyUploadViewModel()
        {
            Policy = new PolicyDetailModel();
        }

        [Required(ErrorMessage = "Please select a file for upload.")]
        [AllowedFileExtensions]
        [Display(Name = "Policy File")]
        public HttpPostedFileBase PolicyFile { get; set; }
        public PolicyDetailModel Policy { get; set; }
    }
}