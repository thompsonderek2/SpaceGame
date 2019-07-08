using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine
{
    public class Player : Sprite
    {
        static PictureBox PlayerImage { get; set; }

        //public int ID { get; set; }
        //// Position of top left corner
        //public int PosX { get; set; }
        //public int PosY { get; set; }
        //public int Speed { get; set; }
        //// Dimensions of sprite hitbox
        //public int Width { get; set; }
        //public int Height { get; set; }
        //// Hit flag to indicate projectile has hit target
        //public bool Hit { get; set; }

        //public void SetPosition(int x, int y)
        //{
        //    PosX = x;
        //    PosY = y;
        //}

        public Player(int x_position, int y_position, int speed, int width, int height)
        {
            PosX = x_position;
            PosY = y_position;
            Width = width;
            Height = height;
            Speed = speed;
            Hit = false;
        }
    }
}
