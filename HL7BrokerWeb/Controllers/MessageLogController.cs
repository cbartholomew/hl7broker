using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HLBrokerWeb.Controllers
{
    public class MessageLogController : Controller
    {
        //
        // GET: /MessageLog/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /MessageLog/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /MessageLog/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /MessageLog/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /MessageLog/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /MessageLog/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /MessageLog/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /MessageLog/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
