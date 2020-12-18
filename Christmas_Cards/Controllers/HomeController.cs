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
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Web;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.EntityFrameworkCore;

namespace Christmas_Cards.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly AppDBContext db;

        public List<CardModel> cardslist = new List<CardModel>();

        private readonly IHttpContextAccessor _httpContextAccessor;

        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public HomeController(ILogger<HomeController> logger, AppDBContext db, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            this.db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Index()
        {
            CardModel cm = new CardModel { Image = new Images(), Emails = new List<EmailModel>() };
            return View(cm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Email()
        {
            if (_session.Keys.Count() > 0)
            {
                foreach (var key in _session.Keys)
                {
                    cardslist.Add((CardModel)ByteArrayWorks.ByteArrayToObject(_session.Get($"{key}")));
                }
            }
            return View(cardslist);
        }

        [HttpPost]
        public IActionResult Email(List<EmailModel> mails)
        {
            if (_session.Keys.Count() > 0)
            {
                foreach (var key in _session.Keys)
                {
                    cardslist.Add((CardModel)ByteArrayWorks.ByteArrayToObject(_session.Get($"{key}")));
                }
            }

            if (_session.Keys.Count() > 0)
            {
                foreach (var card in cardslist)
                {
                    //foreach (var mail in card.Emails)
                    //{

                    //}
                    EmailModel email = new EmailModel { Email = "", FirstName = "", LastName = "" };
                    string FontValueString = $"~/fonts/{card.FontType.GetType().GetEnumName(card.FontType)}";
                    ConvertToPdf(card, email, FontValueString);
                    //}
                }
            }
            return View("Index");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind()] CardModel cardModel, string New)
        {
            cardModel.Image.ImagePath = cardModel.Image.ImagePath.Remove(0, 23);
            cardModel.Image = db.Images.FirstOrDefault(i => i.ImagePath.EndsWith(cardModel.Image.ImagePath));


            if (ModelState.IsValid)
            {
                if (_session.Keys.Count() > 0)
                {
                    foreach (var key in _session.Keys)
                    {
                        cardslist.Add((CardModel)ByteArrayWorks.ByteArrayToObject(_session.Get($"{key}")));
                    }
                }

                cardslist.Add(cardModel);
                db.Cards.Add(cardModel);
                db.SaveChanges();

                foreach (var card in cardslist)
                {
                    _session.Set($"{card.Id}", ByteArrayWorks.ObjectToByteArray(card));
                }

                if (!string.IsNullOrEmpty(New))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Email");
                }
            }
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Email( IEnumerable<CardModel> cardModels)
        {
            
            if (ModelState.IsValid)
            {

            }
            
            return View("Send");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void ConvertToPdf(CardModel Card, EmailModel PersonalMail, string Font)
        {
            string FontAdd = Font.Remove(0, 1);
            FontAdd = $"wwwroot{FontAdd}.ttf";

            string tempPt = Card.FontSize.ToString().Remove(0, 2);

            int FontPt = int.Parse(tempPt);

            string tempstr = Card.Image.ImagePath;

            tempstr = $"wwwroot{tempstr}";

            //creating new Pdf file and page.
            PdfDocument document = new PdfDocument();

            PdfPage page = document.Pages.Add();

            using (var sourceimg = System.IO.File.OpenRead($"{tempstr}"))
            {
                using (var sourcefont = System.IO.File.OpenRead($"{FontAdd}"))
                {
                    PdfBitmap image = new PdfBitmap(sourceimg);

                    PdfGraphicsState state = page.Graphics.Save();

                    // drawing the picture on the background of the Pdf
                    page.Graphics.SetTransparency(1);

                    page.Graphics.DrawImage(image, new PointF(0, 0.2f), new SizeF(page.GetClientSize().Width, page.GetClientSize().Height));

                    page.Graphics.Restore(state);

                    // creating the brush and font type for the text
                    PdfFont font = new PdfTrueTypeFont(sourcefont, FontPt);

                    PdfColor color = new PdfColor(System.Drawing.ColorTranslator.FromHtml($"{Card.FontColour}").ToArgb());

                    PdfSolidBrush brush = new PdfSolidBrush(color);

                    PdfStringFormat format = new PdfStringFormat();

                    format.Alignment = PdfTextAlignment.Center;

                    page.Graphics.DrawString($"{Card.Message}", font, brush, new PointF(page.GetClientSize().Width/2, FontPt), format);

                    MemoryStream stream = new MemoryStream();

                    document.Save(stream);

                    document.Close(true);

                    stream.Position = 0;

                    // Sending the email
                    Attachment file = new Attachment(stream, $"{Card.Id}.pdf", $"Christmas_Cards/pdf");

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {

                        smtp.Credentials = new NetworkCredential();
                        smtp.EnableSsl = true;
                        MailMessage message = new MailMessage();
                        message.From = new MailAddress("emailservicewebservice@gmail.com");
                        message.To.Add($"{PersonalMail.Email}");
                        message.Subject = $"Some one send you an X-Mas Card {PersonalMail.FullName()}";
                        message.Attachments.Add(file);
                        message.IsBodyHtml = false;
                        message.Body = "Someone send you an anonymous christmascard!" + Environment.NewLine + $"Hope you enjoy {PersonalMail.FullName()}";
                        smtp.Send(message);
                    }
                }
            }

        }
    }
}
