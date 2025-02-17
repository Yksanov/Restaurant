﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Restoraunt.Areas.Identity.Data;
using Restoraunt.BrainTree;
using Restoraunt.Data;
using System.Collections.Generic;
using System.IO;
using Restoraunt.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Restoraunt.Migrations;
using Restoraunt.Models.ViewModels;

namespace Restoraunt.Controllers
{
    public class ProductController : Controller
    {
        private readonly RestDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(RestDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Restoraunt.Models.Product> objList = _db.Product.Include(x => x.Category).Include(x => x.ApplicationType);
            return View(objList);
        }

        // GET - UPSERT
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Models.Product(),
                CategorySelectList = _db.Category.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                ApplicationTypeSelectList = _db.ApplicationType.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            if (id == null)
            {
                // this is for create
                return View(productVM);
            }
            else
            {
                // this is for edit
                productVM.Product = _db.Product.Find(id);

                if (productVM.Product == null)
                {
                    return NotFound();
                }

                return View(productVM);
            }
        }

        // POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productVM.Product.Id == 0)
                {
                    // create
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.Image = fileName + extension;
                    productVM.Product.Category = _db.Category.Find(productVM.Product.CategoryId);

                    _db.Product.Add(productVM.Product);
                }
                else
                {
                    // update
                    var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(x => x.Id == productVM.Product.Id);

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.Image = fileName + extension;
                    }
                    else
                    {
                        productVM.Product.Image = objFromDb.Image;
                    }
                    _db.Product.Update(productVM.Product);
                }

                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            productVM.CategorySelectList = _db.Category.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            productVM.ApplicationTypeSelectList = _db.ApplicationType.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            return View();
        }

        //DELETE
        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            Restoraunt.Models.Product product = _db.Product.Include(x => x.Category).Include(x => x.ApplicationType).FirstOrDefault(x => x.Id == id);
            
            if(product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST - DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Product.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;
            string file = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(file))
            {
                System.IO.File.Delete(file);
            }

            _db.Product.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
