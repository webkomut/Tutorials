using System;
using System.IO;
using Tutorials.MailSender.Model;
using Xunit;

namespace Tutorials.MailSender.Test
{
    public class MailSenderBuilderTest
    {
        private readonly MailSenderBuider sender;

        public MailSenderBuilderTest()
        {
            var credental = new MailCredential
            {
                Host = "smtp.gmail.com",
                Password = "***",
                Port = 587
            };
            sender = new MailSenderBuider(credental);
        }
        [Fact]
        public void Mail_Seder()
        {
            var myDocFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var result = sender
                .WithSenderEmail("sender@mail.com")
                .WithDisplayName("KOMUT TV")
                .WithSubject("Subeject")
                .WithReceivers(new[]
                {
                    new EmailNamePair
                    {
                        Email = "mail@mail.com",
                        Name = "Name Surname"
                    }
                })
                .WithBody("Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.")
                .WidthAttachments(new []
                {
                    new Attachment
                    {
                        Path = Path.Combine(myDocFolder, "image1.jpeg")
                    },
                    new Attachment
                    {
                        Path = Path.Combine(myDocFolder, "image2.jpeg")
                    }
                })
                .Send();
            Assert.True(result);
        }
    }

    
}
