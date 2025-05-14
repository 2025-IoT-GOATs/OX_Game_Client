using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Windows;

namespace OX_Game_Client.Models
{
    public class SocketConn
    {
        private Socket socket;
        static private SocketConn instance = new SocketConn();
        private byte[] s_buf;
        private byte[] r_buf;
        Queue<string> TaskQueue = new Queue<string>();
        // 쓰레드 하나가 백그라운드로 돌아감, while (true) 큐에 데이터가 있나?

        public event Action<string> MessageReceived;
        public byte[] s_Buf
        {
            get => s_buf;
        }
        public void setBuf(string name)
        {
            this.s_buf = Encoding.UTF8.GetBytes(name, 0, name.Length);
        }
        public byte[] r_Buf
        {
            get => r_buf;
        }
        static public SocketConn Instance
        {
            get
            {
                return instance;
            }
        }
        private SocketConn()
        {

        }
        public async Task connect(string ip, short port)
        {
            await Task.Run(async () =>
            {
                try
                {
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    EndPoint serverEP = new IPEndPoint(IPAddress.Parse(ip), port);
                    socket.Connect(serverEP);
                    await recv();

                }
                catch (Exception ex)
                {
                }
            });
            
        }
        public async Task send(string msg)
        {
            await Task.Run(() => {
                try
                {
                    setBuf(msg);
                    socket.Send(s_buf);
                    s_buf = null;

                }
                catch (Exception)
                {
                }
            });
        }
        public async Task recv()
        {
            await Task.Run(async () => {
                while (true)
                {
                    try
                    {
                        r_buf = new byte[1024];
                        int size = socket.Receive(r_buf);
                        string txt = Encoding.UTF8.GetString(r_buf, 0, size-1).Trim();
                        TaskQueue.Enqueue(txt);
                        if (TaskQueue.Count > 0)
                        {
                           var temp = TaskQueue.Dequeue().ToString();
                           Application.Current.Dispatcher.Invoke(() => {
                               MessageReceived?.Invoke(temp);
                               Console.WriteLine("Received : " + temp);
                               
                           });
                        }

                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            });
            
            
        }
    }
}
