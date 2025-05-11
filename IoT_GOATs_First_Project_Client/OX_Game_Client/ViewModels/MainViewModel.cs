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

        public void Login(string name)
        {
            if (this.Name is null)
            {
                MessageBox.Show("이름을 입력하시오");
                return;
            }
            Console.WriteLine($"UserName: {name}");
            Thread clientThread = new Thread(clientLoginFunc);
            clientThread.IsBackground = true;
            clientThread.Start();
            Console.WriteLine("죽음의 OX 게임 클라이언트 실행");
            clientThread.Join();

            //Console.ReadLine();

        }
        private void clientLoginFunc(object obj)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            EndPoint serverEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9000);

            socket.Connect(serverEP);
            while (true)
            {
                byte[] buf = Encoding.UTF8.GetBytes(this.Name);
                socket.Send(buf);

                byte[] recvBytes = new byte[1024];
                int nRecv = socket.Receive(recvBytes);
                string txt = Encoding.UTF8.GetString(recvBytes, 0, nRecv);

                Console.WriteLine("Login Success log from Server : " + txt);
            }
            socket.Close();
            Console.WriteLine("TCP Client Socket Closed");
        }
    }
}
