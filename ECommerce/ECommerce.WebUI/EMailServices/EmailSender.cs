﻿using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebUI.EMailServices
{
    public class EmailSender : IEmailSender
    {
     private const string SendGridApiKey= "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";
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //var apikey = Environment.GetEnvironmentVariable(SendGridApiKey);//sitede gördüm

            return Execute(SendGridApiKey, subject, htmlMessage, email);
        }

        private Task Execute(string sendGridApiKey, string subject, string htmlMessage, string email)
        {
            var client = new SendGridClient(sendGridApiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("info@fuatdeneme.com", "Üyelik kontrol sistemi"),
                Subject=subject,
                PlainTextContent=htmlMessage,
                HtmlContent=htmlMessage
            };

            msg.AddTo(new EmailAddress(email));

            return client.SendEmailAsync(msg);
        }
    }
}
