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

namespace OX_Game_Client.ViewModels
{
    public partial class InGameViewModel : ObservableObject
    {
        public InGameViewModel()
        {

        }

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string inputMessage;


        [RelayCommand]
        public async void SendChat(string chatMsg)
        {
            chatMsg = ($"CHAT {chatMsg} \n").Trim();
            try
            {
                await SocketConn.Instance.send(chatMsg);
                await SocketConn.Instance.recv();
            }
            catch (Exception)
            {

                throw;
            }
        }

        //private async void OnConfirmClick(object sender, RoutedEventArgs e)
        //{
        //    string userName = UserNameTextBox.Text;
        //    if (string.IsNullOrEmpty(userName))
        //    {
        //        MessageBox.Show("이름을 입력하시오");
        //        return;
        //    }
        //    await Task.Run(() => SocketConn.Instance.Login(userName));
        //    Dispatcher.BeginInvoke(() => {
        //        MainGrid.Children.Clear();
        //        MainGrid.Children.Add(new Views.InGameWindow(userName));
        //    });
        //    //SocketConn.Instance.Login(userName);
        //    //await Login(userName);
        //    //Thread t1 = new Thread(() => {  
        //    //SocketConn.Instance.Login(userName);
        //    ////await Login(userName);
        //    //Dispatcher.Invoke(() => {
        //    //    MainGrid.Children.Clear();
        //    //    MainGrid.Children.Add(new Views.InGameWindow(userName));
        //    //});
        //    //await Task.Run(() => SocketConn.Instance.Login(userName));

        //    //t1.Start();

        //}


    }
}
