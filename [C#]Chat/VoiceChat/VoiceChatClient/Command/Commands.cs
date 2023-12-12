using Lyten.VoiceChat;

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
                        Program.client?.SendText($"{CommandArgs.Text}");
                    }
                },
//              "/stop={}"
                { "stop",
                    (CommandArgs) => {
                        Program.Record.OutStopAudio();
                        Program.client?.SendText($"StopRecording audio stream.");
                    }
                },
//              "/stoprec={}"
                { "stoprec",
                    (CommandArgs) => {
                        Program.Record.StopRecording();
                        Program.client?.SendText($"StopRecording audio recording.");
                    }
                },
//              "/playrec={}"
                { "playrec",
                    (CommandArgs) => {
                        Program.Record.StartRecording();
                        Program.client?.SendText($"StartRecording audio recording.");
                    }
                },
//              "/play={}"
                { "play",
                    (CommandArgs) => {
                        Program.Record.OutPlayAudio(CommandArgs.Volume);
                        Program.client?.SendText($"Play audio stream. {CommandArgs.Volume}");
                    }
                },
//              "/sendaudio={}"
                { "sendaudio",
                    async (CommandArgs) => {
                        await Program.client.SendAudio(new byte[1024]);
                    }
                },
            };
        }
    }
}
