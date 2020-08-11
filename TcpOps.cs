using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPTester
{
    public static class TcpOps
    {
        private static Socket _sck;

        public static event Action<string> StatusMessage;

        public static string TcpHost { get; set; } = "127.0.0.1";
        public static int TcpPort { get; set; } = 2024;

        /// <summary>
        /// Connect to camera AI software via TCP socket
        /// </summary>
        /// <returns>true if successful</returns>
        public static bool Connect()
        {
            try
            {
                _sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                {
                    ReceiveTimeout = 5000
                };
                var endPoint = new IPEndPoint(IPAddress.Parse(TcpHost), TcpPort);
                _sck.Connect(endPoint);
            }
            catch (Exception e)
            {
                StatusMessage?.Invoke(e.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Read back the data from the AI software
        /// </summary>
        /// <param name="msg">a message request sent to the AI software</param>
        /// <returns>message returned from the AI software</returns>
        public static string Message(string msg)
        {
            string result = "";
            if (_sck != null && _sck.Connected)
                try
                {
                    var msgBuffer = Encoding.Default.GetBytes(msg);
                    _sck.Send(msgBuffer, 0, msgBuffer.Length, 0);

                    var buffer = new byte[255];
                    var rec = _sck.Receive(buffer, 0, buffer.Length, 0);
                    Array.Resize(ref buffer, rec);

                    _sck.Close();
                    result = Encoding.Default.GetString(buffer);
                }
                catch (Exception e)
                {
                    StatusMessage?.Invoke(e.Message);
                }

            return result;
        }
    }
}
