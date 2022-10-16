using NAudio.Wave;

namespace Osu.Console.Core
{
    public class SoundController : IGameController
    {
        public void PlayWelcome()
        {
            var ms = new MemoryStream(Properties.Resources.osu_Game_Resources_Samples_Intro_Welcome_welcome);
            var audioFile = new Mp3FileReader(ms);
            var outdev = new WaveOutEvent();
            outdev.Init(audioFile);
            outdev.Play();
            outdev.PlaybackStopped += (s, e) => { outdev.Dispose(); audioFile.Dispose(); ms.Dispose(); };
        }
        public void PlayTutorial()
        {
            var ms = new MemoryStream(Properties.Resources.audio);
            var audioFile = new Mp3FileReader(ms);
            var outdev = new WaveOutEvent();
            outdev.Init(audioFile);
            outdev.Play();
            outdev.PlaybackStopped += (s, e) => { outdev.Dispose(); audioFile.Dispose(); ms.Dispose(); };
        }
        public enum HitSounds
        {
            HitClap,
            HitFinish,
            HitNormal,
            HitWhistle,
            SliderSlide,
            SliderTick,
            SliderWhistle,
            SpinnerBonus,
            SpinnerSpin,
        }
        public void PlayHitSound(HitSounds res)
        {
            var ms = Properties.Resources.ResourceManager.GetStream("osu_Game_Resources_Samples_Gameplay_normal_" + res.ToString().ToLower());
            var audioFile = new WaveFileReader(ms);
            var outdev = new WaveOutEvent();
            outdev.Init(audioFile);
            outdev.Play();
            outdev.PlaybackStopped += (s, e) => { outdev.Dispose(); audioFile.Dispose(); };
        }
        public void PlayGoodbye()
        {
            var ms = new MemoryStream(Properties.Resources.osu_Game_Resources_Samples_Intro_Welcome_welcome);
            var audioFile = new Mp3FileReader(ms);
            var outdev = new WaveOutEvent();
            outdev.Init(audioFile);
            outdev.Play();
            outdev.PlaybackStopped += (s, e) => { outdev.Dispose(); audioFile.Dispose(); ms.Dispose(); };
        }
    }
}
