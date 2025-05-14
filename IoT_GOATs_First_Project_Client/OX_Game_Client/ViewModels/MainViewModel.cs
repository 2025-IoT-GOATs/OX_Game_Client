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
        private readonly SocketConn _client = SocketConn.Instance;

        public MainViewModel()
        {
            //SocketConn.Instance.recv();
            //SocketConn.Instance.connect("210.119.12.82", 9000);
            Thread t = new Thread(async () =>
            {
                await SocketConn.Instance.connect("210.119.12.82", 9000);
            });
            t.Start();
            _client.MessageReceived += OnMessageReceived;

        }

        private void OnMessageReceived(string msg)
        {
            msg = msg.Substring(0, msg.Length);
            if (msg == "CHAT OK")
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    Console.WriteLine("채팅방 연결 완료");
                });
            }
            //else if (msg == $"CHAT {Name} {InGameViewModel.}")
            //{

            //}
            else
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Console.WriteLine("연결 안 됨" + msg);
                });
            }
        }

        [ObservableProperty]
        private string name;

        [RelayCommand]
        public async void Login()
        {
            //Name = name;
            //Name = "CHAT " + name + "\n";
            string msg = "CHAT " + Name + "\n";
            try
            {
                //await SocketConn.Instance.connect("210.119.12.82", 9000);
                await SocketConn.Instance.send(msg);

                //SocketConn.Instance.recv();
            }
            catch (Exception)
            {
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                var vm = new InGameViewModel(Name);
                var v = new InGameView
                {
                    DataContext = vm
                };
                CurrentView = v;
            });
        }

        
    }
}
