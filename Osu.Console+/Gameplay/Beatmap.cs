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
            throw new NotImplementedException();
        }
        public Dictionary<string, (string, string)[]> UnresolvedData { get; set; } = new();
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
