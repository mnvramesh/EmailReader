using OpenPop.Mime;
using System.ComponentModel;

namespace Mailbird
{
    public class Attachment : INotifyPropertyChanged
    {

        public Attachment(MessagePart attachment)
        {
            Name = attachment.FileName;
            Content = attachment.Body;
        }

        public string Name { get; set; }
        public byte[] Content { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Notify(string PropName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropName));
        }
    }

}
