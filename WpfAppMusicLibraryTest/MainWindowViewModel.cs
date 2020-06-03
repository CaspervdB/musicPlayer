using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfAppMusicLibraryTest
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string link;

        public string Link
        {
            get { return link; }
            set
            {
                if (!string.Equals(this.link, value) && value.Contains("youtube.com/watch?v="))
                {
                    this.link = value;
                    Console.WriteLine(link);
                }
                NotifyPropertyChanged("LinkTextBox");
            }
        }
        public ICommand ButtonCommand { get; set; }
        private VideoLibraryTester vlt;

        public MainWindowViewModel()
        {
            this.vlt = new VideoLibraryTester();
            ButtonCommand = new RelayCommand(async () => await vlt.SaveAudioToDisk2Async());
        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
