﻿using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace QuanLyKyTucXa.Common
{
    public class SendMail
    {
        public static async Task<bool> SendEmailAsync(string title, string noiDung, string toEmail)
        {
            try
            {
                var fromEmail = "clontomi@gmail.com";
                var fromPassword = "aaozjsngurqrieih";

                using (var smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential(fromEmail, fromPassword);
                    smtpClient.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(fromEmail),
                        Subject = title,
                        Body = noiDung,
                        IsBodyHtml = true,
                    };

                    mailMessage.To.Add(toEmail);

                    await smtpClient.SendMailAsync(mailMessage);
                }

                return true;
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP Error: {smtpEx.StatusCode} - {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }

            return false;
        }
    }
}