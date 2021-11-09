using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BlazorChatApp.Shared;

namespace BlazorChatApp.Client.Model
{
    public class Ball
    {
        public string Id{get;set; }
        public string Name{get;set; }
        public string ChatMessage{get;set; }

        private int ChatViewTime {get;set; }

        public double X { get; private set; }
        public double Y { get; private set; }
        public double XVel { get; private set; }
        public double YVel { get; private set; }
        public double Radius { get; private set; }
        public string Color { get; private set; }

        public double TargetX { get; private set; }
        public double TargetY { get; private set; }

        public Ball(string id, string name, double x, double y, double xVel, double yVel, double radius, string color)
        {
            (Id, Name, X, Y, XVel, YVel, Radius, Color) = (id, name, x, y, xVel, yVel, radius, color);

            TargetX = x;
            TargetY = y;
        }

        public void MoveForward(double _Xvel,double _Yvel)
        {
            TargetX = X+_Xvel;
            TargetY = Y+_Yvel;
        }

        public void AddChatMessage(ChatMessage chatMessage)
        {
            ChatMessage = chatMessage.Message;
            ChatViewTime = 1000;
        }
        
        public void StepForward()
        {
            if( TargetX-X > 1)
            {
                X+=XVel;
            }
            else if( X-TargetX > 1)
            {
                X-=XVel;
            }

            if( TargetY-Y > 1)
            {
                Y+=YVel;
            }
            else if( Y-TargetY > 1)
            {
                Y-=YVel;
            }

            if(!string.IsNullOrEmpty(ChatMessage))
            {
                ChatViewTime--;
                if( 0 > ChatViewTime)
                {
                    ChatMessage=string.Empty;
                }
            }
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
