using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BuyFood_Template.Models
{
    public class ShareFunction
    {
        public string 產生亂數(int length)
        {
            var str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var next = new Random();
            var builder = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                builder.Append(str[next.Next(0, str.Length)]);
            }
            return builder.ToString();
        }

        public void sendGrid(string mailtoAddress, string mailtoName, string subject, string body)
        {
 
            //var apiKey = System.Environment.GetEnvironmentVariable("buyfood_API");
            var api = "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";

            var client = new SendGridClient(api); //api應該可以直接寫死

            SendGridMessage msg = new SendGridMessage();
            msg.SetFrom("sunfengmsit129@gmail.com", "擺腹buyfood");
            msg.AddTo(mailtoAddress, mailtoName);
            msg.SetSubject(subject);
            msg.AddContent(MimeType.Text, body);
            

            var response = client.SendEmailAsync(msg);



        }
        //public void sendEmail(string mailtoAddress,string mailtoName, string subject, string body)
        //{
        //    var fromAddress = new MailAddress("sunfengmsit129@gmail.com", "擺腹buyfood");
        //    var toAddress = new MailAddress(mailtoAddress, mailtoName);
        //    const string fromPassword = "@MSIT129";
        //    //const string subject = "Subject";
        //    //const string body = "Body";

        //    var smtp = new SmtpClient
        //    {
        //        Host = "smtp.gmail.com",
        //        Port = 587,
        //        EnableSsl = true,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
        //    };
        //    using (var message = new MailMessage(fromAddress, toAddress)
        //    {
        //        Subject = subject,
        //        Body = body
        //    })
        //    {
        //        smtp.Send(message);
        //    }
        //}
        //hashAlgorithm = 加密方法 SHA1 SHA256等等
        //input = 欲加密字串
        public string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

    }
}
