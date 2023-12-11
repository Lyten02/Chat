using System.Net.Sockets;

namespace VoiceChatServer
{
    public class Program
    {
        public Server Server { get; private set; } = new(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp));

        public static void Main(string[] args)
        {
            Program program = new();
            program.StartProgram();
            program.StartProgramAsync().Wait();

            Console.WriteLine("Press any key to stop the server...");
            Console.ReadLine();
        }

        public void StartProgram()
        {
            
        }

        public async Task StartProgramAsync()
        {
            await Task.WhenAll(Server.WaitForClients(), Server.ReceiveFromClients());
        }
    }
}