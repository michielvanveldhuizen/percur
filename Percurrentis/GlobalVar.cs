using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Percurrentis
{
    public static class GlobalVar
    {
        //default companies
        public static readonly string[] defaultCompanies = { "CSi Industries", "CSi Romania", "CSi China", "Guest" };
        //GuidOfKees
        public static readonly string COOGuid = "a73d1a5e-b640-467e-8583-e4b52cfae437";
        //DevelopMode REMIND TO PUT IT ON FALSE BEFORE SETTING IT TO THE LIVE VERSION!
        public static readonly bool developMode = true;

        
    }
}