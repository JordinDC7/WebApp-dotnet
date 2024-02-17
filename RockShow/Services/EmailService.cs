using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RockShow.Domain.AppSettings;
using RockShow.Interfaces;
using RockShow.Requests.Email;
using RockShow.Requests.Users;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using Task = System.Threading.Tasks.Task;


namespace RockShow.Services
{
    public class EmailService : IEmailService
    {

        private IWebHostEnvironment _environment;
        private AppKeys _appKeys;
        private readonly IHttpClientFactory _httpClientFactory;
        public EmailService(IWebHostEnvironment environment, IOptions<AppKeys> appKeys, IHttpClientFactory httpClientFactory)
        {
            _environment = environment;
            _appKeys = appKeys.Value;
            _httpClientFactory = httpClientFactory;

        }

        public async Task ContactUs(ContactUsRequest model)
        {
            // Construct the plain text content for the email
            string textContent = $"Message from: {model.From}\nEmail: {model.Email}\n\n{model.Message}";

            // Email subject taken from the model
            string subject = model.Subject;

            // The recipient's email and name are taken from the application settings (AppKeys)
            string toEmail = _appKeys.SenderEmail;
            string toName = _appKeys.SenderName;

            // Use the updated SendEmailAsync method to send the email
            await SendEmailAsync(subject, textContent, toEmail, toName);
        }


        public async Task SendConfirm(UserAddRequest userModel, TokenAddRequest token)
        {
            // Assuming LoadHtmlTemplate generates the HTML content for the email
            string htmlContent = LoadHtmlTemplate("confirmation.html", userModel.FirstName, token);

            // Email subject
            string subject = "Confirm your account at OracleIllusions";

            // Recipient's email and name
            string userEmail = userModel.Email;
            string userName = userModel.FirstName + " " + userModel.LastName;

            // Use the updated SendEmailAsync method to send the email
            await SendEmailAsync(subject, htmlContent, userEmail, userName);
        }

        public async Task SendPassReset(UserBase userModel, TokenAddRequest token)
        {
            // Generate the HTML content for the email using the reset-password template
            string htmlContent = LoadHtmlTemplate("reset-password.html", token: token, userEmail: userModel.Email);

            // Email subject
            string subject = "Password Change Requested at OracleIllusions";

            // Recipient's email
            string userEmail = userModel.Email;

            // Recipient's name (if available, otherwise just use the email)
            string userName = "User"; // Assuming UserBase has FirstName and LastName properties

            // Use the updated SendEmailAsync method to send the email
            await SendEmailAsync(subject, htmlContent, userEmail, userName);
        }



        private string LoadHtmlTemplate(string templateFileName, string firstName = null, TokenAddRequest token = null, string userEmail = null)
        {
            try
            {
                string templatePath = Path.Combine(_environment.WebRootPath, "EmailTemplates", templateFileName);

                if (File.Exists(templatePath))
                {
                    if (token != null)
                    {
                        if (templateFileName == "confirmation.html" && token.TokenType == 1)
                        {
                            string customLink = $"{_appKeys.DomainUrl}/confirm?tokenId={token.TokenId}";
                            string customScript = File.ReadAllText(templatePath).Replace("Confirm-Link-Insert", customLink).Replace("Users-First-Name", firstName);

                            return customScript;
                        }
                        else if (templateFileName == "reset-password.html" && token.TokenType == 2)
                        {
                            string customLink = $"{_appKeys.DomainUrl}/changepassword?token={token.TokenId}&email={userEmail}";
                            string customScript = File.ReadAllText(templatePath).Replace("Confirm-Link-Insert", customLink);

                            return customScript;
                        }
                        else { throw new Exception("Token not recognized. Please try again."); };
                    }
                    else
                    { return File.ReadAllText(templatePath); }
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //private async Task SendEmailAsync(SendSmtpEmail email)
        //{
        //    Configuration.Default.ApiKey["api-key"] = _appKeys.SendGridAppKey;
        //    var apiInstance = new TransactionalEmailsApi();
        //    CreateSmtpEmail result = await apiInstance.SendTransacEmailAsync(email);
        //}

        public async Task SendEmailAsync(string subject, string textContent, string toEmail, string toName)
        {
            var apiKey = _appKeys.MandrillApiKey; ;
            var client = _httpClientFactory.CreateClient();

            var message = new
            {
                key = apiKey,
                message = new
                {
                    html = textContent,
                    subject = subject,
                    from_email = _appKeys.SenderEmail,
                    from_name = _appKeys.SenderName,
                    to = new[]
                    {
                new { email = toEmail, name = toName, type = "to" }
            }
                }
            };

            var json = JsonConvert.SerializeObject(message);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("https://mandrillapp.com/api/1.0/messages/send.json", content);

            string responseString = await response.Content.ReadAsStringAsync();
           
        }
    }
}
