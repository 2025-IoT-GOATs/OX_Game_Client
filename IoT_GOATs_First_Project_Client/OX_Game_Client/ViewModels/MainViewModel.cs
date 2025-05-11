using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net.Sockets;
using System.Net;
using System.Windows.Navigation;
using System.Windows.Input;
using OX_Game_Client.Views;

namespace OX_Game_Client.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public string Name { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainViewModel()
        {
            
        }
    }
}
