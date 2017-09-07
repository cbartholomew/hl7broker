using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HLBrokerWeb.Controllers
{
    public class ConnectionsController : Controller
    {
        //
        // GET: /Connections/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Connections/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Connections/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Connections/Create
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
        // GET: /Connections/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Connections/Edit/5
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
        // GET: /Connections/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Connections/Delete/5
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
