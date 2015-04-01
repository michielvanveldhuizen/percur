using Percurrentis.Context;
using Percurrentis.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Percurrentis.Controllers
{
    public class PictureController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Picture
        public ActionResult TravelDocument(string id)
        {

            RequestTraveller traveller = db.RequestTraveller.Where(r=>r.TravelDocument == id).FirstOrDefault();
            
            if (traveller != null)
            {
                if (traveller.TravelDocument != null)
                {
                    var dir = Server.MapPath("~/FileUpload/TravelDocuments");
                    var path = Path.Combine(dir, traveller.TravelDocument);
                    return base.File(path, "image/jpeg");
                }
            }
            
            return HttpNotFound();
            
        }

        public byte[] ReadImageFile(string imageLocation)
        {
            byte[] imageData = null;
            FileInfo fileInfo = new FileInfo(imageLocation);
            long imageFileLength = fileInfo.Length;
            FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            imageData = br.ReadBytes((int)imageFileLength);
            return imageData;
        }
        
    }
}