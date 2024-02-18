using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ascii_3d
{
    internal class Program
    {
        private static Random rnd;
        private static int W = Console.BufferWidth;
        private const int H = 29;
        private static int PIXELS;
        private const string ASCII_GRADIENT = " .':,\"!/r(l1Z4H9W8$@";
        private static readonly int gMaxIndex = ASCII_GRADIENT.Length - 1;
        private const double RATIO = 1.8;
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
        private static bool isPressed_W = false;
        private static bool isPressed_A = false;
        private static bool isPressed_S = false;
        private static bool isPressed_D = false;
        private static bool isPressed_Q = false;
        private static bool isPressed_E = false;
        private static void keyPressCatcher(ConsoleKey key, out bool flag)
        {
            while (true)
            {
                if (key == Console.ReadKey(true).Key)
                {
                    flag = true;
                    Thread.Sleep(50);
                    flag = false;
                }
            }
        }
        
        public static void Main(string[] args)
        {
            Task.Factory.StartNew(()=> keyPressCatcher(ConsoleKey.W, out isPressed_W));
            Task.Factory.StartNew(()=> keyPressCatcher(ConsoleKey.A, out isPressed_A));
            Task.Factory.StartNew(()=> keyPressCatcher(ConsoleKey.S, out isPressed_S));
            Task.Factory.StartNew(()=> keyPressCatcher(ConsoleKey.D, out isPressed_D));
            Task.Factory.StartNew(()=> keyPressCatcher(ConsoleKey.Q, out isPressed_Q));
            Task.Factory.StartNew(()=> keyPressCatcher(ConsoleKey.E, out isPressed_E));
            
            loadMap("maps/map1.txt");

            rnd = new Random();
            double rndD = rnd.NextDouble();
            PIXELS = W * H;
            Console.CursorVisible = false;
            double cameraX = -5;
            double cameraY = 0;
            double cameraRotateZ = 0;
            vec3 spherePos = new vec3(0, 0, 0);
            double sphereR = 1.5d;
            
            vec3 cameraDirection = vec3.One;
            
            while (true)
            {
                DateTime startFrame = DateTime.Now;
                char[] screen = new char[PIXELS];
                int curPos = 0;
                vec3 light = vec.norm(new vec3(-0.6, 1/*Math.Sin(deltaT * rndD) * 2*/, -1.0));
                double FOV = 0.9;
                
                cameraHandler(ref cameraX, ref cameraY, ref cameraRotateZ);

                render(cameraX, cameraY, FOV, cameraRotateZ, spherePos, sphereR, light, screen);

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

        private static void render(double cameraX, double cameraY, double FOV, double cameraRotateZ, vec3 spherePos,
            double sphereR, vec3 light, char[] screen)
        {
            int curPos;
            vec3 cameraDirection;
            for (int i = 0; i < W; i++)
            {
                for (int j = 0; j < H; j++)
                {
                    curPos = i + j * W;

                    vec2 uv = new vec2(i, j) / new vec2(W, H) - new vec2(0.5, 0.5);
                    uv.x *= RATIO;

                    vec3 cameraOrigin = new vec3(cameraX, cameraY, 0);
                    cameraDirection = vec.norm(new vec3(FOV, uv));
                    vec.rot(cameraRotateZ, ref cameraDirection);
                    //cameraDirection.y *= Math.Sin(deltaT * rndD) * 2;

                    renderSphere(spherePos, sphereR, light, screen, cameraOrigin, cameraDirection, curPos);
                }
            }
        }

        private static void renderSphere(vec3 spherePos, double sphereR, vec3 light, char[] screen, vec3 cameraOrigin,
            vec3 cameraDirection, int curPos)
        {
            vec2 it = vec.sphIntersect(cameraOrigin, cameraDirection, spherePos, sphereR);
            vec3 oneIfIntersect = vec.castRay(cameraOrigin, cameraDirection, it, light);
            int color = (int)(vec.avg(oneIfIntersect) * gMaxIndex);

            screen[curPos] = ASCII_GRADIENT[color > gMaxIndex ? gMaxIndex : color <= 0 ? 0 : color];
        }

        private static void loadMap(string mapResource)
        {
            foreach (String mapLine in File.ReadAllLines(mapResource))
            {
                mapChar.AddRange(mapLine.ToCharArray());
                mapLines.Add(mapLine);
            }
        }

        private static void cameraHandler(ref double cameraX, ref double cameraY, ref double cameraRotateZ)
        {
            /*cameraY = Math.Sin(deltaT) / 2;*/
            if (isPressed_D)
                cameraY += 0.2;
            if (isPressed_A)
                cameraY -= 0.2;
            if (isPressed_W)
                cameraX += 0.2;
            if (isPressed_S)
                cameraX -= 0.2;
            if (isPressed_Q)
                cameraRotateZ += 0.02;
            if (isPressed_E)
                cameraRotateZ -= 0.02;
        }

        private static void writeMap(char[] screen)
        {
            writeLinesOnScreen(screen, mapLines, 1, 1);
        }

        private static void writeInfo(char[] screen)
        {
            if (!INFO_ENABLE) return;
            string infoText = $"FPS {FPS} ({(1000 / lastFrameTime.TotalMilliseconds):F0}) + PRESSED: W:{isPressed_W}, A:{isPressed_A}, S: {isPressed_S}, D:{isPressed_D}";
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
