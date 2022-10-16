namespace Osu.Console.Core
{
    public class CursorController : IGameController
    {
        internal (int x, int y) mousepos;
        private bool clicked;
        private Game game;
        void IGameController.Init(Game game)
        {
            this.game = game;
        }
        void IGameController.PushFrame(GameBuffer buffer)
        {
            buffer.TrySetPixel((clicked ? '+' : 'x', (255, 255, 255)), mousepos.x, mousepos.y);
        }
        void IGameController.Click(int x, int y, int up)
        {
            clicked = up == 0;
        }
        void IGameController.Move(int x, int y)
        {
            mousepos.x = x;
            mousepos.y = y;
        }
    }
}
