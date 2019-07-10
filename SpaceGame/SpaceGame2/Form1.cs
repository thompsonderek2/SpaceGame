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

            hitctr = missctr = scorectr = 0;
            enemy_speed = 10;
            game_over = false;

            InitializePlayer();
            this.Controls.Add(Player.PlayerImage);


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
            Player.PlayerObj.MovePlayer(move_right, move_left, player_speed, this);
            
        }

        // Tick for moving missiles
        private void Missiletimer_Tick(object sender, EventArgs e)
        {
            Projectile.MoveProjectile(missile_speed, this);

            // handle weapon cooldown rate
            if (weapon_delay_ctr == 10)
            {
                // Fire weapon 
                if (fire_weapon == true && game_over == false)
                {
                    new Projectile((Player.PlayerImage.Left + Player.PlayerImage.Width / 2), Player.PlayerImage.Top, missile_speed, this);
                    
                    // subtract one from score for each missile shot
                    scorectr--;
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

            Enemy.MoveEnemy(game_over, ref missctr, ref scorectr, this);

            if (enemy_delay_ctr >= 10 && game_over == false)
            {
                // increase speed of enemies as game progresses
                if(hitctr%10 == 0 && 0 < hitctr && hitctr < 120)
                {
                    enemy_speed += 10;
                }
                new Enemy(RandomNumber(Player.PlayerImage.Width / 2, Width - Player.PlayerImage.Width), 0, enemy_speed, 100, 90, this, global::SpaceGame2.Properties.Resources.enemy_ship);
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
            Explosion.ExplosionTime(this);
        }

// CLASS METHODS

        // create the player sprite
        private void InitializePlayer()
        {
            // construct the player object
            new Player(this.Width / 2 - 50, this.Height - 140, player_speed, 100, 100, global::SpaceGame2.Properties.Resources.player_ship, this);
            // set up event handler for weapon customizability (NOT USED)
            Player.PlayerImage.Click += new System.EventHandler(this.PlayerImage_Click);
        }

        // ends the game when an enemy hits the player
        private void EnemyImpact()
        {
            foreach(Enemy enemy in Enemy.Enemies)
            {
                if ((enemy.PosY+50) > Player.PlayerObj.PosY && (enemy.PosX + enemy.Width) > Player.PlayerObj.PosX && enemy.PosX < (Player.PlayerObj.PosX + Player.PlayerObj.Width))
                {
                    new Explosion(Player.PlayerObj.PosX - 50, Player.PlayerObj.PosY - 50, 50, 200, 200, global::SpaceGame2.Properties.Resources.explosion, this);
                    this.Controls.Remove(Player.PlayerImage);

                    enemy.Hit = true;
                    foreach (PictureBox enemy_image in Enemy.EnemyImage)
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
            foreach (Projectile missile in Projectile.Projectiles)
            {
                foreach (Enemy enemy in Enemy.Enemies)
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
                        foreach (PictureBox missile_image in Projectile.ProjectileImage)
                        {
                            if (missile_image.Name == missile.ID.ToString())
                            {
                                // remove missile image from the form
                                this.Controls.Remove(missile_image);
                                // let the item from the image list get removed when it reaches the top of the window
                            }
                        }
                        foreach (PictureBox enemy_image in Enemy.EnemyImage)
                        {
                            if (enemy_image.Name == enemy.ID.ToString())
                            {
                                // replace enemy image with explosion animation
                                // set counter for ticks to display explosion
                                new Explosion(enemy_image.Left-50, enemy_image.Top-50, 50, 200, 200, global::SpaceGame2.Properties.Resources.explosion, this);


                                // remove enemy image from the form
                                this.Controls.Remove(enemy_image);
                                
                            }
                        }
                        // remove the image based on the matching coordinates of the object in the list
                        // action must go within these brackets or null pointer exception occurs
                        Enemy.EnemyImage.RemoveAll(x => x.Top == Enemy.Enemies.Find(y => y.Hit == true).PosY);
                        Projectile.ProjectileImage.RemoveAll(x => x.Top == Projectile.Projectiles.Find(y => y.Hit == true).PosY);
                    }
                }

            }
            Enemy.Enemies.RemoveAll(x => x.Hit == true);
            Projectile.Projectiles.RemoveAll(x => x.Hit == true);
        }

        // If the player and enemy collide
        private void GameOver()
        {
            this.Controls.Add(this.start);
            Enemy.ClearLists(this);
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