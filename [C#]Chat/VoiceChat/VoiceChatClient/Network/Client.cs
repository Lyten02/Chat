using System.Net;
using System.Net.Sockets;
using System.Text;

namespace VoiceChat.Network
{
    public class Client
    {
        public Socket? SocketClient;
        public bool IsConnected = false;
        private byte[] bytes = new byte[1024];

        public async Task Send(string text)
        {
            try
            {
                if (SocketClient == null)
                    return;
                byte[] bytes = Encoding.UTF8.GetBytes($"/text={text}");
                Console.WriteLine($"[Send] {text}");
                await SocketClient.SendAsync(bytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Send Text] Error: {ex}");
            }
        }

        public async Task SendBytes(byte[] bytes)
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
                Console.WriteLine($"[SendBytes] Error: {ex}");
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
            while (true)
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
                    Console.WriteLine($"No connect!");
                    return;
                }    

                if (IsConnected 
                    && SocketClient.Poll(0, SelectMode.SelectRead))
                {
                    int bytesRead = await SocketClient.ReceiveAsync(bytes);
                    Socket socket = SocketClient;
                    Console.WriteLine($"Receive [{socket.RemoteEndPoint}]: {Encoding.UTF8.GetString(bytes, 0, bytesRead)}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Server Receive] Error: {ex}.");
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
