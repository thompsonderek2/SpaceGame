using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Engine
{
    public class Explosion : Sprite
    {
        public static List<Explosion> Explosions = new List<Explosion>();
        public static List<PictureBox> ExplosionImage = new List<PictureBox>();

        protected static int ctr;

        private int DisplayCounter { get; set; }

        public Explosion(int x_position, int y_position, int time, int width, int height, Image image, Form form)
        {
            ID = ctr;
            PosX = x_position;
            PosY = y_position;
            Width = width;
            Height = height;
            DisplayCounter = time;
            Hit = false;
            ctr += 1;

            NewExplosion(image, form);
        }

        /* creating the explosion animation is very similar to the enemy and missile sprites. the passed in parameters are the 
           x and y coordinates of the destroyed enemy */
        private void NewExplosion(Image image, Form form)
        {
            Explosions.Add(this);
            PictureBox NewExplosionImage = new PictureBox();
            NewExplosionImage.Location = new Point(this.PosX, this.PosY);
            NewExplosionImage.BackColor = Color.Black;
            NewExplosionImage.Size = new Size(this.Width, this.Height);

            NewExplosionImage.Name = this.ID.ToString();
            NewExplosionImage.Image = image;//global::SpaceGame2.Properties.Resources.explosion;
            ExplosionImage.Add(NewExplosionImage);

            // Adds the NewExplosionImage picturebox to the form
            form.Controls.Add(NewExplosionImage);
        }

        // control the timing of for how long the explosion animation remains on screen
        public static void ExplosionTime(Form form)
        {
            foreach (Explosion explosion in Explosions)
            {
                explosion.DisplayCounter--;
                if (explosion.DisplayCounter <= 0)
                {
                    explosion.Hit = true;
                    foreach (PictureBox explosion_image in ExplosionImage)
                    {
                        if (explosion_image.Name == explosion.ID.ToString())
                        {
                            // remove explosion image from the form
                            form.Controls.Remove(explosion_image);
                        }
                    }
                    // release the image from the list
                    ExplosionImage.RemoveAll(x => x.Name == explosion.ID.ToString());
                }
            }
            Explosions.RemoveAll(x => x.Hit == true);
        }

        ~Explosion()
        {
            ctr--;
        }
    }
}
