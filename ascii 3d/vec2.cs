namespace ascii_3d
{
    public struct vec2
    {
        public double x;
        public double y;

        public vec2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public vec2(double val) : this(val, val) { }

        public static vec2 operator /(vec2 a, vec2 b) => new vec2(a.x / b.x, a.y / b.y);
        public static vec2 operator /(vec2 a, double b) => new vec2(a.x / b, a.y / b);
        public static vec2 operator *(vec2 a, vec2 b) => new vec2(a.x * b.x, a.y * b.y);
        public static vec2 operator *(vec2 a, double b) => new vec2(a.x * b, a.y * b);
        public static vec2 operator +(vec2 a, vec2 b) => new vec2(a.x + b.x, a.y + b.y);
        public static vec2 operator +(vec2 a, double b) => new vec2(a.x + b, a.y + b);
        public static vec2 operator -(vec2 a, vec2 b) => new vec2(a.x - b.x, a.y - b.y);
        public static vec2 operator -(vec2 a, double b) => new vec2(a.x - b, a.y - b);

        public override string ToString()
        {
            return x + "; " + y;
        }
    }
}
