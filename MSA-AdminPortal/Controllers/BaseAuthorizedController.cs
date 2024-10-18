using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MSA_AdminPortal.Controllers
{
    [MSA_Authorize]
    public class BaseAuthorizedController : BaseController
    {
    }
}
