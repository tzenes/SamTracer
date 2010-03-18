using System;
using System.Collections.Generic;
using System.Text;

namespace SamTracer
{
    class Color
    {
        public float r, g, b;

        public Color() : this(0, 0, 0) { }

        public Color(Color init) : this(init.r, init.g, init.b) { }

        public Color(float r, float g, float b)
        {
            this.r = r;
            this.b = b;
            this.g = g;

        }

        public static Color operator +(Color left, Color right)
        {
            return new Color(left.r + right.r, left.g + right.g, left.b + right.b);
        }

        public static Color operator -(Color left, Color right)
        {
            return left + Negative(right);
        }

        public static Color operator *(Color left, float right)
        {
            return new Color((left.r * right), (left.g * right), (left.b * right));
        }

        public static Color operator *(Color left, Color right)
        {
            return new Color(left.r * right.r, left.g * right.g, left.b * right.b);
        }

        public static Color Negative(Color input)
        {
            input.r = -input.r;
            input.g = -input.g;
            input.b = -input.b;
            return input;
        }

        public static Color phong(ISect record, Scene scene)
        {
            if (record == null) return null;

            Color ambiant = new Color();
            Color diffuse = new Color();
            Color specular = new Color();
            Vec3 position = Ray.GetPosition(record.Ray, record.Dist);
            int i;


            foreach (Light light in scene.lights)
            {
                ambiant += record.Thing.mat.Ambiant * light.mat.Ambiant;
                var N = (position - record.Thing.Position).Normalize();
                var L = (light.Position - position).Normalize();
                var NDotL = N.Dot(L);
               
                if (NDotL < 0) continue;
                diffuse += record.Thing.mat.Diffuse * light.mat.Diffuse * NDotL;

                var R = N * 2 * NDotL - L;
                var V = (light.Position - position).Normalize();
                var RdotV = R.Dot(V);
                if (RdotV < 0) continue;
                specular += record.Thing.mat.Specular * light.mat.Specular * (float)Math.Pow(RdotV, record.Thing.mat.shinyness);
            }
            return Clamp(ambiant + diffuse + specular);
        }

        public static Color Clamp(Color unclamped)
        {
            return new Color(unclamped.r > 1.0f ? 1.0f : unclamped.r < 0.0f ? 0.0f : unclamped.r,
                                unclamped.g > 1.0f ? 1.0f : unclamped.g < 0.0f ? 0.0f : unclamped.g,
                                unclamped.b > 1.0f ? 1.0f : unclamped.b < 0.0f ? 0.0f : unclamped.b);
        }

        public static System.Drawing.Color swap(Color input)
        {
            return System.Drawing.Color.FromArgb((int)(input.r * 255), (int)(input.g * 255), (int)(input.b * 255));
        }
    }

    class Material
    {
        public Color Specular;
        public Color Diffuse;
        public Color Ambiant;
        public float shinyness = 4.0f;

        public Material(Color Ambiant, Color Diffuse, Color Specular)
        {
            this.Ambiant = Ambiant;
            this.Diffuse = Diffuse;
            this.Specular = Specular;
        }

        public Material(Color myColor) : this(myColor, myColor, new Color(.9f, .9f, .9f)) { }
        public Material(Color myColor, Color ambiant) : this(ambiant, myColor, myColor) { }
        public Material() : this(new Color(1.0f, 0, 0)) { }
    }
}


