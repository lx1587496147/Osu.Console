using Osu.Console.Core;

namespace Osu.Console
{
    public class Game
    {
        private List<IGameController> Controllers = new();
        public Game()
        {

        }
        public Game Use<T>() where T : IGameController, new()
        {
            Controllers.Add(new T());
            return this;
        }
        public T Get<T>() where T : IGameController
        {
            return (T)Controllers.First(x => x is T);
        }
        public async void PlayWelcome()
        {
            Get<SoundController>().PlayWelcome();
        }
        public async void StartInputCalibration()
        {
            Get<SoundController>().PlayTutorial();
            //Get<OsuScreenController>().LoadBeatmap(Properties.Resources.NU_KO___Pochiko_no_Shiawase_na_Nichijou__Long_Version___SnowNiNo____Hard_);
        }
        public void Run()
        {
            Controllers.ForEach(x => x.Init(this));
        }

        public void Resize(int width, int height)
        {
            Controllers.ForEach(a => a.Resize(width, height));
        }
        public void PushFrame(GameBuffer buffer)
        {
            buffer.Clear();
            Controllers.ForEach(a => a.PushFrame(buffer));
            buffer.PushFrame();
        }
        public void Click(int x, int y, int up)
        {
            Controllers.ForEach(a => a.Click(x, y, up));
        }
        public void Move(int x, int y)
        {
            Controllers.ForEach(a => a.Move(x, y));
        }
        public void Tick(TimeSpan fromRun)
        {
            Controllers.ForEach(x => x.Tick(fromRun));
        }
        public void Stop()
        {
            Controllers.ForEach(x => x.Destory());
        }
        public void Key(KeyEvent cki)
        {
            Controllers.ForEach(x => x.Key(cki));
        }
        public void Focus(bool focus)
        {
            Controllers.ForEach(x => x.Focus(focus));
        }
    }
}
