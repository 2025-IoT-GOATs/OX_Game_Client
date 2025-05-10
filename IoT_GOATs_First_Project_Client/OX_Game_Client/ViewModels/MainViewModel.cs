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
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object? parameter) => _execute();
    }
    public class MainViewModel : INotifyPropertyChanged
    {
        public string Name { get; set; }

        public ICommand TestSubmitCommand { get; }

        public event Action? RequestNavigateToEmptyPage;

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainViewModel()
        {
            TestSubmitCommand = new RelayCommand(TestSubmit);
        }

        private void TestSubmit()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("유저 이름을 입력해주세요.");
                return;
            }

            // View에 전환 요청
            RequestNavigateToEmptyPage?.Invoke();
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
