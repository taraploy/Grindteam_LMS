using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Assignment1.Models;

using System.Net.Http;
using System.Net.Http.Headers;

using System.Threading;
using System.Threading.Tasks;


namespace Assignment1.Controllers
{
    public class PaymentController : Controller
    {
        LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();

        // GET: Payment
        //public ActionResult Balance()
        public async Task<ActionResult> Balance()
        {
            var semesterCredits = (from c in gds.Courses
                                join sc in gds.StudentCourses
                                on c.course_id equals sc.course_id
                                where sc.student_id == Name.user_id
                                select c).ToList();

            // use NEWTONSOFT 

            HttpClient client = new HttpClient();

            var cardValues = new Dictionary<string, string>
            {
                { "card[number]", "card number" },
                { "card[exp_month]", "exp month"},
                { "card[exp_year]", "exp year" },
                { "card[cvc]", "cvt"}
            };

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "Your key that starts with sk_");

            var cardContent = new FormUrlEncodedContent(cardValues);

            var cardResponse = await client.PostAsync("https://api.stripe.com/v1/tokens", cardContent);


            var cardResponseString = await cardResponse.Content.ReadAsStringAsync();


            return View("BalanceView", semesterCredits);
        }
        
        public ActionResult PaymentInfo()
        {
            return View("PaymentInfoView");
        }
    }
}