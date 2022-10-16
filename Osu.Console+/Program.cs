using Osu.Console.Core;
using static System.Console;
namespace Osu.Console;
internal class Program
{
    static void Main(string[] args)
    {
        var game = new Game()
            .Use<FpsOverlayController>()
            .Use<OsuScreenController>()
            .Use<CursorController>()
            .Use<BufferController>()
            .Use<Win32ConsoleInputContoller>()
            .Use<StopWatchTickingController>()
            .Use<KeyboardTranslatorController>()
            .Use<SoundController>();
        game.Get<StopWatchTickingController>().TargetFps = 1000;
        WriteLine("Welcome to Osu!");
        game.PlayWelcome();
        game.Run();
        game.StartInputCalibration();
        while (true)
        {
            Thread.Sleep(1);
        }
    }
}