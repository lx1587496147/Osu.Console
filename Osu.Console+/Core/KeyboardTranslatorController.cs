using System.Diagnostics;

namespace Osu.Console.Core
{
    class KeyboardTranslatorController : IGameController
    {
        private Game? game;
        private bool zkey = false;
        private bool xkey = false;
        void IGameController.Init(Game game)
        {
            this.game = game;
        }
        void IGameController.Key(KeyEvent cki)
        {
            if (game != null)
            {
                var zkeyn = cki.Key == ConsoleKey.Z && cki.Pressed;
                var xkeyn = cki.Key == ConsoleKey.X && cki.Pressed;
                if (zkeyn ^ zkey || xkeyn ^ xkey)
                {
                    var pos = game.Get<CursorController>().mousepos;
                    game.Click(pos.x, pos.y, cki.Pressed ? 0 : 1);
                    zkey = zkeyn;
                    xkey = xkeyn;
                }
            }
        }
    }
}
