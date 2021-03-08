using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Timers;

namespace ascii_3d
{
    internal class Program
    {
        private static Random rnd;
        private static int W = Console.BufferWidth;
        private const int H = 29;
        private static int PIXELS;
        private const string gradient = " .':,\"!/r(l1Z4H9W8$@";
        private static int gMaxIndex = gradient.Length - 1;
        private const double ratio = 1.8;
        private const int FPS = 60;
        private const bool INFO_ENABLE = true;
        private const int MAP_W = 10;
        private const int MAP_H = 10;

        static double xPos = 0d;
        static double xPosMax = 15d;
        static double increment = 0.05d;
        static double deltaT = 0d;
        static double diffContrast = 5;

        private static int frame = 0;
        private static TimeSpan lastFrameTime = TimeSpan.Zero;
        // private double realFPS => lastFrameTime / 1000 / ;
        // map
        static List<char> mapChar = new List<char>();
        static List<string> mapLines = new List<string>();

        public static void Main(string[] args)
        {
            foreach (String mapLine in File.ReadAllLines("maps/map1.txt")) {
                mapChar.AddRange(mapLine.ToCharArray());
                mapLines.Add(mapLine);
            }

            rnd = new Random();
            double rndD = rnd.NextDouble();
            PIXELS = W * H;
            Console.CursorVisible = false;
            while (true)
            {
                DateTime startFrame = DateTime.Now;
                char[] screen = new char[PIXELS];
                int curPos = 0;
                vec3 light = vec.norm(new vec3(-0.6, Math.Sin(deltaT * rndD) * 2, -1.0));
                double sphereR = 1.5d;

                vec3 spherePos = new vec3(0, 0, 0);

                for (int i = 0; i < W; i++)
                {
                    for (int j = 0; j < H; j++)
                    {
                        curPos = i + j * W;

                        vec2 uv = new vec2(i, j) / new vec2(W, H) - new vec2(0.5, 0.5);
                        uv.x *= ratio;

                        vec3 cameraOrigin = new vec3(-5, Math.Sin(deltaT) / 2, 0);
                        vec3 cameraDirection = vec.norm(new vec3(1, uv));

                        vec2 it = vec.sphIntersect(cameraOrigin, cameraDirection, spherePos, sphereR);
                        vec3 oneIfIntersect = vec.castRay(cameraOrigin, cameraDirection, it, light);
                        int color = (int)(vec.avg(oneIfIntersect) * gMaxIndex);

                        screen[curPos] = gradient[color > gMaxIndex ? gMaxIndex : color <= 0 ? 0 : color];
                    }
                }

                writeMap(screen);
                writeInfo(screen);

                // это нужно что бы не дергалась консоль, курсор почему-то сдвигается вправо
                Console.CursorLeft = 0;
                Console.CursorTop = 0;
                Console.Write(screen);

                lastFrameTime = DateTime.Now - startFrame;

                Thread.Sleep(1000 / FPS);

                deltaT += increment;
            }

            Console.ReadKey(); 
        }

        private static void writeMap(char[] screen)
        {
            writeLinesOnScreen(screen, mapLines, 1, 1);
        }

        private static void writeInfo(char[] screen)
        {
            if (!INFO_ENABLE) return;
            string infoText = $"FPS {FPS} ({(1000 / lastFrameTime.TotalMilliseconds):F0})";
            writeLineOnScreen(screen, infoText, 0);
        }

        public static int mapValue(double val, int map)
        {
            return (int)(map * val);
        }

        public static void writeLineOnScreen(char[] screen, string str, int posX, int lineNum = 0)
        {
            int shift = (lineNum * W) + posX;
            Array.Copy(str.ToCharArray(), 0, screen,  shift, str.Length);
        }

        public static void writeLinesOnScreen(char[] screen, List<string> lines, int posX, int posY)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                writeLineOnScreen(screen, lines[i], posX, posY + i);
            }
        }

        // TODO:
        // if (vec.length(uv) < 0.35) color = 10; // TODO: круг разобраться как работает
    }
}
