using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine
{
    public class Sprite
    {
        public int ID { get; set; }
        // Position of top left corner
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int Speed { get; set; }
        // Dimensions of sprite hitbox
        public int Width { get; set; }
        public int Height { get; set; }
        // Hit flag to indicate projectile has hit target
        public bool Hit { get; set; }

    }
}
