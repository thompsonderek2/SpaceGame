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

        /* called in the constructor, adds the new enemy object to the static class list and adds a corresponding image 
           (sharing the same coordinates on the screen) to the images list. This was done so that the player has a visual 
           in the Windows Forms graphics platform that represents the object in real time. 
           i.e: the hitbox of the object relates to the dimensions of the image.

           Arguments are:
           Image image is a global resource, such as a .png, .jpg
           Form form is the windows form class object in which the enemy constructor is called */
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

        /* move each enemy sprite in the vertical direction.
           the method is static so that it can manipulate each item in the static class lists for the enemy objects
           and the enemy images.
           
           Arguments:
           bool game_over - flag indicating end of game
           ref int missctr - keep track of each miss, passed by reference
           ref int scorectr - score tracker, passed by reference
           Form form - the Windows Form class object containing the enemy class instance */
           // form does not need to be passed by reference because the object is calling the method on
           // itself, whereas the int vars are just fields.
        public static void MoveEnemy( bool game_over, ref int missctr, ref int scorectr, Form form)
        {
            // change the position of each enemy object
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
            Enemies.RemoveAll(x => x.PosY > (form.Height - 10));
            //Enemies.RemoveAll(x => x.Hit == true);

        }

        /* made this function to delete all enemies from the screen after the game ends */
        public static void ClearLists(Form form)
        {
            Enemies.Clear();
            foreach (PictureBox enemy_image in EnemyImage)
            {
                // remove enemy image from the form
                form.Controls.Remove(enemy_image);
                // let the item from the image list get removed when it reaches the top of the window
            }
            EnemyImage.Clear();
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
