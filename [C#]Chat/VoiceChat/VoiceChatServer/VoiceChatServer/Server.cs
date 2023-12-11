using System.Net;
using System.Net.Sockets;
using System.Text;

namespace VoiceChatServer
{
    public class Server
    {
        public Socket ServerSocket { get; private set; }
        private List<Socket> _clients = new List<Socket>();
        private byte[] bytes = new byte[2048];

        public Server(Socket socketServer)
        {
            try
            {
                ServerSocket = socketServer;
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 25565);
                ServerSocket.Bind(ipPoint);
                ServerSocket.Listen(100);

                Console.WriteLine("Start server.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[StartServer] Error: {ex.Message}");
            }
        }

        public async Task WaitForClients()
        {
            while (true)
            {
                try
                {
                    await WaitJoin();
                    await Task.Delay(100);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Server WaitForClients] Error: {ex.Message}.");
                    await Task.Delay(1);
                }
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
                await Task.Delay(100);
            }
        }

        public async Task WaitJoin()
        {
            try
            {
                Socket client = await ServerSocket.AcceptAsync();
                _clients.Add(client);
                Console.WriteLine($"Client join: {client.RemoteEndPoint}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Server WaitJoin] Error: {ex}.");
            }
        }

        public async Task Receive()
        {
            try
            {
                List<Socket> disconnectedClients = new List<Socket>();
                foreach (var client in _clients)
                {
                    if (client.Poll(0, SelectMode.SelectRead))
                    {
                        int availableData = client.Available;
                        if (availableData == 0)
                        {
                            disconnectedClients.Add(client);
                            continue;
                        }

                        int bytesRead = await client.ReceiveAsync(bytes);
                        if (bytesRead == 0)
                        {
                            disconnectedClients.Add(client);
                            continue;
                        }

                        string? receiveBytes = Encoding.UTF8.GetString(bytes, 0, bytesRead);

                        if (receiveBytes.StartsWith("/audio="))
                        {
                            try
                            {
                                string base64Audio = receiveBytes.Substring("/audio=".Length);
                                string cleanedBase64Audio = base64Audio.Trim();
                                byte[] audioBytes = Convert.FromBase64String(cleanedBase64Audio);
                                Console.WriteLine($"Receive Audio: {audioBytes.Length}.");
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine($"Error while decoding Base64 audio: {ex.Message}");
                            }
                        }
                        else if (receiveBytes.StartsWith("/text="))
                        {
                            string textMessage = receiveBytes.Substring("/text=".Length);
                            Console.WriteLine($"Receive Text: {textMessage}.");
                        }
                        else
                        {
                            Console.WriteLine("Неизвестная команда!");
                        }
                        await SendAll(new Socket[] { client }, receiveBytes);
                    }
                }
                foreach (var client in disconnectedClients)
                {
                    Console.WriteLine($"Client {client.RemoteEndPoint} disconnected.");
                    _clients.Remove(client);
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Server Receive] Error: {ex}.");
            }
        }

        public async Task SendAll(Socket[] ignoreClients, string text)
        {
            try
            {
                foreach (var client in _clients)
                {
                    if (!ignoreClients.Contains(client))
                    {
                        await client.SendAsync(Encoding.UTF8.GetBytes(text));
                        Console.WriteLine($"{client.RemoteEndPoint}: {text}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Server SendAll] Error: {ex}.");
            }
        }
    }
}
