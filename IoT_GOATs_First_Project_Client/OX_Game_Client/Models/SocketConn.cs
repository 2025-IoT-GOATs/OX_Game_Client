using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
        public byte[] s_Buf
        {
            get => s_buf;
        }
        public void setBuf(string name)
        {
            this.s_buf = Encoding.UTF8.GetBytes(name);
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
            await Task.Run(() =>
            {
                try
                {
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    EndPoint serverEP = new IPEndPoint(IPAddress.Parse(ip), port);
                    socket.Connect(serverEP);


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
            await Task.Run(() => {
                while (true)
                {
                    try
                    {
                        r_buf = new byte[1024];
                        int size = socket.Receive(r_buf);
                        string txt = Encoding.UTF8.GetString(r_buf, 0, size);
                        TaskQueue.Enqueue(txt);
                        //TaskQueue.Dequeue().ToString();
                        Console.WriteLine("Login Success log from Server : " + txt);
                        //return TaskQueue.Dequeue();
                        // r_buf [0 ~ size] 짤라내고 짤린바로다음을 0번으로 땡겨와야됨 아마도 씨샾 기본제공 메서드 있을거임

                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            });
            
            
        }
        //public void Login(string name)
        //{
        //    name = "CHAT " + name + "\n";
        //    try
        //    {
        //        //EndPoint serverEP = new IPEndPoint(IPAddress.Parse("210.119.12.82"), 9000);
        //        //socket.Connect(serverEP);
        //        //byte[] buf = Encoding.UTF8.GetBytes(name);
        //        //socket.Send(buf);

        //        //byte[] recvBytes = new byte[1024];
        //        //int nRecv = socket.Receive(recvBytes);
        //        //string txt = Encoding.UTF8.GetString(recvBytes, 0, nRecv);

        //        //Console.WriteLine("Login Success log from Server : " + txt);

        //        //socket.Close();
        //        //Console.WriteLine("TCP Client Socket Closed");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error: " + ex.Message);

        //    }
        //}
    }
}
