using System.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SamTracer
{
    class Scene
    {
        public Traceables[] things;
        public Traceables[] lights;
    }



    class RayTracer
    {
        //public List<Color> Buffer = new List<Color>();
        int width, height;
        public Action<int, int, System.Drawing.Color> setPixel;
        public RayTracer() : this(320, 280, (int x, int y, System.Drawing.Color color) => { }) { }

        public RayTracer(int width, int height, Action<int, int, System.Drawing.Color> setPixel)
        {
            this.setPixel = setPixel;
            this.width = width;
            this.height = height;
        }

        public void castRays()
        {
            /*
            Ray[] Rays = new Ray[width*height];
            
            for(int y = 0; y<height; y++)
                for(int x = 0; x<width; x++)
                    Rays[y*width+x] = new Ray(){
                        Origin = Camera.Origin,
                        Direction = Camera.Direction + new Vec3(x/width-.5f,y/height-.5f,0)
                    };

            foreach (Ray currentRay in Rays)
            {
                var intersections = defaultScene.things
                .Select(thing => thing.intersect(currentRay))
                .Where(t => t != null)
                .OrderBy(t=>t)
                .FirstOrDefault();
            }
             */


            Ray CurrentRay, reflectedRay;


            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    //Create Ray
                    CurrentRay = new Ray()
                    {
                        Origin = Camera.Origin,
                        Direction = (new Vec3(x / ((float)width) - .5f, y / ((float)height) - .5f, 0) - Camera.Origin).Normalize()
                    };

                    //Intersect Ray
                    var intersection = defaultScene.things
                    .Select(thing => thing.intersect(CurrentRay))
                    .Where(t => t != null)
                    .OrderBy(t => t.Dist)
                    .FirstOrDefault();

                    var pixelColor = new Color(0, 0, 0);

                    var reflection = 0.0f;


                    for (int r = 0; r <= 5; r++)
                    {
                        if (intersection == null)
                            break;

                        pixelColor = pixelColor * reflection + (Color.phong(intersection, defaultScene) * (1 - reflection));
                        reflection = intersection.Thing.reflect;

                        reflectedRay = new Ray()
                        {
                            Direction = intersection.Position() - intersection.Thing.Position,
                            Origin = intersection.Position() + ((intersection.Position() - intersection.Thing.Position) * .01f)
                        };

                        intersection = defaultScene.things
                        .Select(thing => thing.intersect(reflectedRay))
                        .Where(t => t != null)
                        .OrderBy(t => t.Dist)
                        .FirstOrDefault();
                    }



                    //Color Pixel
                    //Buffer.Add(Color.phong(intersections, defaultScene) ?? new Color());
                    setPixel(x, y, Color.swap(pixelColor ?? new Color()));
                }


            return;
        }


        internal readonly Scene defaultScene = new Scene()
        {
            things = new Traceables[2]{
                new Sphere(){
                    Position = new Vec3(-.25f,0f,-1.0f),
                    radius = .5f,
                    mat = new Material(new Color(0.0f,1.0f,0.0f))},
                new Sphere(){
                    Position = new Vec3(.25f,-.25f,-.5f),
                    radius = .25f,
                    mat = new Material(new Color(0.0f,0.0f,1.0f))}
                },
            lights = new Traceables[3]{
                new Light(){
                    Position = new Vec3(-.5f,-1f,1f),
                    mat = new Material(new Color(.3f,.1f,.1f),new Color(.1f,.1f,.1f))
                },
                new Light(){
                    Position = new Vec3(.5f,-1f,1f),
                    mat = new Material(new Color(.1f,.3f,.1f),new Color(.1f,.1f,.1f))
                },
                new Light(){
                    Position = new Vec3(.5f,0f,1f),
                    mat = new Material(new Color(.1f,.1f,.3f),new Color(.1f,.1f,.1f))
                }
            }
        };

        internal readonly Ray Camera = new Ray()
        {
            Origin = new Vec3(0.0f, 0.0f, 1.0f),
            Direction = new Vec3(0.0f, 0.0f, -1.0f)
        };

    }

    class Ray
    {
        public Vec3 Origin;
        public Vec3 Direction;

        public static Vec3 GetPosition(Ray currentRay, float t)
        {
            return currentRay.Direction * t + currentRay.Origin;
        }
    }

    class ISect
    {
        public Traceables Thing;
        public Ray Ray;
        public float Dist;
        public Vec3 Position()
        {
            return Ray.Direction * Dist + Ray.Origin ?? new Vec3(0, 0, 0);
        }
    }


}
