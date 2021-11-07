using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorChatApp.Client.Model
{
    public class Ball
    {
        public string Id{get;set; }
        public string Name{get;set; }
        public string ChatMessage{get;set; }

        public double X { get; private set; }
        public double Y { get; private set; }
        public double XVel { get; private set; }
        public double YVel { get; private set; }
        public double Radius { get; private set; }
        public string Color { get; private set; }

        public Ball(string id, string name, double x, double y, double xVel, double yVel, double radius, string color)
        {
            (Id, Name, X, Y, XVel, YVel, Radius, Color) = (id, name, x, y, xVel, yVel, radius, color);
        }

        public void MoveForward(double Xvel,double Yvel)
        {
            X += Xvel;
            Y += Yvel;
        }

        public void StepForward(double width, double height)
        {
            X += XVel;
            Y += YVel;
            if (X < 0 || X > width)
                XVel *= -1;
            if (Y < 0 || Y > height)
                YVel *= -1;

            if (X < 0)
                X += 0 - X;
            else if (X > width)
                X -= X - width;

            if (Y < 0)
                Y += 0 - Y;
            if (Y > height)
                Y -= Y - height;
        }
    }
}
