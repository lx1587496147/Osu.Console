using System.Diagnostics;

namespace Osu.Console.Core
{
    public class StopWatchTickingController : IGameController
    {
        private Stopwatch? watch;
        private Game? game;
        private bool stopTicking = false;
        public double TargetFps { get; set; } = 30.0;
        private double curfps = 0.0;
        public double CurrentFps => curfps;
        void IGameController.Init(Game game)
        {
            stopTicking = false;
            watch = new Stopwatch();
            watch.Start();
            this.game = game;
            Task.Run(() =>
            {
                TimeSpan lastcount = default;
                TimeSpan lastsleep = default;
                int frames = 0;
                while (!stopTicking)
                {
                    try
                    {
                        game.Tick(watch.Elapsed);
                    }
                    catch
                    {
                        Debugger.Break();
                    }
                    frames++;
                    if (watch.Elapsed - lastcount > new TimeSpan(0, 0, 1))
                    {
                        curfps = frames / (watch.Elapsed.TotalSeconds - lastcount.TotalSeconds);
                        frames = 0;
                        lastcount = watch.Elapsed;
                    }
                    while ((watch.Elapsed - lastsleep).Ticks < (long)(1 / TargetFps * 3000000))//魔法数字别乱搞（
                    {
                        Thread.Sleep(1);
                    }
                    lastsleep = watch.Elapsed;
                }
            });
        }
        void IGameController.Destory()
        {
            if (watch != null)
            {
                watch.Stop();
                watch = null;
            }
            stopTicking = true;
        }
    }
}
