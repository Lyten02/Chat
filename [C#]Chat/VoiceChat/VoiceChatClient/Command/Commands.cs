using Lyten.VoiceChat;
using VoiceChat.Network;

namespace VoiceChat.Command
{
    public class Commands
    {
        public readonly Action<string> standartCommand = new((string nameCommand) =>
        {
            Console.WriteLine($"CommandArgs use: \"{nameCommand}\"");
        });

        public readonly Dictionary<string, Action<CommandArgs>> commands;

        public Commands()
        {
            commands = new()
            {
//              "/close={}"
                { "close",
                    (CommandArgs) => {
                        Program.client.Disconnect();
                    }
                },
//              "/join={}"
                { "join",
                    (CommandArgs) => {
                        Program.client?.Join();
                    }
                },
//              "/send={"text":"текст"}"
                { "send",
                    (CommandArgs) => {
                        Program.client?.Send($"{CommandArgs.Text}");
                    }
                },
//              "/stop={}"
                { "stop",
                    (CommandArgs) => {
                        Program.Record.OutStopAudio();
                        Program.client?.Send($"Stop audio stream.");
                    }
                },
//              "/play={}"
                { "play",
                    (CommandArgs) => {
                        Program.Record.OutPlayAudio(CommandArgs.Volume);
                        Program.client?.Send($"Play audio stream. {CommandArgs.Volume}");
                    }
                },
//              "/sendaudio={}"
                { "sendaudio",
                    async (CommandArgs) => {
                        await Program.client.SendBytes(new byte[1024]);
                    }
                },
            };
        }
    }
}
