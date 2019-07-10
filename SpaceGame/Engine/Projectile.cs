using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Engine
{
    public class Projectile : Sprite
    {
        public static List<Projectile> Projectiles = new List<Projectile>();
        public static List<PictureBox> ProjectileImage = new List<PictureBox>();

        static int Speed { get; set; }

        private static int ctr = 0;

        public Projectile(int x_position, int y_position, int speed, Form form)
        {
            ID = ctr;
            PosX = x_position;
            PosY = y_position;
            Speed = speed;
            Hit = false;
            ctr += 1;

            MakeNewProjectile(form);
        }

        private void MakeNewProjectile(Form form)
        {
            Projectiles.Add(this);
            PictureBox NewProjectileImage = new PictureBox();
            NewProjectileImage.Location = new Point(this.PosX, this.PosY);
            NewProjectileImage.BackColor = Color.Orange;
            NewProjectileImage.Size = new Size(5, 10);

            /* Naming the picturebox is only necessary because that is required for adding controls to a form at runtime, chose projectile ID because unique
            better way would be to use a random number since there is a small chance of number repeat.however, because the garbage collector runs at random,
            reseting the object counter, the chance is pretty small. */
            NewProjectileImage.Name = this.ID.ToString();
            ProjectileImage.Add(NewProjectileImage);

            // Adds the missile picturebox to the form
            form.Controls.Add(NewProjectileImage);
        }

        /* I needed to ensure that the visible image and the invisible sprite object stayed aligned while moving across the screen */
        public static void MoveProjectile(Form form)
        {
            /* move the position of each missile upwards
            using a foreach to change the position of each projectile */
            foreach (Projectile projectile in Projectiles)
            {
                projectile.PosY -= Speed;
            }
            foreach (PictureBox projectile_image in ProjectileImage)
            {
                /* The missile image needs to be removed from the form AND the list
                the list is needed to sysematically increment the position of the image */
                projectile_image.Top -= Speed;//Projectiles.Find(x => x.ID.ToString() == projectile_image.Name).Speed;
                if (projectile_image.Top < 10)
                {
                    form.Controls.Remove(projectile_image);
                }
            }
            ProjectileImage.RemoveAll(x => x.Top < 10);
            Projectiles.RemoveAll(x => x.PosY < 10);
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
