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
using System.Collections.ObjectModel;
using System.IO;

namespace OX_Game_Client.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
       
        private UserControl _currentView;
        private bool isStart = false;
        public UserControl CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }
        private readonly SocketConn _client = SocketConn.Instance;
        public ObservableCollection<string> LoginMessages { get; } = new ObservableCollection<string>();

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
            Console.WriteLine("메인뷰모델꺼");

            StringReader rs = new StringReader(msg);
            string readMsg = rs.ReadLine();
            string[] words = readMsg.Split(' ');
            //msg = msg.Substring(0, msg.Length);
            if (!isStart && words[0] == "LOGIN" && words[1] == Name)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Character participant = new Character(words[1], Convert.ToDouble(words[2]), Convert.ToDouble(words[3]));
                    CharacterManager.InitCharManager(participant);
                    var manager = CharacterManager.Instance;
                    LoginMessages.Add($"{words[1]} {words[2]} {words[3]}");
                    Console.WriteLine("채팅방 연결 완료");
                    var vm = new InGameViewModel(Name, LoginMessages);
                    var v = new InGameView
                    {
                        DataContext = vm
                    };
                    CurrentView = v;
                });
                isStart = true;
            }
            else if (words[0] == "LOGIN")
            {
                var newChar = new Character(words[1], Convert.ToDouble(words[2]), Convert.ToDouble(words[3]));
                CharacterManager.Instance?.AddParticipant(newChar);
            }
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
            string msg = "LOGIN " + Name + "\n";
            Console.WriteLine(msg);
            try
            {
                //await SocketConn.Instance.connect("210.119.12.82", 9000);
                await SocketConn.Instance.send(msg);
                //Character participant = new Character(Name, 100, 100);
                //CharacterManager.InitCharManager(participant);
                //var manager = CharacterManager.Instance;

                //SocketConn.Instance.recv();
            }
            catch (Exception)
            {
            }
            
        }

        
    }
}
