using Lyten.Packages;
using VoiceChat.Enums;

namespace VoiceChat.Voice
{
    public class User
    {
        private Time _currentUserTime = new();
        public Time CurrentTime
        {
            private get => _currentUserTime;
            set
            {

            }
        }
        private StatusUserVoice _statusUserVoice = StatusUserVoice.Empty;
        public StatusUserVoice StatusUserVoice
        {
            get => _statusUserVoice;
            set
            {
                _statusUserVoice = value;
            }
        }
    }
}
