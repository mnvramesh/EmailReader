using OpenPop.Mime;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Mailbird
{
    public class MailDetails : INotifyPropertyChanged
    {
        private Attachment _selectedAttachment;
        private ObservableCollection<Attachment> _attachments;

        public string Id { get; set; }
        public string FromFull
        {
            get
            {
                return string.Format("{0}<{1}>", From, FromAddress);
            }
            set { }
        }
        public string FromAddress { get; set; }
        public string From { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }

        public string Body { get; set; }


        public ObservableCollection<Attachment> Attachments
        {
            get
            {
                return _attachments;
            }
            set
            {
                _attachments = value;
                Notify("Attachments");
            }
        }

        public Attachment SelectedAttachment
        {
            get
            {
                return _selectedAttachment;
            }
            set
            {
                _selectedAttachment = value;
                OpenAttachment(value);
                Notify("SelectedAttachment");
            }
        }

        private void OpenAttachment(Attachment value)
        {
            File.WriteAllBytes(value.Name, value.Content);
            Process.Start(value.Name);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void Notify(string PropName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropName));
        }

        public MailDetails(Message message)
        {
            Id = message.Headers.MessageId;
            From = message.Headers.From.DisplayName;
            FromAddress = message.Headers.From.Address;
            var cc = message.Headers.Cc.Select(x => string.Format("{0}<{1}>", x.DisplayName, x.Address));
            foreach (var eachcc in cc)
            {
                CC += eachcc + ";";

            }

            Subject = message.Headers.Subject;

            var html = message.FindFirstHtmlVersion();
            var plainText = message.FindFirstPlainTextVersion();
            if (html != null)
            {
                Body = html.GetBodyAsText();
            }
            else if (plainText != null)
            {
                Body = plainText.GetBodyAsText();
            }
            Attachments = new ObservableCollection<Attachment>();
            var attachments = message.FindAllAttachments();
            if (attachments != null && attachments.Count > 0)
            {
                foreach (var attachment in attachments)
                {
                    Attachments.Add(new Attachment(attachment));
                }
            }
        }
    }
}
