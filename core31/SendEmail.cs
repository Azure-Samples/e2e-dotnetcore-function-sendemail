using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;

namespace GeneralFuncDotNetCoreSmtp
{
    public static class SendEmail
    {

        [FunctionName("SendEmail")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "sendemail")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("sendemail");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            log.LogInformation("sendemail received data");

            string toEmail = data?.toEmail;
            string textSubject = data?.textSubject;
            string textBody = data?.textBody;
            string fromAccountEmail = data?.fromAccountEmail; 
            string fromAccountPassword = data?.fromAccountPassword;
            string fromAccountDomain = data?.fromAccountDomain; // 'microsoft.com' for Microsoft internal teams
            string smtpHost = data?.smtpHost; // Office 365 - "smtp.office365.com"
            string smtpPort = data?.smtpPort; // Office 365 - 25 or 587


            log.LogInformation("sendemail fetched variables");

            if (String.IsNullOrEmpty(toEmail)) throw new ArgumentException("sendemail failed to get variable toEmail");
            if (String.IsNullOrEmpty(textSubject)) throw new ArgumentException("sendemail failed to get variable textSubject");
            if (String.IsNullOrEmpty(textBody)) throw new ArgumentException("sendemail failed to get variable textBody");
            if (String.IsNullOrEmpty(fromAccountEmail)) throw new ArgumentException("sendemail failed to get variable fromAccountEmail");
            if (String.IsNullOrEmpty(fromAccountPassword)) throw new ArgumentException("sendemail failed to get variable fromAccountPassword");
            if (String.IsNullOrEmpty(fromAccountDomain)) throw new ArgumentException("sendemail failed to get variable fromAccountDomain");
            if (String.IsNullOrEmpty(smtpHost)) throw new ArgumentException("sendemail failed to get variable smtpHost");
            if (String.IsNullOrEmpty(smtpPort)) throw new ArgumentException("sendemail failed to get variable smtpPort");

            log.LogInformation("sendemail got variables");

            try
            {

                SmtpClient client = new SmtpClient();
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(fromAccountEmail, fromAccountPassword, fromAccountDomain);
                client.Port = Int32.Parse(smtpPort);
                client.Host = smtpHost;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;

                log.LogInformation("sendemail client constructed");

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromAccountEmail);
                mail.To.Add(toEmail);
                mail.Subject = textSubject;
                mail.Body = textBody;

                log.LogInformation("sendemail mail constructed");

                client.Send(mail);

                log.LogInformation("sendemail mail sent");

                return new OkObjectResult(true);
            }
            catch (Exception e)
            {
                log.LogInformation("sendemail failed " + e.Message);
                System.Diagnostics.Trace.TraceError(e.Message);
                var errorObjectResult = new ObjectResult(e.Message);
                errorObjectResult.StatusCode = StatusCodes.Status500InternalServerError;

                return errorObjectResult;
            }

            
        }
    }
}
