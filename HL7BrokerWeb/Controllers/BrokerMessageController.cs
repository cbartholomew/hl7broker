using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HL7BrokerSuite.App.DAO;
using HL7BrokerSuite.App.Model;
using HL7BrokerSuite.Sys.DAO;
using HL7BrokerSuite.Sys.Model;

namespace HLBrokerWeb.Controllers
{
    public class BrokerMessageController : Controller
    {
        //
        // GET: /BrokerMessage/
        public ActionResult Index()
        {
            ViewBag.otherTitle = "Active Connections";

            // interface related titles
            // incoming interface should always show if it's connected or not
            ViewBag.incomingTitleInterface = "Incoming from Interface to Broker Service";

          
            ViewBag.outgoingTitleInterface = "Outgoing Interface from Broker Service (PASS THROUGH)";  
          
            // database related titles
            // this should always show the database connection status?
            ViewBag.outgoingTitleDatabase  = "Outgoing from Service to Broker Database";
            ViewBag.incomingTitleDatabase  = "Incoming from External Database";

            // web service titles
            // this is going to show what needs to be processed
            ViewBag.incomingTitleWebService = "Incoming from Broker Database to External Webservice";

            List<Broker> interfaceList = new List<Broker>();

            interfaceList = BrokerDAO.GetBrokerWorklist();
            interfaceList.RemoveAll(b => b.communicationTypeName == "DATABASE" || b.communicationTypeName == "WEBSERVICE");
            ViewBag.interfaceList = interfaceList;

            return View();
        }

        //
        // GET: /BrokerMessage/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /BrokerMessage/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BrokerMessage/Create
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
        // GET: /BrokerMessage/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /BrokerMessage/Edit/5
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
        // GET: /BrokerMessage/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /BrokerMessage/Delete/5
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
