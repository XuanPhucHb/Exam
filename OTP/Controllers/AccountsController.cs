using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using OTP.Data;
using OTP.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace OTP.Controllers
{
    public class AccountsController : Controller
    {
        private OTPContext db = new OTPContext();
        static List<Account> list = new List<Account>();

        // GET: Accounts
        public ActionResult Index()
        {
            return View(db.Accounts.ToList());
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Password,Email,Phone,Active,Otp")] Account account)
        {
            db.Accounts.Add(account);
            account.Otp = "7890";
            account.Active = 0;
            db.SaveChanges();
            list.Add(account);
            return RedirectToAction("AfterRegister");
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Password,Email,Phone,Active,Otp")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(account);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Activation([Bind(Include = "Username,Otp")] Account account)
        {
            foreach (var item in list)
            {
                if (item.Username == account.Username)
                {
                    if (account.Otp == item.Otp)
                    {
                        item.Active = 1;
                        Edit(item);
                        return RedirectToAction("Success");
                    }
                }
            }
            return RedirectToAction("Failed");

        }

        public static void sendSMS()
        {
            TwilioClient.Init(
                    Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID"),
                    Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN"));
            MessageResource.Create(
            to: new PhoneNumber("** YOUR PHONE NUMBER **"),
            from: new PhoneNumber("** YOUR TWILIO NUMBER **"),
            body: "Ahoy from Twilio!");
        }
    }

    //public static Message SendSms(DeliveryDetails details)
    //{
    //    var messageResult = new Message();
    //    try
    //    {
    //        if (details?.ToNumber != null)
    //        {
    //            var toNumberList = details.ToNumber.ToList();
    //            if (toNumberList.Count > 0)
    //            {
    //                foreach (var toNumber in toNumberList)
    //                {
    //                    messageResult = Twilio.SendMessage(FromNumber, toNumber, $"{details.Subject}\n\n{details.Message}");

    //                    if (messageResult == null)
    //                    {
    //                        logger.Error(string.Format(
    //                            "Error connecting to Twilio, message sending failed to {0}",
    //                            toNumber));
    //                    }
    //                    else if (messageResult.RestException != null)
    //                    {
    //                        logger.Error(string.Format("Twilio Error Message Description - {0}",
    //                            messageResult.RestException.Message));
    //                    }
    //                    else
    //                    {
    //                        logger.Info(String.Format("SMS {0} deliverd to {1}", messageResult.Body, messageResult.To));
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                Debug.WriteLine("ToNumber List Empty");
    //            }
    //        }
    //        else
    //        {
    //            Debug.WriteLine("ToNumber List Null");
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.WriteLine("error");
    //    }

    //    return messageResult;
    //}
}
