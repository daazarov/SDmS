using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Configuration;
using SendGrid.Helpers.Mail;
using SendGrid;
using SDmS.Identity.Core.Interfaces.Services;
using System;

namespace SDmS.Identity.Core.Services
{
    public class EmailService : IIdentityEmailService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await configSendGridasync(message);
        }

        // Use NuGet to install SendGrid (Basic C# client lib) 
        private async Task configSendGridasync(IdentityMessage message)
        {
            try
            {
                var from = new EmailAddress(ConfigurationManager.AppSettings["emailService:Sender"], "Dmitriy Azarov");
                var subject = message.Subject;
                var to = new EmailAddress(message.Destination);
                var htmlContent = message.Body;

                var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
                //myMessage.HtmlContent = "Test";

                // Create a Web transport for sending email.
                var client = new SendGridClient(ConfigurationManager.AppSettings["emailService:ApiKey"]);

                // Send the email.
                var response = await client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
