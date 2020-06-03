using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfAppMusicLibraryTest
{
    class MainWindowViewModel
    {
        public ICommand ButtonCommand { get; set; }
        private VideoLibraryTester vlt;

        public MainWindowViewModel()
        {
            this.vlt = new VideoLibraryTester();
            ButtonCommand = new RelayCommand(async () => await vlt.SaveAudioToDisk2Async());
        }
    }
}
