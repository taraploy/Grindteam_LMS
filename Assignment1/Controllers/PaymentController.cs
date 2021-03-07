using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment1.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        public ActionResult Balance()
        {
            return View("BalanceView");
        }
        
        public ActionResult PaymentInfo()
        {
            return View("PaymentInfoView");
        }
    }
}