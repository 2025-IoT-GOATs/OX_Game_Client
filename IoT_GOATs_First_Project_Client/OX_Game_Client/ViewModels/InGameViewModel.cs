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

namespace OX_Game_Client.ViewModels
{
    public partial class InGameViewModel : ObservableObject
    {
        private readonly SocketConn _client = SocketConn.Instance;
        public InGameViewModel(string name)
        {
            Name = name;
            _client.MessageReceived += OnChatReceived;
        }
        private void OnChatReceived(string msg)
        {
            StringReader rs = new StringReader(msg);
            string readMsg = rs.ReadLine();
            string[] words = readMsg.Split(' ');
            if (words[0] ==  "CHAT")
            {
                msg = msg.Substring(5, msg.Length - 5);
                if (words[0] + words[1] == "CHATOK")
                {
                    OutputMessages.Add("채팅방 입갤");
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        OutputMessages.Add(msg);
                    });
                }
            }
            else if (words[0] == "MOVE")
            {
                // TO DO MOVE
            }
            
        }

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string inputMessage;

        public ObservableCollection<string> OutputMessages { get; } = new ObservableCollection<string>();

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
        [RelayCommand]
        public async void SendKeyPress(Key key)
        {
            Console.WriteLine("키보드 입력");
            string keyPressMsg = $"MOVE {key.ToString()}\n";
            try
            {
                await _client.send(keyPressMsg);
            }
            catch (Exception)
            {
            }
        }

    }
}
