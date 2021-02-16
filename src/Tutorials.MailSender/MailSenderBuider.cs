using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Tutorials.MailSender.Model;
using Attachment = Tutorials.MailSender.Model.Attachment;

namespace Tutorials.MailSender
{
    public class MailSenderBuider : SendMail<MailSenderBuider>
    {
        public MailSenderBuider(MailCredential credental) : base(credental)
        {
            
        }
    }

    public class SenderEmailBuilder<T> : EmailSenderConfiguration where T : SenderEmailBuilder<T>
    {
        public T WithSenderEmail(string email)
        {
            SenderEmail = email;
            return (T) this;
        }
    }

    public class DisplayNameBuilder<T> : SenderEmailBuilder<DisplayNameBuilder<T>> where T : DisplayNameBuilder<T>
    {
        public T WithDisplayName(string name)
        {
            DisplayName = name;
            return (T) this;
        }
    }

    public class SubjectBuilder<T> : DisplayNameBuilder<SubjectBuilder<T>> where T : SubjectBuilder<T>
    {
        public T WithSubject(string subject)
        {
            Subject = subject;
            return (T) this;
        }
    }

    public class ReceiverBuilder<T> : SubjectBuilder<ReceiverBuilder<T>> where T : ReceiverBuilder<T>
    {
        public T WithReceivers(EmailNamePair[] emails)
        {
            Receivers = emails;
            return (T) this;
        }
    }

    public class BodyBuilder<T> : ReceiverBuilder<BodyBuilder<T>> where T : BodyBuilder<T>
    {
        public T WithBody(string body)
        {
            Body = body;
            return (T) this;
        }
    }

    public class SendMail<T> : BodyBuilder<SendMail<T>> where T : SendMail<T>
    {
        private readonly MailCredential credental;
        public SendMail(MailCredential credental)
        {
            this.credental = credental;
        }

        public bool Send()
        {
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(SenderEmail, DisplayName),
                    Subject = Subject,
                    Body = Body,
                    IsBodyHtml = true,
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                };
                foreach (var receiver in Receivers)
                {
                    mailMessage.To.Add(receiver.Email);
                }

                if (Attachments != null)
                {
                    if (Attachments.Any())
                    {
                        foreach (var attachment in Attachments)
                        {
                            mailMessage.Attachments.Add(new System.Net.Mail.Attachment(attachment.Path));
                        }
                    }
                }

                var smptpClient = new SmtpClient
                {
                    Credentials = new NetworkCredential(SenderEmail, credental.Password),
                    EnableSsl = true,
                    Port = credental.Port,
                    Host = credental.Host,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };
                smptpClient.Send(mailMessage);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    public static class SendMailExtension
    {
        public static SendMail<T> WidthAttachments<T>(this SendMail<T> mail, IEnumerable<Attachment> attachments)
            where T : SendMail<T>
        {
            mail.Attachments = attachments.ToArray();
            return mail;
        }
    }
}
