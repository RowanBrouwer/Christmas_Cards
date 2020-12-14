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
using System.Threading.Tasks;

namespace Christmas_Cards.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void ConvertToPdf()
        {
            PdfDocument document = new PdfDocument();

            PdfPage page = document.Pages.Add();

            PdfBitmap image = new PdfBitmap($"{}");

            PdfGraphicsState state = page.Graphics.Save();

            page.Graphics.SetTransparency(0.2f);

            page.Graphics.DrawImage(image, new PointF(0,0), new SizeF(page.GetClientSize().Width, page.GetClientSize().Height));

            page.Graphics.Restore(state);

            PdfFont font = new PdfTrueTypeFont($"{}");

            PdfSolidBrush brush = new PdfSolidBrush($"{}");

            page.Graphics.DrawString($"{}", font, brush, new PointF(0,0));

            MemoryStream stream = new MemoryStream();

            document.Save(stream);

            document.Close(true);

            stream.Position = 0;

            Attachment file = new Attachment(stream, $"{}", $"{}");
            using (SmtpClient smtp = new SmtpClient($"{}"))
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress("X-Mas-Cards@gmail.com");
                message.To.Add("");
                message.Subject = "Some one send you an X-Mas Card!";
                message.Attachments.Add(file);
                message.IsBodyHtml = false;
                smtp.Send(message);
            }
            
        }
    }
}
