using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Engine
{
    public class Enemy : Sprite
    {
        public static List<Enemy> Enemies = new List<Enemy>();
        public static List<PictureBox> EnemyImage = new List<PictureBox>();

        protected static int ctr = 0;
        //public static int StartPosX { get; set; }
        //public static int StartPosY { get; set; }


        public Enemy(int x_position, int y_position, int speed, int width, int height, Form form, Image image)
        {
            ID = ctr;
            PosX = x_position;
            PosY = y_position;
            Width = width;
            Height = height;
            Speed = speed;
            ctr += 1;
            Hit = false;

            MakeNewEnemy(image, form);
        }

        /* creating the enemy sprite is similar to the creation of a missile sprite. I decided to use a random x coordinate
        based on the players image width to ensure that the enemy is shootable when it is generated */
        private void MakeNewEnemy(Image image, Form form)
        {
            //NewEnemy = new Enemy(RandomNumber(PlayerImage.Width / 2, Width - PlayerImage.Width), 0, 5, 100, 90);
            Enemies.Add(this);

            PictureBox NewEnemyImage = new PictureBox();
            NewEnemyImage.Location = new Point(this.PosX, this.PosY);
            NewEnemyImage.BackColor = Color.Black;
            NewEnemyImage.Size = new Size(this.Width, this.Height);

            NewEnemyImage.Name = this.ID.ToString();
            NewEnemyImage.Image = image;//global::SpaceGame2.Properties.Resources.enemy_ship;
            EnemyImage.Add(NewEnemyImage);

            // Adds the NewEnemyImage picturebox to the form
            form.Controls.Add(NewEnemyImage);
        }

        // move the enemy sprite
        // the method is static so that it can manipulate each item in the static class lists for the enemy object
        // and the enemy images
        public static void MoveEnemy( bool game_over, int missctr, int scorectr, Form form)
        {
            // move the position of each enemy downwards 
            // using a foreach to change the position of each projectile
            foreach (Enemy enemy in Enemies)
            {
                enemy.PosY += enemy.Speed;

                // increment miss counter
                if (enemy.PosY > (form.Height - 10) && game_over == false)
                {
                    missctr += 1;
                    // subtract 5 points for enemy missed
                    scorectr -= 5;
                }
            }
            Enemies.RemoveAll(x => x.PosY > (form.Height - 10));
            foreach (PictureBox enemy_image in EnemyImage)
            {
              /* The enemy image needs to be removed from the form AND the list
                 the list is needed to sysematically increment the position of the image
                 corresponding to the object of the same ID in the object list.
                 basically the Name of the enemy image should be the same as the ID.ToString() of the corresponding
                 enemy object in the object list. so if we can find it in the list we can incremet the image by the 
                 value of the speed property for that enemy object */

                enemy_image.Top += Enemies.Find(x => x.ID.ToString() == enemy_image.Name).Speed;//enemy_speed;
                if (enemy_image.Top > (form.Height - 10))
                {
                    form.Controls.Remove(enemy_image);
                }
            }
            EnemyImage.RemoveAll(x => x.Top > (form.Height - 10));
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
