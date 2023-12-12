using System.Net;
using System.Net.Sockets;
using System.Text;

namespace VoiceChat.Network
{
    public class Client
    {
        public Socket? SocketClient;
        public bool IsConnected = false;
        private byte[] _bytes = new byte[1024];
        private bool _isFindJoin = true;

        public async Task SendText(string text)
        {
            try
            {
                if (SocketClient == null)
                    return;
                byte[] bytes = Encoding.UTF8.GetBytes($"/text={text}");
                Console.WriteLine($"[SendText] {text}");
                await SocketClient.SendAsync(bytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SendText Text] Error: {ex}");
            }
        }

        public async Task SendAudio(byte[] bytes)
        {
            try
            {
                if (SocketClient == null)
                    return;
                string? audioBase64 = Convert.ToBase64String(bytes);
                bytes = Encoding.UTF8.GetBytes($"/audio={audioBase64}");
                await SocketClient.SendAsync(bytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SendAudio] Error: {ex}");
            }
        }

        public async Task Join()
        {
            try
            {
                if (IsConnected == true)
                {
                    Console.WriteLine("No connect, connected");
                    return;
                }
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 25565);
                SocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                await SocketClient.ConnectAsync(ipPoint);
                IsConnected = true;

                Console.WriteLine($"Join: {Settings.IP}:{Settings.Port}");
            }
            catch (SocketException)
            {
                SocketClient = null;
                Console.WriteLine($"Do not set connect to server: {Settings.IP}:{Settings.Port}");
            }
        }

        public async Task ReceiveFromClients()
        {
            while (_isFindJoin)
            {
                try
                {
                    await Receive();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Server ReceiveFromClients] Error: {ex.Message}.");
                }
                await Task.Delay(10);
            }
        }

        public async Task Receive()
        {
            try
            {
                if (!IsConnected)
                {
                    //Console.WriteLine($"No connect!");
                    return;
                }    

                if (IsConnected 
                    && SocketClient.Poll(0, SelectMode.SelectRead))
                {
                    int bytesRead = await SocketClient.ReceiveAsync(_bytes);
                    Socket socket = SocketClient;
                    Console.WriteLine($"Receive [{socket.RemoteEndPoint}]: {Encoding.UTF8.GetString(_bytes, 0, bytesRead)}.");
                }
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case SocketException:
                        Console.WriteLine($"[Server Receive] Error: Close host connection.");
                        Disconnect();
                        break;
                    default:
                        Console.WriteLine($"[Server Receive] Error: {ex}.");
                        break;
                }
            }
        }

        public void Disconnect()
        {
            if (SocketClient != null)
            {
                SocketClient.Shutdown(SocketShutdown.Both);
                SocketClient.Close();
                SocketClient = null;
                IsConnected = false;
                Console.WriteLine($"Disconnect to: {Settings.IP}:{Settings.Port}");
            }
        }
    }
}
