using Christmas_Cards.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
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

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind()] CardModel cardModel, string New)
        {
            cardModel.Image.ImagePath = cardModel.Image.ImagePath.Remove(0, 23);
            cardModel.Image = db.Images.FirstOrDefault(i => i.ImagePath.EndsWith(cardModel.Image.ImagePath));
            
            if (ModelState.IsValid)
            {                 
                db.Cards.Add(cardModel);
                db.SaveChanges();
                if (!string.IsNullOrEmpty(New))
                {
                    return View("Index");
                }
                else
                {
                    return View("Email");
                }                
            }
            return View("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void ConvertToPdf(CardModel Card, EmailModel PersonalMail, string Font, Color color)
        {
            // Converting string to memorystream
            byte[] imgpth = Encoding.ASCII.GetBytes(Card.Image.ImagePath.Remove(0, 23));
            MemoryStream imgStream = new MemoryStream(imgpth);

            // converting string to memorystream
            byte[] Fontpth = Encoding.ASCII.GetBytes(Font);
            MemoryStream FontStream = new MemoryStream(Fontpth);

            //creating new Pdf file and page.
            PdfDocument document = new PdfDocument();

            PdfPage page = document.Pages.Add();

            PdfBitmap image = new PdfBitmap(imgStream);

            PdfGraphicsState state = page.Graphics.Save();

            // drawing the picture on the background of the Pdf
            page.Graphics.SetTransparency(0.0f);

            page.Graphics.DrawImage(image, new PointF(0,0), new SizeF(page.GetClientSize().Width, page.GetClientSize().Height));

            page.Graphics.Restore(state);

            // creating the brush and font type for the text
            PdfFont font = new PdfTrueTypeFont(FontStream, 0,0);

            PdfSolidBrush brush = new PdfSolidBrush(color);

            page.Graphics.DrawString($"{Card.Message}", font, brush, new PointF(0,0));

            MemoryStream stream = new MemoryStream();

            document.Save(stream);

            document.Close(true);

            stream.Position = 0;

            // Sending the email
            //Attachment file = new Attachment(stream, $"{Card.Id}", $"Christmas_Cards/pdf");
            //using (SmtpClient smtp = new SmtpClient($"{}"))
            //{
            //    MailMessage message = new MailMessage();
            //    message.From = new MailAddress("X-Mas-Cards@gmail.com");
            //    message.To.Add($"{PersonalMail.Email}");
            //    message.Subject = $"Some one send you an X-Mas Card {PersonalMail.FullName()}";
            //    message.Attachments.Add(file);
            //    message.IsBodyHtml = false;
            //    smtp.Send(message);
            //}
            
        }
    }
}
