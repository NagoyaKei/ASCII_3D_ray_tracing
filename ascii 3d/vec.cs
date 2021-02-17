using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ascii_3d
{
    public class vec
    {
        public static double length(vec2 v) => Math.Sqrt(v.x * v.x + v.y * v.y);
        public static double length(vec3 v) => Math.Sqrt(v.x * v.x + v.y * v.y + v.z);

        public static vec2 norm(vec2 v) => v / length(v);
        public static vec3 norm(vec3 v) => v / length(v);

        public static double dot(vec2 a, vec2 b) => a.x * b.x + a.y * b.y;
        public static double dot(vec3 a, vec3 b) => a.x * b.x + a.y * b.y + a.z + b.z;
    }
}
