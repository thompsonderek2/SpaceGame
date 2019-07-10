using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Engine
{
    public class Player : Sprite
    {
        // static player properties so that there is only one instance at any time
        public static PictureBox PlayerImage { get; set; }
        public static Player PlayerObj { get; set; }

        public Player(int x_position, int y_position, int speed, int width, int height, Image image, Form form)
        {
            PosX = x_position;
            PosY = y_position;
            Width = width;
            Height = height;
            Speed = speed;
            Hit = false;

            NewPlayer(image, form);
        }

        // create the player sprite
        private void NewPlayer(Image image, Form form)
        {
            PlayerObj = this;
            PlayerImage = new PictureBox();
            PlayerImage.Location = new Point(this.PosX, this.PosY);
            PlayerImage.BackColor = Color.Black;
            PlayerImage.Size = new Size(this.Width, this.Height);
            PlayerImage.Image = image;//global::SpaceGame2.Properties.Resources.player_ship;
            // Adds the PlayerImage picturebox to the form
            form.Controls.Add(PlayerImage);
        }

        // move the player sprite
        public void MovePlayer(bool move_right, bool move_left, int player_speed, Form form)
        {
            if (move_right == true && (PosX < (form.Width - PlayerImage.Width)))
            {
                PosX += player_speed;
            }
            if (move_left == true && PosX > 0)
            {
                PosX -= player_speed;
            }
            PlayerImage.Left = PosX;
        }
    }
}
