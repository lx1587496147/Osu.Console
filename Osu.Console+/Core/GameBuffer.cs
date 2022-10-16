namespace Osu.Console.Core
{
    public class GameBuffer
    {
        private (char Char, (byte, byte, byte) Color)[][] Buffer = Array.Empty<(char Char, (byte, byte, byte) Color)[]>();
        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;
        public void InitConsole()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                NativeApi.EnableANSI();
#pragma warning disable CA1416 // 验证平台兼容性
                System.Console.BufferHeight = System.Console.WindowHeight;
                System.Console.BufferWidth = System.Console.WindowWidth;
#pragma warning restore CA1416 // 验证平台兼容性
            }
            System.Console.Write("\u001b[?25l");
            Width = System.Console.BufferWidth;
            Height = System.Console.BufferHeight;
        }
        public void ResizeBuffer()
        {
            Buffer = Enumerable.Range(0, Height).Select(x => new (char Char, (byte, byte, byte) Color)[Width]).ToArray();
        }
        public async void PushFrame()
        {
            System.Console.CursorLeft = 0;
            System.Console.CursorTop = 0;
            StringWriter s = new();
            int i = 0;
            foreach (var line in Buffer)
            {
                foreach (var pixel in line)
                {
                    if (pixel.Char != '\0')
                        s.Write($"\u001B[38;2;{pixel.Color.Item1};{pixel.Color.Item2};{pixel.Color.Item3}m{pixel.Char}\u001b[0m");
                    else
                        s.Write(' ');
                }
                i++;
                if (i != Buffer.Length)
                    s.Write('\n');
            }
            await System.Console.Out.WriteAsync(s.ToString());
            s.Dispose();
        }
        public void Clear()
        {
            foreach (var line in Buffer)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    line[i] = default;
                }
            }
        }
        public const double Ratio = 1.7;
        public void DrawEplise(Func<char> fill, (byte, byte, byte) clr, int x, int y, int a, int b)
        {
            int max = (int)(Math.Max(a, b) * Ratio);
            for (double i = -max; i < max; i++)
            {
                for (double j = -max; j < max; j++)
                {
                    if (Math.Abs(Math.Sqrt(i * i + j * j * Ratio * Ratio) - a) < 0.56) // 近似
                    {
                        TrySetPixel((fill(), clr), (int)(x + i), y + (int)j);
                    }
                }
            }
        }
        public void FillEplise(Func<char> fill, (byte, byte, byte) clr, int x, int y, int a, int b)
        {
            int max = (int)(Math.Max(a, b) * Ratio);
            for (double i = -max; i < max; i++)
            {
                for (double j = -max; j < max; j++)
                {
                    if (Math.Sqrt(i * i + j * j * Ratio * Ratio) <= a) // 近似
                    {
                        TrySetPixel((fill(), clr), (int)(x + i), y + (int)j);
                    }
                }
            }
        }
        public void DrawString(string str, (byte, byte, byte) clr, int x, int y)
        {
            int i = 0;
            foreach (char chr in str)
            {
                if (chr == '\n')
                {
                    y++;
                    i = 0;
                    continue;
                }
                TrySetPixel((chr, clr), x + i, y);
                i++;
            }
        }
        public void SetPixel((char Char, (byte, byte, byte) Color) data, int x, int y)
        {
            if (x < 0 || x > Width)
                throw new ArgumentOutOfRangeException(nameof(x));
            if (y < 0 || y > Height)
                throw new ArgumentOutOfRangeException(nameof(y));
            Buffer[y][x] = data;
        }
        public bool TrySetPixel((char Char, (byte, byte, byte) Color) data, int x, int y)
        {
            if (x < 0 || x >= Width)
                return false;
            if (y < 0 || y >= Height)
                return false;
            Buffer[y][x] = data;
            return true;
        }
        public (char Char, (byte, byte, byte) Color) GetPixel(int x, int y)
        {
            if (x < 0 || x >= Width)
                throw new ArgumentOutOfRangeException(nameof(x));
            if (y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException(nameof(y));
            return Buffer[y][x];
        }
        public (char Char, (byte, byte, byte) Color)? TryGetPixel(int x, int y)
        {
            if (x < 0 || x > Width)
                return null;
            if (y < 0 || y > Height)
                return null;
            return Buffer[y][x];
        }
    }
}
