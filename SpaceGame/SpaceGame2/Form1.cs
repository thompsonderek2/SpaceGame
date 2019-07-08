using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;

namespace SpaceGame2
{
    public partial class Form1 : Form
    {
        // flags for direction of movement for the player sprite
        bool move_right = false;
        bool move_left = false;
        // flags for weapon firing rate
        bool fire_weapon = false;
        bool cool_down = false;
        // end game flag
        bool game_over = false;
        // counter fields and fields for sprite movement speeds
        int weapon_delay_ctr;
        int enemy_delay_ctr;
        int player_speed = 10;
        int enemy_speed;
        int missile_speed = 10;
        // counter fields for the displayed score
        int hitctr = 0;
        int missctr = 0;
        int scorectr = 0;


        // List to keep track of Projectile objects and images
        List<Projectile> Missile = new List<Projectile>();
        List<PictureBox> MissileImage = new List<PictureBox>();
        PictureBox NewMissileImage;
        Projectile NewMissile;

        // List to keep track of enemies and/or enemy projectiles
        List<Enemy> Enemies = new List<Enemy>();
        List<PictureBox> EnemyImage = new List<PictureBox>();
        PictureBox NewEnemyImage;
        Enemy NewEnemy;

        // List to keep track of explosion animations
        List<Explosion> Explosions = new List<Explosion>();
        List<PictureBox> ExplosionImage = new List<PictureBox>();
        PictureBox NewExplosionImage;
        Explosion NewExplosion;

        // Intitialize a new player object
        PictureBox PlayerImage;
        Player Player;

        public Form1()
        {
            InitializeComponent();

            //Setup events that listens on keypress
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;

        }

// CLASS EVENT HANDLERS

        // Allows user to start the game by clicking the start button
        private void start_Click(object sender, EventArgs e)
        {
            TimersEnable();
            this.Controls.Remove(start);
            this.Controls.Remove(label4);
            //start.Hide();
            //label4.Hide();
            hitctr = missctr = scorectr = 0;
            enemy_speed = 10;
            game_over = false;

            InitializePlayer();
            this.Controls.Add(PlayerImage);


        }

        // Handle the KeyDown event to print the type of character entered into the control.
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D)
            {
                move_right = true;
            }
            else if (e.KeyCode == Keys.A)
            {
                move_left = true;
            }
            else if (e.KeyCode == Keys.W && cool_down == false)
            {
                fire_weapon = true;
                cool_down = true;
            }
        }

        // Handle the KeyUp event to print the type of character entered into the control.
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.A) || (e.KeyCode == Keys.D))
            {
                move_right = false;
                move_left = false;
            }
            else if (e.KeyCode == Keys.W)
            {
                fire_weapon = false;
            }
        }

        // Tick for moving player sprite
        private void Timer1_Tick(object sender, EventArgs e)
        {
            // update score
            label1.Text = "Score: " + scorectr;

            // move the player sprite
            MovePlayer();
        }

        // Tick for moving missiles
        private void Missiletimer_Tick(object sender, EventArgs e)
        {
            MoveMissile();

            // handle weapon cooldown rate
            if (weapon_delay_ctr == 10)
            {
                // Fire weapon 
                if (fire_weapon == true && game_over == false)
                {
                    MakeNewMissile();
                }
                cool_down = false;
                weapon_delay_ctr = 0;
            }
            else
            {
                weapon_delay_ctr++;
            }
        }

        /*This event handler controls the movement of enemy sprites as well as the rate at which 
        new Enemy sprites are generated. Updates the "miss" and "hit" score counters*/

        private void Enemytimer_Tick(object sender, EventArgs e)
        {
            label2.Text = "Miss: "+missctr.ToString();
            label3.Text = "Hits: " + hitctr.ToString();

            MoveEnemy();

            if (enemy_delay_ctr >= 10 && game_over == false)
            {
                // increase speed of enemies as game progresses
                if(hitctr%10 == 0 && 0 < hitctr && hitctr < 120)
                {
                    enemy_speed += 10;
                }
                MakeNewEnemy();
                enemy_delay_ctr = 0;
            }
            else
            {
                enemy_delay_ctr++;
            }
        }

        /*This event handler checks if a missile has impacted an enemy*/

        private void Impacttimer_Tick(object sender, EventArgs e)
        {
            MissileImpact();
            EnemyImpact();
        }

        /*event handler for explosion animation*/

        private void ExplosionTimer_Tick(object sender, EventArgs e)
        {
            ExplosionTime();
        }

// CLASS METHODS

        /* Produce a new missile object and image (picturebox object) and add it to the respective lists. In order to ensure that the 
         visible image on the form matches up with the parameters of the object I initialized the top left corners to the same pixel coordinates*/
        private void MakeNewMissile()
        {
            NewMissile = new Projectile((PlayerImage.Left + PlayerImage.Width / 2), PlayerImage.Top, missile_speed);
            Missile.Add(NewMissile);
            NewMissileImage = new PictureBox();
            NewMissileImage.Location = new Point(NewMissile.PosX, NewMissile.PosY);
            NewMissileImage.BackColor = Color.Orange;
            NewMissileImage.Size = new Size(5, 10);

            /* Naming the picturebox is only necessary because that is required for adding controls to a form at runtime, chose projectile ID because unique
            better way would be to use a random number since there is a small chance of number repeat.however, because the garbage collector runs at random,
            reseting the object counter, the chance is pretty small. */

           NewMissileImage.Name = NewMissile.ID.ToString();

            // Adds the missile picturebox to the form
            this.Controls.Add(NewMissileImage);

            MissileImage.Add(NewMissileImage);

            // subtract one from score for each missile shot
            scorectr--;
        }

        /* I needed to ensure that the visible image and the invisible sprite object stayed aligned while moving across the screen */
        private void MoveMissile()
        {
            /* move the position of each missile upwards
            using a foreach to change the position of each projectile */
            foreach (Projectile missile in Missile)
            {
                missile.PosY -= missile.Speed;
            }
            Missile.RemoveAll(x => x.PosY < 10);
            foreach (PictureBox missile_image in MissileImage)
            {
                /* The missile image needs to be removed from the form AND the list
                the list is needed to sysematically increment the position of the image */
                missile_image.Top -= missile_speed;
                if (missile_image.Top < 10)
                {
                    this.Controls.Remove(missile_image);
                }
            }
            MissileImage.RemoveAll(x => x.Top < 10);
        }

        /* creating the enemy sprite is similar to the creation of a missile sprite. I decided to use a random x coordinate
         based on the players image width to ensure that the enemy is shootable when it is generated */
        private void MakeNewEnemy()
        {
            NewEnemy = new Enemy(RandomNumber(PlayerImage.Width / 2, Width - PlayerImage.Width), 0, 5, 100, 90);
            Enemies.Add(NewEnemy);
            NewEnemyImage = new PictureBox();
            NewEnemyImage.Location = new Point(NewEnemy.PosX, NewEnemy.PosY);
            NewEnemyImage.BackColor = Color.Black;
            NewEnemyImage.Size = new Size(NewEnemy.Width, NewEnemy.Height);

            NewEnemyImage.Name = NewEnemy.ID.ToString();
            NewEnemyImage.Image = global::SpaceGame2.Properties.Resources.enemy_ship;

            // Adds the NewEnemyImage picturebox to the form
            this.Controls.Add(NewEnemyImage);

            EnemyImage.Add(NewEnemyImage);
        }

        /* creating the explosion animation is very similar to the enemy and missile sprites. the passed in parameters are the 
         x and y coordinates of the destroyed enemy */
        private void MakeNewExplosion(int x, int y)
        {
            NewExplosion = new Explosion(x-50, y-50, 50, 200, 200);
            Explosions.Add(NewExplosion);
            NewExplosionImage = new PictureBox();
            NewExplosionImage.Location = new Point(NewExplosion.PosX, NewExplosion.PosY);
            NewExplosionImage.BackColor = Color.Black;
            NewExplosionImage.Size = new Size(NewExplosion.Width, NewExplosion.Height);

            NewExplosionImage.Name = NewExplosion.ID.ToString();
            NewExplosionImage.Image = global::SpaceGame2.Properties.Resources.explosion;

            // Adds the NewExplosionImage picturebox to the form
            this.Controls.Add(NewExplosionImage);

            ExplosionImage.Add(NewExplosionImage);
        }

        // create the player sprite
        private void InitializePlayer()
        {
            Player = new Player(this.Width/2-50, this.Height-140, player_speed, 100, 100);
            PlayerImage = new PictureBox();
            PlayerImage.Location = new Point(Player.PosX, Player.PosY);
            PlayerImage.BackColor = Color.Black;
            PlayerImage.Size = new Size(Player.Width, Player.Height);
            PlayerImage.Image = global::SpaceGame2.Properties.Resources.player_ship;

            // set up event handler for weapon customizability (NOT USED)
            this.PlayerImage.Click += new System.EventHandler(this.PlayerImage_Click);

            // Adds the PlayerImage picturebox to the form
            this.Controls.Add(PlayerImage);
        }

        // move the player sprite
        private void MovePlayer()
        {
            if (move_right == true && (Player.PosX < (this.Width - PlayerImage.Width)))
            {
                Player.PosX += player_speed;
            }
            if (move_left == true && Player.PosX > 0)
            {
                Player.PosX -= player_speed;
            }
            PlayerImage.Left = Player.PosX;
        }

        // move the enemy sprite
        private void MoveEnemy()
        {
            // move the position of each enemy downwards 
            // using a foreach to change the position of each projectile
            foreach (Enemy enemy in Enemies)
            {
                enemy.PosY += enemy_speed;

                // increment miss counter
                if (enemy.PosY > (Height - 10) && game_over == false)
                {
                    missctr += 1;
                    // subtract 5 points for enemy missed
                    scorectr -= 5;
                }
            }
            Enemies.RemoveAll(x => x.PosY > (Height - 10));
            foreach (PictureBox enemy_image in EnemyImage)
            {
                // The enemy image needs to be removed from the form AND the list
                // the list is needed to sysematically increment the position of the image
                enemy_image.Top += enemy_speed;
                if (enemy_image.Top > (Height - 10))
                {
                    this.Controls.Remove(enemy_image);
                }
            }
            EnemyImage.RemoveAll(x => x.Top > (Height - 10));
        }

        // control the timing of for how long the explosion animation remains on screen
        private void ExplosionTime()
        {
            foreach(Explosion explosion in Explosions)
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
                            this.Controls.Remove(explosion_image);
                        }
                    }
                    // release the image from the list
                    ExplosionImage.RemoveAll(x => x.Name == explosion.ID.ToString());
                }
            }
            Explosions.RemoveAll(x => x.Hit == true);
        }

        // ends the game when an enemy hits the player
        private void EnemyImpact()
        {
            foreach(Enemy enemy in Enemies)
            {
                if ((enemy.PosY+50) > Player.PosY && (enemy.PosX + enemy.Width) > Player.PosX && enemy.PosX < (Player.PosX + Player.Width))
                {
                    //MakeNewExplosion(enemy.PosX, enemy.PosY);
                    MakeNewExplosion(Player.PosX, Player.PosY);
                    this.Controls.Remove(PlayerImage);

                    enemy.Hit = true;
                    foreach (PictureBox enemy_image in EnemyImage)
                    {
                        if (enemy_image.Name == enemy.ID.ToString())
                        {
                            // remove explosion image from the form
                            this.Controls.Remove(enemy_image);
                        }
                    }
                    game_over = true;
                }
            }
            if(game_over == true)
            {
                GameOver();
            }
        }

        private void MissileImpact()
        {
            // iteration to check for impact of missiles with enemies
            foreach (Projectile missile in Missile)
            {
                foreach (Enemy enemy in Enemies)
                {
                    // if the missile has impacted an enemy, set both hit flags to be true
                    if ((enemy.PosY + 50) > missile.PosY && missile.PosY > (enemy.PosY) && enemy.PosX < missile.PosX && missile.PosX < (enemy.PosX + enemy.Width))
                    {
                        missile.Hit = enemy.Hit = true;


                        if (enemy.Hit == true)
                        {
                            hitctr++;
                            scorectr += 20;
                        }

                        // remove the images
                        foreach (PictureBox missile_image in MissileImage)
                        {
                            if (missile_image.Name == missile.ID.ToString())
                            {
                                // remove missile image from the form
                                this.Controls.Remove(missile_image);
                                // let the item from the image list get removed when it reaches the top of the window
                            }
                        }
                        foreach (PictureBox enemy_image in EnemyImage)
                        {
                            if (enemy_image.Name == enemy.ID.ToString())
                            {
                                // replace enemy image with explosion animation
                                // set counter for ticks to display explosion
                                MakeNewExplosion(enemy_image.Left, enemy_image.Top);

                                // remove enemy image from the form
                                this.Controls.Remove(enemy_image);
                                // let the item from the image list get removed when it reaches the top of the window
                            }
                        }
                    }
                }

            }
            Enemies.RemoveAll(x => x.Hit == true);

            Missile.RemoveAll(x => x.Hit == true);
        }

        // If the player 
        private void GameOver()
        {
            this.Controls.Add(this.start);
            //start.Show();
            Enemies.Clear();
            foreach (PictureBox enemy_image in EnemyImage)
            {
                // remove enemy image from the form
                this.Controls.Remove(enemy_image);
                // let the item from the image list get removed when it reaches the top of the window
            }
            EnemyImage.Clear();
        }

        // Enables all timers
        private void TimersEnable()
        {
            timer1.Enabled = true;
            missiletimer.Enabled = true;
            enemytimer.Enabled = true;
            impacttimer.Enabled = true;
            ExplosionTimer.Enabled = true;
        }

        // Generate a random number between two numbers
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

// UNUSED EVENT HANDLERS

        private void PlayerImage_Click(object sender, EventArgs e)
        {
            // change weapon
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
             
        }


    }
}