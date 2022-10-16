using Osu.Console.Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osu.Console.Core
{
    public class OsuGameController : IGameController
    {
        private Beatmap? bm;
        private TimeSpan begintime;
        private TimeSpan timenow;
        private List<Beatmap.HitObject> visbleObjects = new();
        private List<Beatmap.HitObject> hitedObjects = new();
        private Game? game;
        private int Score = 0;
        private int greats = 0;
        private int oks = 0;
        private int mehs = 0;
        private int misses = 0;
        private int combo = 0;
        void IGameController.Init(Osu.Console.Game game)
        {
            this.game = game;
        }
        public void LoadBeatmap(string bmp)
        {
            bm = Beatmap.Parse(bmp);
        }
        const double FromStart = 2;
        const double Range = 8;
        const double ComboR = 100;
        void IGameController.Click(int x, int y, int up)
        {
            if (game != null)
            {
                foreach (var item in visbleObjects.ToList())
                {
                    if ((item.Type & Beatmap.HitObjectType.Circle) != 0)
                        if ((int)(item.X / 510 * game.Get<BufferController>().CurrentBuffer!.Width) > x - 10 && (int)(item.X / 510 * game.Get<BufferController>().CurrentBuffer!.Width) < x + 10
                            && (int)(item.Y / 400 * game.Get<BufferController>().CurrentBuffer!.Height) > y - 10 && (int)(item.Y / 400 * game.Get<BufferController>().CurrentBuffer!.Height) < y + 10)
                        {
                            if (up == 0)
                            {
                                hitedObjects.Add(item);
                                combo++;
                                int judge = 300;
                                Score += judge * combo;
                                game.Get<SoundController>().PlayHitSound(SoundController.HitSounds.HitNormal);
                                break; //防止自动打串(
                            }
                        }
                }
            }
        }
        void IGameController.Tick(System.TimeSpan fromRun)
        {
            if (bm != null)
            {
                if (begintime == default)
                    begintime = fromRun;
                foreach (var item in hitedObjects.ToList())
                {
                    visbleObjects.Remove(item);
                }

                foreach (var obj in bm.HitObjects)
                {
                    var a = obj.StartTime / 1000 - (fromRun.TotalSeconds - begintime.TotalSeconds);
                    if ((obj.Type & Beatmap.HitObjectType.Circle) != 0)
                    {
                        if (a < FromStart && a > -1)
                        {
                            if (visbleObjects.Find(x => x == obj) == null && hitedObjects.Find(x => x == obj) == null)
                                visbleObjects.Add(obj);
                        }
                        else
                        {
                            if (visbleObjects.Find(x => x == obj) != null && hitedObjects.Find(x => x == obj) == null)
                            {
                                misses++;
                                combo = 0;
                                visbleObjects.Remove(obj);
                            }
                        }
                    }
                    if ((obj.Type & Beatmap.HitObjectType.Slider) != 0)
                    {
                        if (a < FromStart && a > (-obj.slider.D / 1000))
                        {
                            if (visbleObjects.Find(x => x == obj) == null)
                                visbleObjects.Insert(0, obj);
                        }
                        else
                        {
                            //misses++;
                            //combo = 0;
                            visbleObjects.Remove(obj);
                        }
                    }
                    if ((obj.Type & Beatmap.HitObjectType.Spinner) != 0)
                    {
                        if (a < FromStart && a > -obj.spinner.EndTime / 1000)
                        {
                            if (visbleObjects.Find(x => x == obj) == null)
                                visbleObjects.Add(obj);
                        }
                        else
                        {
                            //misses++;
                            //combo = 0;
                            visbleObjects.Remove(obj);
                        }
                    }
                }
                timenow = fromRun;
            }
        }

        Random rnd = new();
        void IGameController.PushFrame(Osu.Console.Core.GameBuffer buffer)
        {
            foreach (var obj in visbleObjects)
            {
                if ((obj.Type & Beatmap.HitObjectType.Circle) != 0)
                {
                    var a = obj.StartTime / 1000 - (timenow.TotalSeconds - begintime.TotalSeconds);
                    if (a > 0 && a < FromStart)
                    {
                        a /= FromStart;
                        var objloc = ConvertToBufferLocation(buffer, (obj.X, obj.Y), bm.WidescreenStoryboard == 1);
                        buffer.DrawEplise(() => (char)rnd.Next('A', 'Z'), obj.Color, (int)objloc.x, (int)objloc.y, (int)(16 * a) + 4, 0);
                        buffer.FillEplise(() => (char)rnd.Next('a', 'z'), obj.Color, (int)objloc.x, (int)objloc.y, 4, 0);
                        buffer.DrawEplise(() => (char)rnd.Next('a', 'z'), (0, 255, 0), (int)objloc.x, (int)objloc.y, 4, 0);
                        buffer.TrySetPixel(('O', (0, 255, 0)), (int)objloc.x, (int)objloc.y);
                    }
                }
                if ((obj.Type & Beatmap.HitObjectType.Slider) != 0)
                {
                    var a = obj.StartTime / 1000 - (timenow.TotalSeconds - begintime.TotalSeconds);
                    obj.slider.Path.Take((int)((-a + 1.5) * obj.slider.Path.Count)).ToList().Select(x =>
                    {
                        var xloc = ConvertToBufferLocation(buffer, (x.Item1, x.Item2), bm.WidescreenStoryboard == 1);
                        buffer.FillEplise(() => (char)rnd.Next('a', 'z'), obj.Color, (int)xloc.x, (int)xloc.y, 4, 0);
                        return x;
                    }).ToList().ForEach(x=>
                    {
                        if (obj.slider.Ticks.Contains(x))
                        {
                            var tickloc = ConvertToBufferLocation(buffer, (x.Item1, x.Item2), bm.WidescreenStoryboard == 1);
                            buffer.SetPixel(('*', (255, 255, 255)), (int)tickloc.x, (int)tickloc.y);
                        }
                    });
                    {
                        var x = obj.slider.Path.Take((int)(-a / obj.slider.D * 1000 * obj.slider.Path.Count)).LastOrDefault();
                        if (x != default)
                        {
                            var xloc = ConvertToBufferLocation(buffer, (x.Item1, x.Item2), bm.WidescreenStoryboard == 1);
                            buffer.DrawEplise(() => (char)rnd.Next('a', 'z'), (0, 255, 0), (int)xloc.x, (int)xloc.y, 4, 0);
                        }
                    }
                    if (a > 0 && a < FromStart)
                    {
                        a /= FromStart;
                        var objloc = ConvertToBufferLocation(buffer, (obj.X, obj.Y), bm.WidescreenStoryboard == 1);
                        buffer.DrawEplise(() => (char)rnd.Next('A', 'Z'), obj.Color, (int)objloc.x, (int)objloc.y, (int)(16 * a) + 4, 0);
                        buffer.FillEplise(() => (char)rnd.Next('a', 'z'), obj.Color, (int)objloc.x, (int)objloc.y, 4, 0);
                        buffer.DrawEplise(() => (char)rnd.Next('a', 'z'), (0, 255, 0), (int)objloc.x, (int)objloc.y, 4, 0);
                        buffer.TrySetPixel(('+', (0, 255, 0)), (int)objloc.x, (int)objloc.y);
                    }

                }
                //if ((obj.Type & Beatmap.HitObjectType.Spinner) != 0)
                //{
                //    var a = obj.StartTime / 1000 - (timenow.TotalSeconds - begintime.TotalSeconds);
                //    if (a > 0 && a < FromStart)
                //    {
                //        buffer.TrySetPixel(('=', (0, 255, 0)), (int)(obj.X / 510 * buffer.Width), (int)(obj.Y / 400 * buffer.Height + a * Range));
                //        buffer.TrySetPixel(('=', (0, 255, 0)), (int)(obj.X / 510 * buffer.Width), (int)(obj.Y / 400 * buffer.Height - a * Range));
                //        buffer.TrySetPixel(('=', (0, 255, 0)), (int)(obj.X / 510 * buffer.Width + a * Range * 1.5), (int)(obj.Y / 400 * buffer.Height));
                //        buffer.TrySetPixel(('=', (0, 255, 0)), (int)(obj.X / 510 * buffer.Width - a * Range * 1.5), (int)(obj.Y / 400 * buffer.Height));
                //        buffer.TrySetPixel(('-', (0, 255, 0)), (int)(obj.X / 510 * buffer.Width), (int)(obj.Y / 400 * buffer.Height));
                //    }
                //}
            }
            var scorestr = Score.ToString("D8") + "    " + combo.ToString() + "x";
            var loc = buffer.Width / 2 - scorestr.Length / 2;
            buffer.DrawString(scorestr, (255, 255, 255), loc, 0);
        }
    }
}
