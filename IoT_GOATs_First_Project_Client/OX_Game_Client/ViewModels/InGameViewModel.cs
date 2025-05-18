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
using System.Collections.ObjectModel;
using System.IO;
using System.DirectoryServices;

namespace OX_Game_Client.ViewModels
{
    public partial class InGameViewModel : ObservableObject
    {
        //[ObservableProperty]
        //private ObservableCollection<Character> participants;
        [ObservableProperty]
        private ObservableCollection<Character> currentParticipants;

        private readonly SocketConn _client = SocketConn.Instance;

        public ObservableCollection<string> LoginMessages { get; } = new ObservableCollection<string>();
        public InGameViewModel(string name, ObservableCollection<string> loginMessages)
        {
            Name = name;
            LoginMessages = loginMessages;
            _client.MessageReceived += OnChatReceived;

            CurrentParticipants = CharacterManager.Instance.Participants;

            //foreach (var p in currentParticipants)
            //{
            //    Console.WriteLine($"참가자 목록 : {p}");
            //}
        }
        private void OnChatReceived(string msg)
        {
            Console.WriteLine("인게임뷰모델꺼");

            StringReader rs = new StringReader(msg);
            while (rs.Peek() != -1)
            {
                string readMsg = rs.ReadLine();
                string[] words = readMsg.Split(' ');
                string userName = words[1];
                if (words[0] == "CHAT")
                {
                    msg = msg.Substring(5, msg.Length - 5);
                    if (words[0] + words[1] == "CHATOK")
                    {
                        OutputMessages.Add("채팅방 입장");
                    }
                    else
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            OutputMessages.Add(msg);
                        });
                    }
                }
                else if (words[0] == "LOGIN")
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var alreadyExists = CharacterManager.Instance.Participants.Any(c => c.UserName == userName);
                        if (!alreadyExists)
                        {
                            CharacterManager.Instance.AddParticipant(new Character(userName, 100, 100));
                            CurrentParticipants = CharacterManager.Instance.Participants;
                        }
                        foreach (var p in currentParticipants)
                        {
                            Console.WriteLine($"TEST sibal {p.UserName}");
                        }
                    });
                }
                else if (words[0] == "MOVE")
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        OutputMoveMsg.Add(msg);
                        Console.WriteLine($"Test : {msg}");
                        var character = CurrentParticipants.FirstOrDefault(c => c.UserName == userName);
                        if (character != null)
                        {
                            character.X = Convert.ToDouble(words[2]);
                            character.Y = Convert.ToDouble(words[3]);
                        }
                    });
                }
            }
            
        }

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string inputMessage;

        public ObservableCollection<string> OutputMessages { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> OutputMoveMsg { get; } = new ObservableCollection<string>();

        [RelayCommand]
        public async void SendChat()
        {
            //chatMsg = "CHAT" + Name + chatMsg + "\n";
            string chatMsg = $"CHAT {Name} {InputMessage}\n";
            //Console.WriteLine("sendchat" + chatMsg);
            //string txt = Encoding.UTF8.GetString(SocketConn.Instance.r_Buf, 0, SocketConn.Instance.r_Buf.Length - 1).Trim();
            try
            {
                await SocketConn.Instance.send(chatMsg);
                InputMessage = string.Empty;
                //await SocketConn.Instance.recv();
                //OutputMessages.Add(chatMsg);
                //OutputMessages.Add(txt);
            }
            catch (Exception)
            {

            }
        }

        public async Task Move(string direction)
        {
            //const double step = 10;
            string moveMsg = $"MOVE {direction}\n";
            await SocketConn.Instance.send(moveMsg);
            moveMsg = string.Empty;
            //switch (direction)
            //{
            //    case "Left":
            //        X -= step;
            //        break;
            //    case "Right":
            //        X += step;
            //        break;
            //    case "Up":
            //        Y -= step;
            //        break;
            //    case "Down":
            //        Y += step;
            //        break;
            //}
        }
    }
}
