using MahApps.Metro.Controls;
using OX_Game_Client.ViewModels;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace OX_Game_Client.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : MetroWindow
    {
        public MainView()
        {
            InitializeComponent();
        }
        private async void OnConfirmClick(object sender, RoutedEventArgs e)
        {
            string userName = UserNameTextBox.Text;
            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("이름을 입력하시오");
                return;
            }
            await Login(userName);
            MainGrid.Children.Clear();
            MainGrid.Children.Add(new Views.InGameWindow(userName));
        }
        public async Task Login(string name)
        {
            //if (string.IsNullOrEmpty(name))
            //{
            //    MessageBox.Show("이름을 입력하시오");
            //    return;
            //}
            Console.WriteLine($"UserName: {name}");
            //Thread clientThread = new Thread(clientLoginFunc);
            //clientThread.IsBackground = true;
            await Task.Run(() => clientLoginFunc(name));
            Console.WriteLine("죽음의 OX 게임 클라이언트 실행");
            //clientThread.Join();

            //Console.ReadLine();

        }
        private void clientLoginFunc(string name)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            EndPoint serverEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9000);

            socket.Connect(serverEP);
            try
            {
                byte[] buf = Encoding.UTF8.GetBytes(name);
                socket.Send(buf);

                byte[] recvBytes = new byte[1024];
                int nRecv = socket.Receive(recvBytes);
                string txt = Encoding.UTF8.GetString(recvBytes, 0, nRecv);

                Console.WriteLine("Login Success log from Server : " + txt);

                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("로그인 성공!");
                });

                socket.Close();
                Console.WriteLine("TCP Client Socket Closed");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);

                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("에러 발생");
                });
            }
            
        }
    }
}
