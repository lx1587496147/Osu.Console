using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osu.Console.Gameplay
{
    public struct OsuBeatmapDifficulty
    {
        /// <summary>
        /// The drain rate of the associated beatmap.
        /// </summary>
        public float DrainRate { get; set; }

        /// <summary>
        /// The circle size of the associated beatmap.
        /// </summary>
        public float CircleSize { get; set; }

        /// <summary>
        /// The overall difficulty of the associated beatmap.
        /// </summary>
        public float OverallDifficulty { get; set; }

        /// <summary>
        /// The approach rate of the associated beatmap.
        /// </summary>
        public float ApproachRate { get; set; }

        /// <summary>
        /// The slider multiplier of the associated beatmap.
        /// </summary>
        public double SliderMultiplier { get; set; }

        /// <summary>
        /// The slider tick rate of the associated beatmap.
        /// </summary>
        public double SliderTickRate { get; set; }
    }
    public class OsuStdRuntimeBeatmap
    {
        public List<HitObject> HitObjects { get; set; } = new();
        public List<TimingPoint> TimingPoints { get; set; } = new();
        public abstract class TimingPoint
        {
            public double Time { get; set; }
        }
        public class EffectTimingPoint : TimingPoint
        {
            public double SpeedMultiplier { get; set; }
            public SampleBank SampleBank { get; set; }
            public int SampleVolume { get; set; }
            public EffectFlags Effects { get; set; } = EffectFlags.None;
        }
        public class BaseTimingPoint : TimingPoint
        {
            public double BeatLength { get; set; } = 100;
            public int TimeSignature { get; set; } = 4;
        }
        public OsuBeatmapDifficulty Difficulty { get; set; } = new();
        public TimingPoint GetCurrentBaseTimingPoint(double time)
        {
            BaseTimingPoint? timingPoint = default;
            foreach (TimingPoint tp in TimingPoints)
            {
                if (tp is BaseTimingPoint tp2)
                {
                    timingPoint = tp2;
                }
                if (tp.Time >= time)
                {
                    break;
                }
            }
            return timingPoint ?? TimingPoints.Last(x => x is BaseTimingPoint);
        }
        public TimingPoint GetCurrentEffectTimingPoint(double time)
        {
            EffectTimingPoint? timingPoint = default;
            foreach (TimingPoint tp in TimingPoints)
            {
                if (tp is EffectTimingPoint tp2)
                {
                    timingPoint = tp2;
                }
                if (tp.Time >= time)
                {
                    break;
                }
            }
            return timingPoint ?? new EffectTimingPoint();
        }
        public abstract class HitObject
        {
            public HitObject(Beatmap.HitObject orig, OsuStdRuntimeBeatmap beatmap)
            {
                Location = (orig.X, orig.Y);
                StartTime = orig.StartTime;
                SoundType = orig.SoundType;
                CustomSampleBanks = orig.circle?.CustomSampleBanks ?? orig.spinner?.CustomSampleBanks ?? orig.slider?.CustomSampleBanks;
                NewCombo = (orig.Type & HitObjectType.NewCombo) == HitObjectType.NewCombo;
                this.beatmap = beatmap;
            }
            protected OsuStdRuntimeBeatmap beatmap;
            public Point Location { get; set; } = default;
            public double StartTime { get; set; }
            public HitSoundType SoundType { get; set; }
            public string? CustomSampleBanks { get; set; }
            public bool NewCombo { get; set; } = default;
            protected double GetPreempt(double ar)
            {
                return DifficultyPreempt(ar);
            }
            public double GetFadeIn(double ar)
            {
                return DifficultyFadeIn(GetPreempt(ar));
            }
            public double GetApproachCircleFadeIn()
            {
                var ar = beatmap.Difficulty.ApproachRate;
                var pr = GetPreempt(ar);
                var fi = GetFadeIn(ar);
                return Math.Min(fi * 2, pr);
            }
            protected virtual double GetDisappearDeltaTime()
            {
                var res = GetHitRanges(beatmap.Difficulty.OverallDifficulty);
                return res[HitResult.Meh];
            }
            // 是否显示
            public virtual bool IsVisible(double time)
            {
                var ar = beatmap.Difficulty.ApproachRate;
                var od = beatmap.Difficulty.OverallDifficulty;
                return ShouldPreempt(time) || !ShouldDisappear(time);
            }
            // Hidden Behavior: Fade Out
            public virtual bool ShouldPreempt(double time)
            {
                var v = StartTime - time;
                return v > 0 && v <= GetPreempt(beatmap.Difficulty.ApproachRate);
            }
            public virtual bool ShouldDisappear(double time)
            {
                var v = StartTime - time;
                var delta = GetDisappearDeltaTime();
                if (!ShouldPreempt(time))
                {
                    return v > delta;
                }
                return false;
            }
        }
        public class HitObjectCircle : HitObject
        {
            public HitObjectCircle(Beatmap.HitObject orig,OsuStdRuntimeBeatmap beatmap) : base(orig,beatmap)
            {
                Debug.Assert((orig.Type & HitObjectType.Circle) == HitObjectType.Circle);
            }
        }
        public class HitObjectSlider : HitObject
        {
            public HitObjectSlider(Beatmap.HitObject orig, OsuStdRuntimeBeatmap beatmap) : base(orig, beatmap)
            {
                Debug.Assert((orig.Type & HitObjectType.Slider) == HitObjectType.Slider);
            }

            public int RepeatCount { get; set; }
            public double TotalLength => Length * RepeatCount;
            public double Length => CalcLength();
            public double CalcLength()
            {
                return ConvertToLengths().Sum();
            }
            private List<double> ConvertToLengths()
            {
                List<double> buf = new();
                Point last = InvaildPoint;
                Path.ForEach(((double x, double y) p) =>
                {
                    if (last != InvaildPoint)
                    {
                        buf.Add(Math.Sqrt(Math.Pow(p.x - last.Item1, 2) + Math.Pow(p.y - last.Item2, 2)));
                    }
                    last = p;
                }
                );
                return buf;
            }
            public Point GetSliderBallPosition()
            {
                return InvaildPoint;
            }
            public override bool ShouldDisappear(double time)
            {
                if(base.ShouldDisappear(time))
                {
                    var len = Length * RepeatCount;
                    var basetp = beatmap.GetCurrentBaseTimingPoint(StartTime);
                    var effecttp = beatmap.GetCurrentEffectTimingPoint(StartTime);

                }
                return false;
            }
            public double Speed { get; set; }
            // For score v1 calc the score of a slider(also judgement)
            public List<Point> Ticks { get; set; } = new();
            public List<Point> Path { get; set; } = new();
        }
        public class HitObjectSpinner : HitObject
        {
            public HitObjectSpinner(Beatmap.HitObject orig, OsuStdRuntimeBeatmap beatmap) : base(orig, beatmap)
            {
                Debug.Assert((orig.Type & HitObjectType.Spinner) == HitObjectType.Spinner);
                EndTime = orig.spinner.EndTime;
            }

            public double EndTime { get; set; }
        }
    }
}
