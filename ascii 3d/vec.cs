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
        public static double length(vec3 v) => Math.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);

        public static vec2 norm(vec2 v) => v / length(v);
        public static vec3 norm(vec3 v) => v / length(v);

        public static double dot(vec2 a, vec2 b) => a.x * b.x + a.y * b.y;
        public static double dot(vec3 a, vec3 b) => a.x * b.x + a.y * b.y + a.z * b.z;

        /**
         * sphere of size ra centered at point ce.
         * ro - camera
         * rd - camera target 
         * ce - sphere position
         * ra - sphere radius
         */
        public static vec2 sphIntersect(vec3 ro, vec3 rd, vec3 ce, double ra)
        {
            vec3 oc = ro - ce;
            double b = dot(oc, rd);
            double c = dot(oc, oc) - ra * ra;
            double h = b * b - c;
            if (h < 0.0)
                return new vec2(-1.0); // no intersection
            h = Math.Sqrt(h);

            return new vec2(-b - h, -b + h);
        }

        public static vec3 castRay(vec3 ro, vec3 rd, vec2 it, vec3 light)
        {
            if (it.x < 0d) return vec3.Zero;

            vec3 itPos = ro + rd * it.x;
            vec3 n = itPos;
            double dotLight = dot(n, light);
            double diffuse = Math.Max(0d, dotLight) * .5d;
            vec3 reflected = rd - n * 2d * dot(n, rd);
            double specular = Math.Pow(Math.Max(0d, dot(reflected, light)), 16);

            return new vec3(diffuse + specular);
        }

        public static vec3 revert(vec3 v) => new vec3(-v.x, -v.y, -v.z);

        public static double avg(vec3 v) => (v.x + v.y + v.z) / 3;
    }
}
