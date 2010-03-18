using System;
using System.Collections.Generic;
using System.Text;

namespace SamTracer
{
    class Traceables
    {
        public Vec3 Position = new Vec3();
        public Material mat = new Material();
        public float reflect = 0.8f;
        public virtual ISect intersect(Ray currentRay)
        {
            return null;
        }

        public virtual Color phong(Ray currentRay, float t){
            return new Color(0,0,0);
        }
    }

    class Sphere : Traceables
    {
        public float radius;
        public override ISect intersect(Ray currentRay)
        {
            ISect record = null;
            float a = currentRay.Direction.Dot(currentRay.Direction);
            float b = (currentRay.Origin - Position).Dot(currentRay.Direction) * 2.0f;
            float c = (currentRay.Origin - Position).Dot(currentRay.Origin - Position) - (float)Math.Pow(radius,2.0);

            float det = (float)Math.Pow(b, 2) - 4 * a * c;
            if (det < 0)
                return null;

            float t1 = (-b + (float)Math.Sqrt(det)) / (2 * a);
            float t2 = (-b - (float)Math.Sqrt(det)) / (2 * a);

            if (t1 < 0 && t2 < 0)
                return record;

            record = new ISect();
            record.Thing = this;
            record.Ray = currentRay;

            if (t1 < 0)
                record.Dist = t2;
            else if (t2 < 0)
                record.Dist = t1;
            else
                record.Dist = Math.Min(t1, t2);
            
            return record;
        }

        public override Color phong(Ray currentRay, float t)
        {
            return base.phong(currentRay, t);
        }
    }

    class Light : Traceables{
        static readonly Color ambiant = new Color(25, 25, 25);
    }

    
}
