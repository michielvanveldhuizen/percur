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
        //found at TravelAgency/api/FileUpload
        public HttpResponseMessage Post()
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            //For the file upload
            if (httpRequest.Files.Count > 0)
            {
                string fileName = "";
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];

                    //getting a hash as name so it will always be a unique and unguessable name
                    string[] fileNameSplit = postedFile.FileName.Split('.');
                    string hash = GetHashString(DateTime.Now.ToString("yyyyMMddHHmmssffff"));

                    fileName = hash+"."+fileNameSplit[1];

                    //check if it already exists, ifso get new a hash
                    while (File.Exists(fileName))
                    {
                        fileName = GetHashString(DateTime.Now.ToString("yyyyMMddHHmmssffff")) +"." + fileNameSplit[1];
                    }

                    //saving the file
                    var filePath = HttpContext.Current.Server.MapPath("~/FileUpload/TravelDocuments/"+fileName);
                    postedFile.SaveAs(filePath);
                }
                result = Request.CreateResponse(HttpStatusCode.Created,fileName);
            }//For the deleting of the old file
            else if (!string.IsNullOrEmpty(httpRequest.Form["delID"]))
            {
                string delID = httpRequest.Form["delID"];
                try { 
                    //Deleting the file
                    File.Delete(HttpContext.Current.Server.MapPath("~/FileUpload/TravelDocuments/" + delID));
                    result = Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    result = Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                //Neither fileUpload or File delete = badRequest
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return result;
        }
        
        //Could be set to some kind of lib but currently these are like the only two random methods like this.
        //to create the hash
        public byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA1.Create(); 
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        //to create the hash
        public string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }
    }

    
}
