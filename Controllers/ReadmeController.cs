using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RefactorMe.Controllers
{
    public class ReadmeController : Controller
    {
        // GET: Readme
        public ActionResult Index()
        {
            string readme = "~/Content/Readme/RefactorMe Web API.pdf";
            return File(readme, "application/pdf");
        }
    }
}