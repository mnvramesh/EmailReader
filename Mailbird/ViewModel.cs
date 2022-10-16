using OpenPop.Pop3;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Mailbird
{
    public class ViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<MailDetails> _mailDetails;
        private MailDetails _selectedMailDetails;

        public int Id { get; set; }

        public string From { get; set; }
        public string CC { get; set; }
        public string Subject { get; set; }

        public string Body { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<MailDetails> MailDetails
        {
            get
            {
                return _mailDetails;
            }
            set
            {
                _mailDetails = value;
                Notify("MailDetails");
            }
        }

        public MailDetails SelectedMailDetails
        {
            get
            {
                return _selectedMailDetails;
            }
            set
            {
                _selectedMailDetails = value;
                Notify("SelectedMailDetails");
            }
        }

        public ViewModel()
        {
            MailDetails = new ObservableCollection<MailDetails>();

            Thread thread = new Thread(LoadEmails);
            thread.IsBackground = true;
            thread.Start();
        }


        private void LoadEmails()
        {
            var pop3Client = new Pop3Client();

            //pop3Client.Connect("outlook.office365.com", 995, true);
            pop3Client.Connect("pop.gmail.com", 995, true);

            // enable two step verification for Gmails and use app password
            pop3Client.Authenticate("your email id", "yourpassword");


            var abc = pop3Client.GetMessageCount();



            for (int i = abc; i > 0; i--)
            {

                var ab = pop3Client.GetMessage(i);
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    MailDetails.Add(new MailDetails(ab));
                }), DispatcherPriority.Background);
            }

        }

        private void Notify(string PropName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropName));
        }
    }


}
