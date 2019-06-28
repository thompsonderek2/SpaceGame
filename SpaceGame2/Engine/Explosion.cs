using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Explosion : Sprite
    {
        protected static int ctr;

        public int DisplayCounter { get; set; }

        public Explosion(int x_position, int y_position, int time, int width, int height)
        {
            ID = ctr;
            PosX = x_position;
            PosY = y_position;
            Width = width;
            Height = height;
            DisplayCounter = time;
            Hit = false;
            ctr += 1;
        }

        ~Explosion()
        {
            ctr--;
        }
    }
}
