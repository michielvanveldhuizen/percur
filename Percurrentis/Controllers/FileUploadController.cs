using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;

namespace Percurrentis.Controllers
{
    public class FileUploadController : ApiController
    {
        public HttpResponseMessage Post()
        {
             HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                string fileName = "";
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];

                    string[] fileNameSplit = postedFile.FileName.Split('.');
                    string hash = GetHashString(DateTime.Now.ToString("yyyyMMddHHmmssffff"));

                    fileName = hash+"."+fileNameSplit[1];

                    var filePath = HttpContext.Current.Server.MapPath("~/FileUpload/TravelDocuments/"+fileName);
       
                    postedFile.SaveAs(filePath);
                }
                result = Request.CreateResponse(HttpStatusCode.Created,fileName);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return result;
        }


        public byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA1.Create(); 
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }

    
}
