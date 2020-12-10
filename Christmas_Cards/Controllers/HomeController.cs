﻿using Christmas_Cards.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Christmas_Cards.DAL;


namespace Christmas_Cards.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly AppDBContext db;
        

        public HomeController(ILogger<HomeController> logger, AppDBContext db) 
        {
            _logger = logger;
            this.db = db;
        }

        public IActionResult Index()
        {
            CardModel cm = new CardModel { Image = new Images(), Emails = new List<EmailModel>()};
            return View(cm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //Include = "Id,FirstName,InFix,LastName,Wage"

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind()] CardModel cardModel)
        {

            cardModel.Image.ImagePath = cardModel.Image.ImagePath.Remove(0, 23);

            cardModel.Image = db.Images.FirstOrDefault(i => i.ImagePath.EndsWith(cardModel.Image.ImagePath));

            
            if (ModelState.IsValid)
            { 
                
                db.Cards.Add(cardModel);
                db.SaveChanges();
                return RedirectToAction("Privacy");
            }

            return View("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
