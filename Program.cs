using System;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Configuration;

namespace InternetOK
{
    class Program
    {
        static void Main(string[] args)
        {
            while (!IsInternetON())
            {
                Console.WriteLine(DateTime.Now.ToString() + " - Internet offline!");
            }
            SendMessage();
        }

        private static void SendMessage()
        {
            try
            {
                var smtpClient = CreateSmtpClient();
                smtpClient.Send(CreateMessage());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static MailMessage CreateMessage()
        {
            var message = new MailMessage()
            {
                From = new MailAddress("castelloesjf@gmail.com"),
                Subject = "Internet Status",
                Body = "<h1>Internet is Ok!</h1>",
                IsBodyHtml = true,
            };
            message.To.Add("castelloesjf@hotmail.com");

            return message;
        }

        private static SmtpClient CreateSmtpClient()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            var config = builder.Build();

            var smtpClient = new SmtpClient(config["Smtp:Host"])
            {
                UseDefaultCredentials = false,
                Port = int.Parse(config["Smtp:Port"]),
                Credentials = new NetworkCredential(config["Smtp:Username"], config["Smtp:Password"]),
                EnableSsl = true,
            };

            return smtpClient;
        }

        static bool IsInternetON()
        {
            string host = "www.google.com";
            bool result = false;
            Ping p = new Ping();
            try
            {
                PingReply reply = p.Send(host, 3000);
                if (reply.Status == IPStatus.Success)
                    return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now + " - " + ex.Message);
            }
            return result;
        }
    }
}
