using Laptop_Technology.Models;
using Laptop_Technology.Models.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;

namespace Laptop_Technology.Controllers
{
    [Authorize]
    public class LaptopsController : Controller
    {
        private readonly LaptopDbContext db = new LaptopDbContext();
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public PartialViewResult LaptopDetails(int pg = 1)
        {
            var data = db.Laptops
                         .Include(x=>x.Stocks)
                         .Include(x => x.LaptopModel)
                         .Include(x => x.Brand)
                         .OrderBy(x => x.LaptopId)
                         .ToPagedList(pg, 10);
            return PartialView("_LaptopDetails", data);
        }

        public ActionResult Create()
        {
            return View();
        }
        public ActionResult CreateForm()
        {
            LaptopInputModel model = new LaptopInputModel();
            model.Stocks.Add(new Stock());
            ViewBag.LaptopModels = db.LaptopModels.ToList();
            ViewBag.Brands = db.Brands.ToList();
            return PartialView("_CreateForm", model);
        }
        [HttpPost]
        public ActionResult Create(LaptopInputModel model, string act = "")
        {
            if (act == "add")
            { 
                model.Stocks.Add(new Stock());
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }
            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                model.Stocks.RemoveAt(index);
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }
            if (act == "insert")
            {
                if (ModelState.IsValid)
                {
                    var Laptop = new Laptop
                    {
                        BrandId = model.BrandId,
                        LaptopModelId = model.LaptopModelId,
                        Name = model.Name,
                        FirstIntroduceOn = model.FirstIntroduceOn,
                        OnSale = model.OnSale
                    };
                    //Image
                    string ext = Path.GetExtension(model.Picture.FileName);
                    string f = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                    string savePath = Path.Combine(Server.MapPath("~/Images"), f);
                    model.Picture.SaveAs(savePath);
                    Laptop.Picture = f;

                    db.Laptops.Add(Laptop);
                    db.SaveChanges();
                    //Stocks
                    foreach (var s in model.Stocks)
                    {
                        db.Database.ExecuteSqlCommand($"spInsertStock {(int)s.Size},{s.Price},{(int)s.Quantity},{Laptop.LaptopId}");
                    }
                    LaptopInputModel newModel = new LaptopInputModel()
                    {
                        Name = "",
                        FirstIntroduceOn = DateTime.Today
                    };
                    newModel.Stocks.Add(new Stock());

                    ViewBag.LaptopModels = db.LaptopModels.ToList();
                    ViewBag.Brands = db.Brands.ToList();
                    foreach (var e in ModelState.Values)
                    {
                        e.Value = null;
                    }
                    return View("_CreateForm", newModel);
                }
            }
            ViewBag.LaptopModels = db.LaptopModels.ToList();
            ViewBag.Brands = db.Brands.ToList();
            return View("_CreateForm", model);
        }
        public ActionResult Edit(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public ActionResult EditForm(int id)
        {
            var data = db.Laptops.FirstOrDefault(x => x.LaptopId == id);
            if (data == null) return new HttpNotFoundResult();
            db.Entry(data).Collection(x => x.Stocks).Load();
            LaptopEditModel model = new LaptopEditModel
            {
                LaptopId = id,
                BrandId = data.BrandId,
                LaptopModelId = data.LaptopModelId,
                Name = data.Name,
                FirstIntroduceOn = data.FirstIntroduceOn,
                OnSale = data.OnSale,
                Stocks = data.Stocks.ToList()
            };
            ViewBag.LaptopModels = db.LaptopModels.ToList();
            ViewBag.Brands = db.Brands.ToList();
            ViewBag.CurrentPic = data.Picture;
            return PartialView("_EditForm", model);
        }
        [HttpPost]
        public ActionResult Edit(LaptopEditModel model, string act = "")
        {
            if (act == "add")
            {
                model.Stocks.Add(new Stock());
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }
            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                model.Stocks.RemoveAt(index);
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }
            if (act == "update")
            {
                if (ModelState.IsValid)
                {
                    var laptop = db.Laptops.FirstOrDefault(x => x.LaptopId == model.LaptopId);
                    if (laptop == null) { return new HttpNotFoundResult(); }
                    laptop.Name = model.Name;
                    laptop.FirstIntroduceOn = model.FirstIntroduceOn;
                    laptop.OnSale = model.OnSale;
                    laptop.BrandId = model.BrandId;
                    laptop.LaptopModelId = model.LaptopModelId;
                    if (model.Picture != null)
                    {
                        string ext = Path.GetExtension(model.Picture.FileName);
                        string f = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                        string savePath = Path.Combine(Server.MapPath("~/Images"), f);
                        model.Picture.SaveAs(savePath);
                        laptop.Picture = f;
                    }
                    else
                    {

                    }

                    db.SaveChanges();
                    db.Database.ExecuteSqlCommand($"EXEC DeleteStockByLaptopId {laptop.LaptopId}");
                    foreach (var s in model.Stocks)
                    {
                        db.Database.ExecuteSqlCommand($"EXEC spInsertStock {(int)s.Size}, {s.Price}, {s.Quantity}, {laptop.LaptopId}");
                    }
                }
            }
            ViewBag.LaptopModels = db.LaptopModels.ToList();
            ViewBag.Brands = db.Brands.ToList();
            ViewBag.CurrentPic = db.Laptops.FirstOrDefault(x => x.LaptopId == model.LaptopId)?.Picture;
            return View("_EditForm", model);
        }
        public ActionResult Delete(int? id)
        {
            var laptop = db.Laptops.Find(id);
            if(laptop != null)
            {
                var stock = db.Stocks.Where(x => x.LaptopId == id);
                db.Stocks.RemoveRange(stock);
                db.Laptops.Remove(laptop);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        

    }
}