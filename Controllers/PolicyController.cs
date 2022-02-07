using PolicyUploader.Extensions;
using PolicyUploader.Models;
using PolicyUploader.Models.ViewModels;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace PolicyUploader.Controllers
{
    public class PolicyController : Controller
    {
        public ActionResult Uploader()
        {
            return View();
        }

        public ActionResult Upload(PolicyUploadViewModel uploadViewModel)
        {
            if (ModelState.IsValid)
            {
                string fileName = uploadViewModel.PolicyFile.FileName;
                if (uploadViewModel.PolicyFile.ContentLength > 0)
                {
                    string[] filters = new string[] { "Policy no.", "Telephone", "Email id" };
                    string _path = System.IO.Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["PolicyFilePath"].ToString()), fileName);
                    uploadViewModel.PolicyFile.SaveAs(_path);
                    FileInfo file = new FileInfo(_path);
                    string pdfContent = uploadViewModel.PolicyFile.ExtractPdf(_path);
                    if (file.Exists)
                        file.Delete();
                    string policyFile = pdfContent.GetLinesBySearchTexts("Max Life Insurance").FirstOrDefault();
                    if (string.IsNullOrEmpty(policyFile))
                    {
                        ModelState.AddModelError("InvalidFile", "Please select valid policy file.");
                        uploadViewModel = null;
                        return View("Uploader", uploadViewModel);
                    }
                    List<string> lines = pdfContent.GetLinesBySearchTexts(filters);
                    uploadViewModel.Policy.PolicyNo = lines.Where(l => !string.IsNullOrEmpty(l) && l.Contains(":") && l.Contains(filters[0]))
                                                           .FirstOrDefault().Split(':').LastOrDefault();
                    uploadViewModel.Policy.Phone = lines.Where(l => !string.IsNullOrEmpty(l) && l.Contains(":") && l.Contains(filters[1]))
                                                        .FirstOrDefault().Split(':').LastOrDefault();
                    uploadViewModel.Policy.EmailID = lines.Where(l => !string.IsNullOrEmpty(l) && l.Contains(":") && l.Contains(filters[2]))
                                                          .FirstOrDefault().Split(':').LastOrDefault();
                }
            }
            return View("Uploader", uploadViewModel);
        }
    }
}