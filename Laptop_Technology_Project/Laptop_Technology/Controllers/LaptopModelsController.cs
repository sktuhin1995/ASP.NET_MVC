using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Laptop_Technology.Models;

namespace Laptop_Technology.Controllers
{
    public class LaptopModelsController : Controller
    {
        private LaptopDbContext db = new LaptopDbContext();

        public ActionResult Index()
        {
            return View(db.LaptopModels.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LaptopModelId,ModelName")] LaptopModel laptopModel)
        {
            if (ModelState.IsValid)
            {
                db.LaptopModels.Add(laptopModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(laptopModel);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LaptopModel laptopModel = db.LaptopModels.Find(id);
            if (laptopModel == null)
            {
                return HttpNotFound();
            }
            return View(laptopModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LaptopModel laptopModel = db.LaptopModels.Find(id);
            db.LaptopModels.Remove(laptopModel);
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
    }
}
