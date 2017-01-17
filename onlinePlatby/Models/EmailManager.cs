using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace onlinePlatby.Models
{
    public class EmailManager
    {
        // NOT TESTED YET

        private MailAddress from;
        private string fromPassword;
        private string secretForGoogleCatpcha;

        /// <summary>
        /// (const)
        /// </summary>
        /// <param name="from">You have to provide your "from" email</param>
        /// <param name="fromPassword">You have to provide password for your "from" email</param>
        public EmailManager(MailAddress from, string fromPassword, string secretForGoogleCatpcha = null)
        {
            this.from = from;
            this.fromPassword = fromPassword;
            this.secretForGoogleCatpcha = secretForGoogleCatpcha;
        }

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="to">Who do you want to send your email? MailAddress object</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="message">Your message (can be HTML if allowed)</param>
        /// <param name="isBodyHtml">(optional) Allow your email HTML?</param>
        public void SendEmail(MailAddress to, string subject, string message, bool isBodyHtml = true)
        {
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from.Address, fromPassword)
            };

            using (var message2 = new MailMessage(from, to)
            {
                Subject = subject,
                Body = message,
                BodyEncoding = Encoding.UTF8

            })
            {
                message2.IsBodyHtml = isBodyHtml;
                try
                {
                    smtp.Send(message2);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        /// <summary>
        /// Validates google catpcha in order to response
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public bool ValidateGoogleCatpcha(string response)
        {
            string secretKey;

            // is there any google captcha secret?
            if (string.IsNullOrEmpty(secretForGoogleCatpcha))
            {
                throw new ArgumentException("You have to provide secret for google catpcha. " +
                    "You can do it in constructor or with method AssignGoogleCaptchaSecret.");
            }
            else
            {
                secretKey = this.secretForGoogleCatpcha;
            }

            WebClient client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");
            return status;
        }

        /// <summary>
        /// Assings secret to google catpcha
        /// </summary>
        /// <param name="secret">your google catpcha secret</param>
        public void AssignGoogleCaptchaSecret(string secret)
        {
            this.secretForGoogleCatpcha = secret;
        }
    }
}