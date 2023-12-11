using Newtonsoft.Json;
using System.Text;
using VoiceChat.Command;
using VoiceChat.Network;
using VoiceChat.Voice;

namespace Lyten.VoiceChat
{
    public class Program
    {
        public static Client client = new();
        public static Commands Commands = new();
        public static RecordAndPlayback Record = new RecordAndPlayback();

        public static void Main(string[] args)
        {
            Program program = new Program();
            program.StartProgramAsync();
            Thread.Sleep(3000);
            program.StartProgram();
        }

        private void StartProgram()
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;
            Record.Start();
            WriteConsole();
        }

        private async void StartProgramAsync()
        {
            await client.Join();
            await client.ReceiveFromClients();
        }

        public void WriteConsole()
        {
            while (true)
            {
                Console.WriteLine($"Plz write.\r'/' - commands");
                string? text = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(text))
                {
                    if (text.StartsWith("/"))
                    {
                        string[] parts = text.Split('=');
                        if (parts.Length == 2)
                        {
                            string commandName = parts[0].Substring(1).ToLower();  // Удалите слеш

                            string jsonArgs = parts[1];

                            if (Commands.commands.ContainsKey(commandName))
                            {
                                try
                                {
                                    var commandArgs = JsonConvert.DeserializeObject<CommandArgs>(jsonArgs);
                                    Commands.commands.TryGetValue(commandName, out Action<CommandArgs>? action);
                                    Commands.standartCommand?.Invoke(commandName);
                                    action?.Invoke(commandArgs);
                                }
                                catch (JsonReaderException)
                                {
                                    Console.WriteLine("Ошибка: неправильный формат JSON.");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"This command is not correct: {commandName}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Invalid command format: {text}");
                        }
                    }
                }
            }
        }

        public void UpdateViewDisplay()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"Voice Chat Start");
            Console.WriteLine($"");
            Console.WriteLine($"-----------------");
            Console.WriteLine($"Users: 1/8");
            Console.WriteLine($"Active Users: 0/8");
            Console.WriteLine($"-----------------");
            Console.WriteLine($"");
            Console.WriteLine($"-----------------");
            Console.WriteLine($"Voice chat time: ");
            Console.WriteLine($"Current Status: ");
            Console.WriteLine($"-----------------");
            Console.SetCursorPosition(20, 0);
            Console.WriteLine($"\tUser List");
            Console.SetCursorPosition(20, 1);
            Console.WriteLine($"-----------------");
            Console.SetCursorPosition(20, 2);
            Console.WriteLine($"1");
            Console.SetCursorPosition(20, 3);
            Console.WriteLine($"2");
            Console.SetCursorPosition(20, 4);
            Console.WriteLine($"3");
            Console.SetCursorPosition(20, 5);
            Console.WriteLine($"4");
            Console.SetCursorPosition(20, 6);
            Console.WriteLine($"5");
            Console.SetCursorPosition(20, 7);
            Console.WriteLine($"6");
            Console.SetCursorPosition(20, 8);
            Console.WriteLine($"7");
            Console.SetCursorPosition(20, 9);
            Console.WriteLine($"8");
            Console.SetCursorPosition(20, 10);
            Console.WriteLine($"-----------------");
        }
    }
}
