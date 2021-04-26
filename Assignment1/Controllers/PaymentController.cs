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
using Newtonsoft.Json;
using Stripe;

namespace Assignment1.Controllers
{
    public class PaymentController : Controller
    {
        LMS_GRINDEntities1 gds = new LMS_GRINDEntities1();

        public ActionResult Balance()
        {
            var semesterCredits = (from c in gds.Courses
                                join sc in gds.StudentCourses
                                on c.course_id equals sc.course_id
                                where sc.student_id == Name.user_id
                                select c).ToList();

            var payments = (from p in gds.Payments
                            where p.user_id == Name.user_id
                            select p).ToList();
            

            ViewBag.Payments = payments;

            return View("BalanceView", semesterCredits);
        }

        /// <summary>
        /// Make Payment
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<ActionResult> MakePayment(FormCollection form)
        {
            HttpClient client = new HttpClient();
            string date = form["expiration"];
            int month = int.Parse(date.Substring(5));
            int year = int.Parse(date.Substring(0, 4));
            string amount = form["payment_amount"];
            string paymentAmount = (double.Parse(amount) * 100).ToString();
            long amt = long.Parse(paymentAmount);

            try
            {
                StripeConfiguration.ApiKey = "sk_test_51INLgeIR2cxnZNWwi0AHsRoFifYouOH6QcLsIxEEOmBwXNyXXXX4Jt2MOHbR6d5KKMotvMYyrKa1AtrBpcc8q8IB009h5ew7Iw";

                var optionstoken = new TokenCreateOptions
                {
                    Card = new TokenCardOptions
                    {
                        Number = form["card_number"],
                        ExpMonth = month,
                        ExpYear = year,
                        Cvc = form["cvc"]
                    }
                };

                var servicetoken = new TokenService();
                Token stripetoken = await servicetoken.CreateAsync(optionstoken);

                var options = new ChargeCreateOptions
                {
                    Amount = amt,
                    Currency = "usd",
                    Description = "Payment from " + Name.last_name + ", " + Name.first_name,
                    Source = stripetoken.Id
                };

                var service = new ChargeService();
                Charge charge = await service.CreateAsync(options);

                if (charge.Paid)
                {
                    //update database
                    Payment p = new Payment();

                    p.payment_amount = decimal.Parse(amount);
                    p.user_id = Name.user_id;
                    p.payment_date = DateTime.Now;
                    gds.Payments.Add(p);
                    gds.SaveChanges();

                    return Balance();
                }
                else
                {
                    return View("PaymentInfoView");
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public ActionResult PaymentInfo()
        {
            decimal semesterTotal = 0;

            var semesterCredits = (from c in gds.Courses
                                   join sc in gds.StudentCourses
                                   on c.course_id equals sc.course_id
                                   where sc.student_id == Name.user_id
                                   select c).ToList();

            var payments = (from p in gds.Payments
                            where p.user_id == Name.user_id
                            select p).ToList();

            foreach (var c in semesterCredits)
            {
                semesterTotal += (decimal)(c.num_credits * 100);
            }

            foreach (var p in payments)
            {
                semesterTotal -= (decimal)p.payment_amount;
            }

            ViewBag.Balance = semesterTotal;

            return View("PaymentInfoView");
        }

        public ActionResult PaymentActivity(int studentId)
        {
            return View("BalanceView");
        }
    }
}
