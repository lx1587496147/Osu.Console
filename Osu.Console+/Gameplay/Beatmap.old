using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osu.Console.Gameplay
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CategoryAttribute : Attribute
    {
        public CategoryAttribute(string category)
        {
            Category = category;
        }

        public string Category { get; set; }
    }
    public class Beatmap
    {
        public const float BASE_SCORING_DISTANCE = 100;
        public Beatmap(string audioFileName, int countdown, int mode, string titleUnicode, string artistUnicode, string creator, string version, double hPDrainRate, double circleSize, double overallDifficulty, double approachRate, double sliderMultiplier, double sliderTickRate, List<TimingPoint> timingPoints)
        {
            AudioFilename = audioFileName;
            Countdown = countdown;
            Mode = mode;
            TitleUnicode = titleUnicode;
            ArtistUnicode = artistUnicode;
            Creator = creator;
            Version = version;
            HPDrainRate = hPDrainRate;
            CircleSize = circleSize;
            OverallDifficulty = overallDifficulty;
            ApproachRate = approachRate;
            SliderMultiplier = sliderMultiplier;
            SliderTickRate = sliderTickRate;
            TimingPoints = timingPoints;
        }
        private Beatmap()
        {

        }
        public static Beatmap Parse(string content)
        {
            var bm = new Beatmap();
            StringReader sr = new(content);
            sr.ReadLine();
            string Category = "";
            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.StartsWith('[') && line.EndsWith(']'))
                {
                    Category = line.Substring(1, line.Length - 2);
                    continue;
                }
                if (String.IsNullOrWhiteSpace(line) || line.StartsWith("//"))
                    continue;
                switch (Category)
                {
                    case "Events":
                        {
                            break;
                        }
                    case "TimingPoints":
                        {
                            TimingPoint tp = new();
                            var parts = line.Split(',');
                            var refl = tp.GetType().GetProperties();
                            for (int i = 0; i < parts.Length; i++)
                            {
                                refl[i].SetValue(tp, TypeDescriptor.GetConverter(refl[i].PropertyType).ConvertFromString(parts[i]));
                            }
                            bm.TimingPoints.Add(tp);
                            break;
                        }
                    case "HitObjects":
                        {
                            HitObject tp = new();
                            var parts = line.Split(',');
                            var refl = tp.GetType().GetProperties();
                            int i = 0;
                            for (; i < parts.Length && i < refl.Length; i++)
                            {
                                refl[i].SetValue(tp, TypeDescriptor.GetConverter(refl[i].PropertyType).ConvertFromString(parts[i]));
                            }
                            if ((tp.Type & HitObjectType.Circle) != 0)
                            {
                                var refl2 = tp.circle.GetType().GetProperties();
                                for (; i < refl.Length + refl2.Length && i < parts.Length; i++)
                                {
                                    refl2[i - refl.Length].SetValue(tp.circle, TypeDescriptor.GetConverter(refl2[i - refl.Length].PropertyType).ConvertFromString(parts[i]));
                                }
                            }
                            if ((tp.Type & HitObjectType.Spinner) != 0)
                            {
                                var refl2 = tp.spinner.GetType().GetProperties();
                                for (; i < refl.Length + refl2.Length && i < parts.Length; i++)
                                {
                                    refl2[i - refl.Length].SetValue(tp.spinner, TypeDescriptor.GetConverter(refl2[i - refl.Length].PropertyType).ConvertFromString(parts[i]));
                                }

                            }
                            if ((tp.Type & HitObjectType.Slider) != 0)
                            {
                                var refl2 = tp.slider.GetType().GetProperties();
                                for (; i < refl.Length + refl2.Length && i < parts.Length; i++)
                                {
                                    refl2[i - refl.Length].SetValue(tp.slider, TypeDescriptor.GetConverter(refl2[i - refl.Length].PropertyType).ConvertFromString(parts[i]));
                                }
                                var data = tp.slider.rec.Split('|');// 分离控制点
                                PathType cur = PathType.Bezier;
                                List<(double X, double Y)> ControlPoints = new();
                                ControlPoints.Add((tp.X, tp.Y));
                                foreach (var p in data)
                                {
                                    if (string.IsNullOrWhiteSpace(p))
                                        continue;
                                    if ((p[0] >= 'a' && p[0] <= 'z') || (p[0] >= 'A' && p[0] <= 'Z'))
                                    {
                                        cur = (PathType)p[0];
                                    }
                                    else
                                    {
                                        var data2 = p.Split(":");
                                        var X = double.Parse(data2[0]);
                                        var Y = double.Parse(data2[1]);
                                        ControlPoints.Add((X, Y));
                                    }
                                }
                                switch (cur)
                                {
                                    case PathType.Catmull:
                                        {
                                            ControlPoints = PathApproximator.ApproximateCatmull(ControlPoints.Select(x => new Vector2((float)x.X, (float)x.Y)).ToArray()).Select(x => ((double)x.X, (double)x.Y)).ToList();
                                            break;
                                        }
                                    case PathType.Linear:
                                        {
                                            ControlPoints.Add(ControlPoints.Last());
                                            ControlPoints = PathApproximator.ApproximateBezier(ControlPoints.Select(x => new Vector2((float)x.X, (float)x.Y)).ToArray()).Select(x => ((double)x.X, (double)x.Y)).ToList();
                                            break;
                                        }
                                    case PathType.PerfectCurve:
                                        {
                                            ControlPoints = PathApproximator.ApproximateCircularArc(ControlPoints.Select(x => new Vector2((float)x.X, (float)x.Y)).ToArray()).Select(x => ((double)x.X, (double)x.Y)).ToList();
                                            break;
                                        }
                                    default:
                                        {
                                            ControlPoints = PathApproximator.ApproximateBezier(ControlPoints.Select(x => new Vector2((float)x.X, (float)x.Y)).ToArray()).Select(x => ((double)x.X, (double)x.Y)).ToList();
                                            break;
                                        }
                                }
                                double sumvec = 0;
                                (double X, double Y) last = default;
                                tp.slider.Path.Add(ControlPoints.First());
                                double scoringdis = BASE_SCORING_DISTANCE * bm.SliderMultiplier * bm.GetTp(tp.StartTime).SpeedMultiplier;
                                double vec = scoringdis / bm.GetTp(tp.StartTime).BeatLength;
                                double slidertickdis = scoringdis * bm.SliderTickRate;
                                double tickingsum = 0;
                                ControlPoints.ForEach(x => {
                                    if (last != default)
                                    {
                                        var len = Math.Sqrt(Math.Pow(x.X - last.X, 2) + Math.Pow(x.Y - last.Y, 2));
                                        sumvec += len;
                                        tickingsum += len;
                                        if (sumvec <= tp.slider.Length)
                                        {
                                            tp.slider.Path.Add((x.X, x.Y));
                                            if (tickingsum > slidertickdis)
                                            {
                                                tp.slider.Ticks.Add((x.X, x.Y));
                                                tickingsum -= slidertickdis;
                                            }
                                        }
                                    }
                                    last = x;
                                });
                                if (tp.slider.Ticks.Count > 1)
                                    tp.slider.Ticks.RemoveAt(tp.slider.Ticks.Count - 1);

                                tp.slider.Ticks.Add(tp.slider.Path.Last());
                                sumvec = Math.Min(tp.slider.Length, sumvec);
                                tp.slider.D = Math.Abs(tp.slider.RepeatCount * sumvec / vec);
                            }
                            tp.Color = ((byte)new Random().Next(80, 255), (byte)new Random().Next(80, 255), (byte)new Random().Next(80, 255));
                            bm.HitObjects.Add(tp);
                            break;
                        }
                    default:
                        {
                            var key = line.Split(':')[0];
                            var value = line.Split(':')[1];
                            var prop = bm.GetType().GetProperties().FirstOrDefault(x => x.Name == key);
                            prop?.SetValue(bm, TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromString(value));
                            break;
                        }
                }
            }
            return bm;
        }
        [Category("General")]
        public string AudioFilename { get; set; } = "";
        [Category("General")]
        public int Countdown { get; set; } = 0;
        [Category("General")]
        public int Mode { get; set; } = 1;
        [Category("General")]
        public int WidescreenStoryboard { get; set; } = 1;

        [Category("Metadata")]
        public string TitleUnicode { get; set; } = "";
        [Category("Metadata")]
        public string ArtistUnicode { get; set; } = "";
        [Category("Metadata")]
        public string Creator { get; set; } = "";
        [Category("Metadata")]
        public string Version { get; set; } = "";
        [Category("Difficulty")]
        public double HPDrainRate { get; set; } = 0;
        [Category("Difficulty")]
        public double CircleSize { get; set; } = 0;
        [Category("Difficulty")]
        public double OverallDifficulty { get; set; } = 0;
        [Category("Difficulty")]
        public double ApproachRate { get; set; } = 0;
        [Category("Difficulty")]
        public double SliderMultiplier { get; set; } = 0;
        [Category("Difficulty")]
        public double SliderTickRate { get; set; } = 0;
        public class TimingPoint
        {
            public double Time { get; set; }
            public double BeatLength { get; set; } = 100;
            public int TimeSignature { get; set; } = 4;
            public double SpeedMultiplier { get; set; }
            public SampleBank SampleBank { get; set; }
            public int SampleVolume { get; set; }
            public int TimingChange { get; set; }
            public EffectFlags Effects { get; set; }
        }
        public List<TimingPoint> TimingPoints { get; set; } = new();
        public List<HitObject> HitObjects { get; set; } = new();
        public class HitObject
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double StartTime { get; set; }
            public HitObjectType Type { get; set; }
            public HitSoundType SoundType { get; set; }
            public HitObjectCircle circle = new();
            public HitObjectSlider slider = new();
            public HitObjectSpinner spinner = new();
        }
        public class HitObjectCircle
        {
            public string? CustomSampleBanks { get; set; }
        }
        public class HitObjectSlider
        {
            public string? PathRecord { get; set; }
            public int RepeatCount { get; set; }
            public double Length { get; set; }
            public string? CustomSampleBanks { get; set; }
        }
        public class HitObjectSpinner
        {
            public double EndTime { get; set; }
            public string? CustomSampleBanks { get; set; }
        }
    }
}
