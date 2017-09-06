//-----------------------------------------------------------------------
// <copyright file="MailSender.cs" company="BestDay">
//     Copyright (c) Sprocket Enterprises. All rights reserved.
// </copyright>
// <author>Jose Martín Trejo Pancardo</author>
//----------------------------------------------------------------------

namespace Anxilaris.Utils
{
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using System.Threading.Tasks;

    public class MailSender
    {
        /// <summary>
        /// send a message asynchronously
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Task SendAsync(MessageSend message)
        {
            SmtpClient smtpServer = new SmtpClient();

            MailMessage mailMessage = PrepareMessage(message);

            if (smtpServer != null)
            {
                return smtpServer.SendMailAsync(mailMessage);
            }
            else
            {
                return Task.FromResult(0);
            }
        }

        /// <summary>
        /// send a message asynchronously
        /// </summary>
        /// <param name="message"></param>
        /// <param name="credential"></param>
        /// <returns></returns>
        public static Task SendAsync(MessageSend message, MessageCredential credential, MessageServer server)
        {
            SmtpClient smtpServer = new SmtpClient(server.Host);
            smtpServer.Port = server.Port;
            smtpServer.EnableSsl = server.EnableSsl;

            smtpServer.UseDefaultCredentials = true;
            smtpServer.Credentials = new NetworkCredential(credential.UserName, credential.Password);

            MailMessage mailMessage = PrepareMessage(message);

            mailMessage.From = new MailAddress(credential.UserName, credential.DisplayName);

            if (smtpServer != null)
            {
                return smtpServer.SendMailAsync(mailMessage);
            }
            else
            {
                return Task.FromResult(0);
            }
        }

        /// <summary>
        /// send a message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static void Send(MessageSend message)
        {
            SmtpClient smtpServer = new SmtpClient();

            MailMessage mailMessage = PrepareMessage(message);            

            if (smtpServer != null)
            {
                smtpServer.Send(mailMessage);
            }
        }

        /// <summary>
        /// send a message async
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static void Send(MessageSend message, MessageCredential credential, MessageServer server)
        {
            SmtpClient smtpServer = new SmtpClient(server.Host);
            smtpServer.Port = server.Port;
            smtpServer.EnableSsl = server.EnableSsl;

            smtpServer.UseDefaultCredentials = true;
            smtpServer.Credentials = new NetworkCredential(credential.UserName, credential.Password);

            MailMessage mailMessage = PrepareMessage(message);

            mailMessage.From = new MailAddress(credential.UserName, credential.DisplayName);

            if (smtpServer != null)
            {
                smtpServer.Send(mailMessage);
            }
        }

        /// <summary>
        /// send a message with send grid api key
        /// </summary>
        /// <param name="message"></param>
        public static void SendGridApi(string sendGridApiKey, MessageCredential credential, MessageSend message)
        {
            dynamic sg = new SendGridAPIClient(sendGridApiKey);
            
            var mail = PrepareSendGridMessage(credential, message);

            dynamic response = sg.client.mail.send.post(requestBody: mail.Get());

            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                throw new Exception(string.Format("Error {0}, please check {1}", response.StatusCode, "https://sendgrid.com/docs/API_Reference/Web_API_v3/Mail/errors.html"));
            }
        }

        /// <summary>
        /// send a message with send grid api key async
        /// </summary>
        /// <param name="message"></param>
        public static async Task SendGridApiAsync(string sendGridApiKey, MessageCredential credential, MessageSend message)
        {
            dynamic sg = new SendGridAPIClient(sendGridApiKey);

            var mail = PrepareSendGridMessage(credential, message);

            dynamic response = await sg.client.mail.send.post(requestBody: mail.Get());

            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                throw new Exception(string.Format("Error {0}, please check {1}", response.StatusCode, "https://sendgrid.com/docs/API_Reference/Web_API_v3/Mail/errors.html"));
            }
        }

        /// <summary>
        /// Get a MailMessage Object to sender
        /// </summary>
        /// <param name="message">message</param>
        /// <returns>MailMessage</returns>
        private static MailMessage PrepareMessage(MessageSend message)
        {
            MailMessage mail = new MailMessage();

            mail.Subject = message.Subject;
            mail.Body = message.Body;
            mail.IsBodyHtml = true;

            foreach (var mailto in message.MailTo)
            {
                mail.To.Add(new MailAddress(mailto));
            }

            foreach (var ccp in message.CCP)
            {
                mail.CC.Add(new MailAddress(ccp));
            }

            foreach (var cco in message.CCO)
            {
                mail.CC.Add(new MailAddress(cco));
            }

            foreach (var attachment in message.Attachment)
            {
                mail.Attachments.Add(new System.Net.Mail.Attachment(attachment));
            }
            
            if (message.SubjectEncoding != null)
            {
                mail.SubjectEncoding = message.SubjectEncoding;
            }

            if (message.BodyEnconding != null)
            {
                mail.BodyEncoding = message.BodyEnconding;
            }
            
            return mail;
        }

        /// <summary>
        /// Get a mail object to sendgrid
        /// </summary>
        private static Mail PrepareSendGridMessage(MessageCredential credential, MessageSend message)
        {
            Personalization personalizacion = new Personalization();

            foreach (var mailto in message.MailTo)
            {
                personalizacion.AddTo(new Email(mailto));
            }
            
            foreach (var ccp in message.CCP)
            {
                personalizacion.AddCc(new Email(ccp));
            }

            foreach (var cco in message.CCO)
            {
                personalizacion.AddBcc(new Email(cco));
            }
            
            Mail mail = new Mail();
            mail.From = new Email(credential.UserName, credential.DisplayName);

            mail.Subject = message.Subject;
            mail.CustomArgs = message.Args;

            foreach (var attachment in message.Attachment)
            {
                Byte[] bytes = File.ReadAllBytes(attachment);

                mail.AddAttachment(new SendGrid.Helpers.Mail.Attachment() { Filename = Path.GetFileName(attachment), Content = Convert.ToBase64String(bytes) });

                bytes = null;
            }
            
            mail.AddContent(new Content("text/html", message.Body));
            mail.AddPersonalization(personalizacion);

            return mail;
        }
    }

    public class MessageSend
    {
        private List<string> mailto;
        private List<string> ccp;
        private List<string> cco;
        private List<string> attachment;
        private Dictionary<string, string> args;

        /// <summary>
        /// MessageSend
        /// </summary>
        public MessageSend() { }

        /// <summary>
        /// MessageSend
        /// </summary>
        /// <param name="emailTo">Email to send email</param>
        /// <param name="subject">Email subject</param>
        /// <param name="body">Email body</param>
        public MessageSend(string emailTo, string subject, string body)
        {
            MailTo.Add(emailTo);
            Subject = subject;
            Body = body;
        }

        /// <summary>
        /// Gets or set a custom args
        /// </summary>
        public Dictionary<string, string> Args
        {
            get
            {
                args = args ?? new Dictionary<string, string>();
                return args;
            }
        }

        /// <summary>
        /// Gets or set a destination Mail sender
        /// </summary>
        public List<string> MailTo
        {
            get
            {
                mailto = mailto ?? new List<string>();
                return mailto;
            }
            set
            {
                mailto = value;
            }
        }

        /// <summary>
        /// Get or set a encodig of subject
        /// </summary>
        public Encoding SubjectEncoding { get; set; }
        
        /// <summary>
        /// Gets or set a subject of massage
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or set a body of message
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Get or set a encodig of body
        /// </summary>
        public Encoding BodyEnconding { get; set; }

        /// <summary>
        /// Get or set a list o ccp
        /// </summary>
        public List<string> CCP
        {
            get
            {
                ccp = ccp ?? new List<string>();
                return ccp;
            }
            set
            {
                ccp = value;
            }
        }

        /// <summary>
        /// Get or set a list of cco
        /// </summary>
        public List<string> CCO
        {
            get
            {
                cco = cco ?? new List<string>();
                return cco;
            }
            set
            {
                cco = value;
            }
        }

        /// <summary>
        /// Get or set if is dedug mode
        /// </summary>
        public List<string> Attachment
        {
            get
            {
                attachment = attachment ?? new List<string>();
                return attachment;
            }
            set
            {
                attachment = value;
            }
        }
    }

    public class MessageCredential
    {
        /// <summary>
        /// MessageCredential
        /// </summary>
        public MessageCredential() { }

        /// <summary>
        /// MessageCredential
        /// </summary>
        /// <param name="userName">User name to send </param>
        public MessageCredential(string userName)
        {
            UserName = userName;
        }

        /// <summary>
        /// MessageCredential
        /// </summary>
        /// <param name="userName">User name to send </param>
        /// <param name="password">Password of user name</param>
        public MessageCredential(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        /// <summary>
        /// MessageCredential
        /// </summary>
        /// <param name="userName">User name to send </param>
        /// <param name="displayName">Display name to send</param>
        /// <param name="password">Password of user name</param>
        public MessageCredential(string userName, string displayName, string password)
        {
            UserName = userName;
            Password = password;
            DisplayName = displayName;
        }

        /// <summary>
        /// Gets or sets a username
        /// </summary>
        public string UserName { get; set; }
        
        /// <summary>
        /// Gets or sets a display name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a password
        /// </summary>
        public string Password { get; set; }
    }

    public class MessageServer
    {
        /// <summary>
        /// MessageServer
        /// </summary>
        public MessageServer() { }

        /// <summary>
        /// MessageServer
        /// </summary>
        /// <param name="host">Url of email server</param>
        public MessageServer(string host)
        {
            Host = host;
        }

        /// <summary>
        /// MessageServer
        /// </summary>
        /// <param name="host">Url of email server</param>
        /// <param name="port">port of email server</param>
        public MessageServer(string host, int port)
        {
            Host = host;
            Port = port;
        }

        /// <summary>
        /// MessageServer
        /// </summary>
        /// <param name="host">Url of email server</param>
        /// <param name="port">port of email server</param>
        /// <param name="enableSsl">mode of email server</param>
        public MessageServer(string host, int port, bool enableSsl)
        {
            Host = host;
            Port = port;
            EnableSsl = enableSsl;
        }

        /// <summary>
        /// Gets or sets a host of mail server
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or set a Port of server
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Indicates if server is ssl
        /// </summary>
        public bool EnableSsl { get; set; }
    }
}
