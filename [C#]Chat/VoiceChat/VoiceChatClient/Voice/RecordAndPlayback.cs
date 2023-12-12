using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace VoiceChat.Voice
{
    public class RecordAndPlayback
    {
        private WaveInEvent waveIn;
        private WaveOutEvent waveOut;
        private BufferedWaveProvider bufferedWaveProvider;
        private VolumeSampleProvider volumeProvider;

        public byte[] LatestAudioData { get; private set; }

        public float Volume
        {
            get => volumeProvider.Volume;
            set => volumeProvider.Volume = value;
        }

        public RecordAndPlayback()
        {
            waveIn = new WaveInEvent();
            bufferedWaveProvider = new BufferedWaveProvider(waveIn.WaveFormat)
            {
                DiscardOnBufferOverflow = true // Отбрасывать данные, если буфер полон
            };
            volumeProvider = new VolumeSampleProvider(bufferedWaveProvider.ToSampleProvider());

            waveIn.DataAvailable += OnDataAvailable;
        }

        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            int availableSpace = bufferedWaveProvider.BufferLength - bufferedWaveProvider.BufferedBytes;
            if (e.BytesRecorded <= availableSpace)
            {
                bufferedWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
            }
            else
            {
                // Логирование или обработка ситуации переполнения
            }

            LatestAudioData = e.Buffer;
            PlayAudioBytes(e.Buffer, 5);
        }

        public void Start()
        {
            waveIn.StartRecording();
            
        }

        public void OutPlayAudio(float volume)
        {
            Volume = volume;
            if (waveOut == null)
            {
                waveOut = new WaveOutEvent();
                waveOut.Init(volumeProvider);
            }
            if (waveOut.PlaybackState != PlaybackState.Playing)
            {
                waveOut.Play();
            }
        }

        public void PlayAudioBytes(byte[] bytesAudio, float volume)
        {
            /*int availableSpace = bufferedWaveProvider.BufferLength - bufferedWaveProvider.BufferedBytes;
            if (bytesAudio.Length <= availableSpace)
            {
                bufferedWaveProvider.AddSamples(bytesAudio, 0, bytesAudio.Length);
            }
            else
            {
                // Логирование или обработка ситуации переполнения
            }*/

            Volume = volume;
            if (waveOut == null)
            {
                waveOut = new WaveOutEvent();
                waveOut.Init(volumeProvider);
                waveOut.Play();
            }
            else if (waveOut.PlaybackState != PlaybackState.Playing)
            {
                waveOut.Play();
            }
        }

        public void OutStopAudio()
        {
            waveOut?.Stop();
        }

        public void StopRecording()
        {
            waveIn?.StopRecording();
            waveOut?.Stop();
        }

        public void StartRecording()
        {
            waveIn?.StartRecording();
            waveOut?.Play();
        }
    }
}
