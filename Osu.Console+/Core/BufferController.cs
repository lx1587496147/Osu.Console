namespace Osu.Console.Core
{
    public class BufferController : IGameController
    {
        private Game? game;
        private GameBuffer? buffer;
        public GameBuffer? CurrentBuffer => buffer;
        void IGameController.Init(Game game)
        {
            this.game = game;
            buffer = new();
            buffer.InitConsole();
            buffer.ResizeBuffer();
        }
        void IGameController.Tick(TimeSpan fromRun)
        {
            if (buffer != null)
                game!.PushFrame(buffer);
        }
        void IGameController.Resize(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentOutOfRangeException(nameof(width));
            if (buffer != null)
            {
                buffer.Height = System.Console.WindowHeight;
                buffer.Width = System.Console.WindowWidth;
                buffer.ResizeBuffer();
            }
        }
    }
}
