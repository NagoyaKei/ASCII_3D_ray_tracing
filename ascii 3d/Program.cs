using System;
using System.Threading;

namespace ascii_3d
{
    internal class Program
    {
        private const int W = 120;
        private const int H = 29;
        private const int PIXELS = W * H;
        private const string gradient = " .':,\"!/r(l1Z4H9W8$@";
        private const int gSize = 20;
        private const double ratio = 1.8;
        private const int FPS = 1;

        public static void Main(string[] args)
        {

            while (true)
            {
                char[] screen = new char[W * H];
                int curPos = 0;

                for (int i = 0; i < H; i++)
                {
                    for (int j = 0; j < W; j++)
                    {
                        curPos = j + i * W;

                        vec2 uv = new vec2(i, j) / new vec2(H, W) - 0.5;
                        uv.y *= ratio;
                        int color = 0;
                        if (vec.length(uv) < 0.5) color = 10;

                        screen[curPos] = gradient[color];
                    }
                    if (curPos != PIXELS - 1)
                        screen[curPos] = '\n';
                }
                

                Console.Write(screen);
                    Thread.Sleep(1000 / FPS);
            }

            Console.ReadKey();
        }
    }
}
