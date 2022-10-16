using Osu.Console.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osu.Console.Gameplay
{
    public class OsuStdGameplay
    {
        private OsuStdRuntimeBeatmap? bm;
        private TimeSpan begintime;
        private TimeSpan timenow;
        private List<OsuStdRuntimeBeatmap.HitObject> visbleObjects = new();
        private List<OsuStdRuntimeBeatmap.HitObject> hitedObjects = new();
        private int Score = 0;
        private int greats = 0;
        private int oks = 0;
        private int mehs = 0;
        private int misses = 0;
        private int combo = 0;
        public void Begin(TimeSpan time)
        {
            begintime = time;
        }
        public void Tick(TimeSpan time)
        {

        }
        static ((double x, double y), (double width, double height)) GetPlayfieldRect(GameBuffer gb, bool widescreen)
        {
            //widescreen 16:9 std 4:3
            double ratio = widescreen ? 2.6 : 2; // Width / Height
            double bufferx = 0;
            double buffery = 0;
            double bufferwidth = 0;
            double bufferheight = 0;
            var sh = gb.Height * ratio;
            if (gb.Width > sh) // 过宽
            {
                bufferwidth = gb.Height * ratio;
                bufferheight = gb.Height;
                buffery = 0;
                bufferx = (gb.Width - bufferwidth) / 2;
            }
            else if (gb.Width == sh)
            {
                bufferx = 0;
                buffery = 0;
                bufferwidth = gb.Width;
                bufferheight = gb.Height;
            }
            else // 过高
            {
                bufferwidth = gb.Width;
                bufferheight = gb.Width / ratio;
                buffery = (gb.Height - bufferheight) / 2;
                bufferx = 0;
            }

            return ((bufferx, buffery), (bufferwidth, bufferheight));
        }
        // Playfield 标准大小: 512 384
        static (double width, double height) BeatmapPlayfieldSize = (512, 384);

        static (double x, double y) ConvertToBufferLocation(GameBuffer gb, (double x, double y) loc, bool widescreen)
        {
            var rect = GetPlayfieldRect(gb, widescreen);
            var stdlocx = loc.x / BeatmapPlayfieldSize.width;
            var stdlocy = loc.y / BeatmapPlayfieldSize.height;

            return (rect.Item1.x + stdlocx * rect.Item2.width, rect.Item1.y + stdlocy * rect.Item2.height);
        }

    }
}
