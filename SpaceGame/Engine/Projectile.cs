using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Projectile : Sprite
    {
        protected static int ctr = 0;

        public Projectile(int x_position, int y_position, int speed)
        {
            ID = ctr;
            PosX = x_position;
            PosY = y_position;
            Speed = speed;
            Hit = false;
            ctr += 1;
        }

        ~Projectile()
        {
            ctr--;
        }

        public static string ShowCount()
        {
            return ctr.ToString();
        }

    }
}
