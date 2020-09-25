namespace Tutorials.MailSender.Model
{
    public abstract class EmailSenderConfiguration
    {
        public string SenderEmail { get; set; }
        public string DisplayName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public EmailNamePair[] Receivers { get; set; }
        public Attachment[] Attachments { get; set; }
    }
}