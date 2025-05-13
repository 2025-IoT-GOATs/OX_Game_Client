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
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.Input;
using OX_Game_Client.Models;
using MahApps.Metro.Controls;
using System.Windows.Controls;

namespace OX_Game_Client.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
       
        private UserControl _currentView;

        public UserControl CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }
        public MainViewModel()
        {
            //SocketConn.Instance.connect("210.119.12.82", 9000);
        }

        [ObservableProperty]
        private string name;

        [RelayCommand]
        public async void Login(string name)
        {
            name = "CHAT " + name + "\n";
            try
            {
                await SocketConn.Instance.connect("210.119.12.82", 9000);

                await SocketConn.Instance.send(name);
                
                await SocketConn.Instance.recv();
                
                //Thread t = new Thread(() =>
                //{
                //    //SocketConn.Instance.connect("210.119.12.82", 9000);
                //    while (true)
                //    {
                //        SocketConn.Instance.recv();
                //    }
                //});
                //t.IsBackground = true;
                //t.Start();
            }
            catch (Exception)
            {
            }
            var vm = new InGameViewModel();
            var v = new InGameView
            {
                DataContext = vm
            };
            CurrentView = v;
        }

        
    }
}
