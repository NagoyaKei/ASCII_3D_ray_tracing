namespace ascii_3d
{
    public struct vec3
    {
        public double x;
        public double y;
        public double z;
        public static vec3 One => new vec3(1d);
        public static vec3 Zero => new vec3(0d);

        public vec3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public vec3(double val) : this(val, val, val) { }
        public vec3(vec2 v, double z) : this(v.x, v.y, z) { }
        public vec3(double x, vec2 v) : this(x, v.x, v.y) { }

        public static vec3 operator /(vec3 a, vec3 b) => new vec3(a.x / b.x, a.y / b.y, a.z / b.z);
        public static vec3 operator /(vec3 a, double b) => new vec3(a.x / b, a.y / b, a.z / b);
        public static vec3 operator *(vec3 a, vec3 b) => new vec3(a.x * b.x, a.y * b.y, a.z * b.z);
        public static vec3 operator *(vec3 a, double b) => new vec3(a.x * b, a.y * b, a.z * b);
        public static vec3 operator +(vec3 a, vec3 b) => new vec3(a.x + b.x, a.y + b.y, a.z + b.z);
        public static vec3 operator +(vec3 a, double b) => new vec3(a.x + b, a.y + b, a.z + b);
        public static vec3 operator -(vec3 a, vec3 b) => new vec3(a.x - b.x, a.y - b.y, a.z - b.z);
        public static vec3 operator -(vec3 a, double b) => new vec3(a.x - b, a.y - b, a.z - b);
        
        public override string ToString()
        {
            return x + "; " + y + "; " + z;
        }
    }
}
