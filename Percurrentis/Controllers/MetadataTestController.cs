using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Linq;

using Percurrentis.Models;
using Breeze.WebApi2;
using Breeze.ContextProvider.EF6;
using Breeze.ContextProvider;
using System.Data.Entity.Validation;
using System.Net;
using System.Net.Http;
namespace Percurrentis.Controllers
{

    [BreezeController]
    public class MetadataTestController : ApiController
    {

        readonly EFContextProvider<DatabaseContext> _contextProvider =
            new EFContextProvider<DatabaseContext>();

        [HttpGet]
        public string Metadata()
        {
            return _contextProvider.Metadata();
        }

        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return _contextProvider.SaveChanges(saveBundle);
        }

        [HttpGet]
        public IQueryable<TravelRequest> TravelRequests()
        {
            return _contextProvider.Context.travelRequest;
        }
        //[httpget]
        //public iqueryable<breezesampletodoitem> todos()
        //{
        //    return _contextprovider.context.todos;
        //}

    }
}