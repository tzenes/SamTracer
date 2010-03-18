using System;
using System.Collections.Generic;
using System.Text;

namespace SamTracer
{
    class Vec3
    {
        public float x, y, z;

        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vec3() : this(0, 0, 0) { }
        public Vec3(Vec3 init) : this(init.x, init.y, init.z) { }

        public float Dot(Vec3 right)
        {
            return this.x * right.x + this.y * right.y + this.z * right.z;            
        }

        public Vec3 Cross(Vec3 that)
        {
            return new Vec3(this.y * that.z - this.z * that.y,
                this.z * that.x - this.x * that.z,
                this.x * that.y - this.y * that.x);
        }

        public static Vec3 operator +(Vec3 left, Vec3 right)
        {
            return new Vec3(left.x + right.x, left.y + right.y, left.z + right.z);
        }

        public static Vec3 operator -(Vec3 left, Vec3 right)
        {
            return left + Negative(right);
        }

        public static Vec3 operator *(Vec3 left, float right)
        {
            return new Vec3(left.x * right, left.y * right, left.z * right);
        }

        public static Vec3 operator *(Vec3 left, Vec3 right)
        {
            return new Vec3(left.x * right.x, left.y * right.y, left.z * right.z);
        }

        public static Vec3 Negative(Vec3 input)
        {
            return new Vec3(-input.x,-input.y,-input.z);
        }

        public Vec3 Normalize()
        {
            float biggest = (float)Math.Sqrt(x*x+y*y+z*z);
            this.x /= biggest;
            this.y /= biggest;
            this.z /= biggest;
            return this;
        }
        
    }
}


