using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit.Text;
using MimeKit;
using MomAndBaby.Services.Helpers;
using MailKit;
using MomAndBaby.Services.Interface;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.WebUtilities;

namespace MomAndBaby.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendMailRegister(string email)
        {
            var emailToSend = new MimeMessage();
            emailToSend.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            emailToSend.To.Add(MailboxAddress.Parse(email));
            emailToSend.Subject = _mailSettings.DisplayName;

            var queryParams = new Dictionary<string, string?>
            {
                { "email", email }
            };

            var url = QueryHelpers.AddQueryString($"{_mailSettings.Url}api/authen/confirm-email/", queryParams);
            string htmlBody = GetHtmlContentConfirmEmail(url);

            emailToSend.Body = new TextPart(TextFormat.Html)
            {
                Text = htmlBody
            };

            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                emailClient.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                emailClient.Send(emailToSend);
                emailClient.Disconnect(true);
            }
        }

        public async Task SendMailForgotPassword(string email, string token)
        {
            var emailToSend = new MimeMessage();
            emailToSend.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            emailToSend.To.Add(MailboxAddress.Parse(email));
            emailToSend.Subject = _mailSettings.DisplayName;

            var queryParams = new Dictionary<string, string?>
            {
                { "email", email },
                { "token", token },
                { "newPassword", "Nguvl123@" }
            };

            var url = QueryHelpers.AddQueryString($"{_mailSettings.Url}api/authen/reset-password", queryParams);
            string htmlBody = GetHtmlContentForgotPassword(url);

            emailToSend.Body = new TextPart(TextFormat.Html)
            {
                Text = htmlBody
            };

            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                emailClient.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                emailClient.Send(emailToSend);
                emailClient.Disconnect(true);
            }
        }


        #region HTML Content
        public static string GetHtmlContentConfirmEmail(string url)
        {
            return $@"
        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Email Confirmation</title>
            <style>
                body {{
                    background-color: #f3f4f6;
                    font-family: Arial, sans-serif;
                    margin: 0;
                    padding: 0;
                    -webkit-font-smoothing: antialiased;
                    -moz-osx-font-smoothing: grayscale;
                }}

                .wrap {{
                    display: flex;
                    width: 700px;
                    margin: 0 auto;
                    padding: 20px;
                    background-color: #ffffff;
                    border-radius: 10px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                }}

                .email-container {{
                    background-color: #ffffff;
                    max-width: 500px;
                    padding: 25px;
                    text-align: center;
                    border-radius: 12px;
                    box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
                    border: 1px solid #e0e0e0;
                }}

                .logo {{
                    width: 100px;
                    margin-bottom: 20px;
                }}

                .title {{
                    font-size: 26px;
                    color: #ff5722;
                    font-weight: bold;
                    margin-bottom: 10px;
                }}

                .message {{
                    font-size: 18px;
                    color: #333;
                    margin-bottom: 25px;
                }}

                .confirm-button {{
                    display: inline-block;
                    background: linear-gradient(135deg, #4CAF50, #388E3C);
                    color: white;
                    padding: 14px 28px;
                    text-decoration: none;
                    border-radius: 8px;
                    font-size: 18px;
                    font-weight: bold;
                    transition: 0.3s ease;
                }}

                .confirm-button:hover {{
                    background: linear-gradient(135deg, #388E3C, #2E7D32);
                    transform: scale(1.05);
                }}

                .footer {{
                    margin-top: 20px;
                    font-size: 14px;
                    color: #777;
                }}

                .footer p {{
                    margin: 5px 0;
                }}
            </style>
        </head>
        <body>
            <div class=""wrap"">
                <div class=""email-container"">
                    <img class=""logo"" src=""https://cdn-icons-png.flaticon.com/512/561/561127.png"" alt=""Email Confirmation"">
                    <div class=""title"">Email Confirmation 🎉</div>
                    <div class=""message"">Please confirm your email address to activate your account.</div>
                    <a href=""{url}"" class=""confirm-button"">Confirm Email</a>
                    <div class=""footer"">
                        <p>🏫 FPT University</p>
                        <p>📍 Lô E2a-7, Đường D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Hồ Chí Minh</p>
                    </div>
                </div>
            </div>
        </body>
        </html>";
        }

        public static string GetHtmlContentForgotPassword(string url)
        {
            return $@"
        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Email Confirmation</title>
            <style>
                body {{
                    background-color: #f3f4f6;
                    font-family: Arial, sans-serif;
                    margin: 0;
                    padding: 0;
                    -webkit-font-smoothing: antialiased;
                    -moz-osx-font-smoothing: grayscale;
                }}

                .wrap {{
                    display: flex;
                    width: 700px;
                    margin: 0 auto;
                    padding: 20px;
                    background-color: #ffffff;
                    border-radius: 10px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                }}

                .email-container {{
                    background-color: #ffffff;
                    max-width: 500px;
                    padding: 25px;
                    text-align: center;
                    border-radius: 12px;
                    box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
                    border: 1px solid #e0e0e0;
                }}

                .logo {{
                    width: 100px;
                    margin-bottom: 20px;
                }}

                .title {{
                    font-size: 26px;
                    color: #ff5722;
                    font-weight: bold;
                    margin-bottom: 10px;
                }}

                .message {{
                    font-size: 18px;
                    color: #333;
                    margin-bottom: 25px;
                }}

                .confirm-button {{
                    display: inline-block;
                    background: linear-gradient(135deg, #4CAF50, #388E3C);
                    color: white;
                    padding: 14px 28px;
                    text-decoration: none;
                    border-radius: 8px;
                    font-size: 18px;
                    font-weight: bold;
                    transition: 0.3s ease;
                }}

                .confirm-button:hover {{
                    background: linear-gradient(135deg, #388E3C, #2E7D32);
                    transform: scale(1.05);
                }}

                .footer {{
                    margin-top: 20px;
                    font-size: 14px;
                    color: #777;
                }}

                .footer p {{
                    margin: 5px 0;
                }}
            </style>
        </head>
        <body>
            <div class=""wrap"">
                <div class=""email-container"">
                    <img class=""logo"" src=""https://cdn-icons-png.flaticon.com/512/561/561127.png"" alt=""Forgot Password"">
                    <div class=""title"">Forgot Password :(</div>
                    <div class=""message"">Please click the button below to reset your password</div>
                    <a href=""{url}"" class=""confirm-button"">Reset Password</a>
                    <div class=""footer"">
                        <p>🏫 FPT University</p>
                        <p>📍 Lô E2a-7, Đường D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Hồ Chí Minh</p>
                    </div>
                </div>
            </div>
        </body>
        </html>";
        }
        #endregion
    }
}