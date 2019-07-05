using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Enemy : Sprite
    {
        protected static int ctr = 0;
        //public static int StartPosX { get; set; }
        //public static int StartPosY { get; set; }


        public Enemy(int x_position, int y_position, int speed, int width, int height)
        {
            ID = ctr;
            PosX = x_position;
            PosY = y_position;
            Width = width;
            Height = height;
            Speed = speed;
            ctr += 1;
            Hit = false;
        }

        ~Enemy()
        {
            ctr--;
        }

        public static string ShowCount()
        {
            return ctr.ToString();
        }
    }
}
