using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine
{
    public class Projectile : Sprite
    {
        public static List<Projectile> Missile = new List<Projectile>();
        public static List<PictureBox> MissileImage = new List<PictureBox>();


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
