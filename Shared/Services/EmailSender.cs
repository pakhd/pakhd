using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Options;
using SendGrid;
using Newtonsoft.Json;

namespace Pakhd.Shared.Services
{
    public enum EmailType
    {
        Confirmation,
        PasswordReset,
        Reservations,
        Sell
    }
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } 

        public Task SendEmailAsync(string email, string subject, string message)
        {
            
            return Execute(Options.SendGridKey, subject, message, email);
        }

        public Task Execute(string? apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage();

            msg.SetFrom(new EmailAddress("no-reply@pakhd.app", "Pakhd"));
            msg.AddTo(new EmailAddress(email));
            msg.SetTemplateId("d-c677343e60be4ffda482b5a9663c04e5");
            TemplateData templateData = new TemplateData()
            {
                Subject = subject,
                Message = message,
            };

            msg.SetTemplateData(templateData);

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            var response =  client.SendEmailAsync(msg);

            return response;
        }

    }

    public class TemplateData
    {
        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("Sender_Name")]
        public string SenderName { get; set; }

        [JsonProperty("Sender_Address")]
        public string SenderAddress { get; set; }

        [JsonProperty("Sender_City")]
        public string SenderCity { get; set; }

        [JsonProperty("Sender_State")]
        public string SenderState { get; set; }

        [JsonProperty("Sender_Zip")]
        public string SenderZip { get; set; }
    }
}
