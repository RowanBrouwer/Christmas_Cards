using Christmas_Cards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Christmas_Cards.DAL
{
    public class AppDBContextInit
    {
        public static void Seed(AppDBContext db)
        {
            List<Images> images = new List<Images>();

            if (db.Images.Count() == 0)
            {
                Images image1 = new Images()
                {
                    ImagePath = "/Images/1.png"
                };
                images.Add(image1);
                Images image2 = new Images()
                {
                    ImagePath = "/Images/2.png"
                };
                images.Add(image2);
                Images image3 = new Images()
                {
                    ImagePath = "/Images/3.png"
                };
                images.Add(image3);
                Images image4 = new Images()
                {
                    ImagePath = "/Images/4.png"
                };
                images.Add(image4);
                Images image5 = new Images()
                {
                    ImagePath = "/Images/5.png"
                };
                images.Add(image5);
                Images image6 = new Images()
                {
                    ImagePath = "/Images/6.png"
                };
                images.Add(image6);
                Images WhiteSpace = new Images()
                {
                    ImagePath = "/Images/WhiteSpace.png"
                };
                images.Add(WhiteSpace);
                foreach (var img in images)
                {
                    db.Add(img);
                }
                db.SaveChanges();
            }

            ICollection<CardModel> Cards = new List<CardModel>();
            ICollection<EmailModel> seedEmails = new List<EmailModel>();
            ICollection<EmailModel> TempEmails = new List<EmailModel>();

            if (db.Emails.FirstOrDefault(e => e.Email == "Brouwerrowan@gmail.com") == null)
            {
                EmailModel mail1 = new EmailModel()
                {
                    Email = "Brouwerrowan@gmail.com",
                    FirstName = "Rowan",
                    LastName = "Brouwer"
                };
                seedEmails.Add(mail1);

                foreach (var mail in seedEmails)
                {
                    db.Add(mail);
                    TempEmails.Add(mail);
                }

                db.SaveChanges();

                if (db.Cards.Count() == 0)
                {
                    CardModel card1 = new CardModel()
                    {
                        FontType = FontType.Christmabet,
                        Message = "Hello from the organised side",
                        Emails = TempEmails,
                        Image = db.Images.FirstOrDefault(i => i.Id == 2),
                    };
                    Cards.Add(card1);

                    foreach (var card in Cards)
                    {
                        db.Add(card);
                    }
                }
            }
            db.SaveChanges();

        }
    }
}
