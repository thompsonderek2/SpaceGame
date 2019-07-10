## SpaceGame

An arcade-style space shooter game using Windows Forms for the visual effects.

![Game play](https://github.com/thompsonderek2/SpaceGame/blob/migrate_methods/SpaceGame/project_image1.png "Game play")


## Description

This project is a stand-alone application that consists of a game engine for keeping track of multiple moving sprites of different types, controlling speed, direction, updating location, and collision detection.  A graphical user interface is provided that uses the Windows Forms class libraries to provide visual effects.

## Motivation

The purpose of undertaking this project was to imporve my skill using C#, develop a greater familiarity with OOP concepts, and test problem solving skills by pushing the limits of a tool, namely, Windows Forms as a GUI for a game with many moving objects.
Mostly, this was a fun project and I enjoyed it. Maybe it can be some use to you as an example.
Surprisingly, there doesn't seem to be much demand for Windows Forms based games ;)

## Usage
Install the demo application that will be posted shortly or start your own project using the code from the engine classes.

To get started using the game engine for your own project, open a new Windows Forms Application solution in Visual Studio 2017 or newer.

Insert several timers into the form. The event handlers for these timers will be used for everything from moving the player, to incrementing the position of the enemy or missile sprites.

You will also want to add some keypress events (.KeyUp, .KeyDown) to accept user input.
### Player Class
To instantiate the Player class, simply call the constructor. Only one Player instance can exist at a time. The constructor arguments are the X and Y coordinates of the top left corner of the Player sprite, the height, width, and the form instance in which the constructor is being called.

The following example is a simple instantiation of the Player class using the constructor and moving the sprite with keypress events.
```C#
using System.Windows.Forms;
using Engine;

namespace SpaceGame2
{
    public class Form1 : Form
    {
	    /* when the button is clicked, a player appears in the 
	    lower part of the form */ 
        private void Button1_Click(object sender, EventArgs e)
        {
	        // construct the player object
            new Player(
            this.Width / 2 - 50, // X coord of left edge
            this.Height - 140, // Y coord of top edge
            player_speed, // speed (pixels per increment)
            100, // width in pixels
            100, // height in pixels
            global::SpaceGame2.Properties.Resources.player_ship, 
            this);
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
        }
        
        // Handle the KeyUp event to print the type of character entered into the control.
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.A) || (e.KeyCode == Keys.D))
            {
                move_right = false;
                move_left = false;
            }
        }
        
		// Example of moving the player sprite in a timer event handler
        private void Timer1_Tick(object sender, EventArgs e)
        {
            /* PlayerObj is the static property limiting the 
	           Player class to a single instantiation */
		    Player.PlayerObj.MovePlayer(
			    move_right, // flag indicating right movement
			    move_left, // flag indicating left movement
			    this);  // the Form in which the constructor is called
            
        }
    }
    
```

### Projectile Class
The Projectile class, like most of the other Sprite classes, contain two lists of Projectile and PictureBox instances. For each Projectile instance there is a corresponding PictureBox instance in the other list (the .ID and location of the first matches the .Name and location of the second).  The two instances together make up the sprite.

Add add a Projectile sprite to the form by calling the constructor.
```C#
// Projectile constructor
public Projectile(
	int x_position, // position of left edge
	int y_position, // position of top edge
	int speed, // speed of movement (pixels per tick)
	Form form) // the Form in which the constructor is called
```
Move the Projectile sprite by calling the .MoveProjectile function in a timer event handler.
```C#
        // Timer Handler for moving missiles
        private void Timer2_Tick(object sender, EventArgs e)
        {
	        // Pass the form as an argument in which the function is called
            Projectile.MoveProjectile(this);
            
            // Handle weapon cooldown rate
            if (weapon_delay_ctr == 10)
            {
                // Fire weapon 
                if (fire_weapon == true)
                {
	                // Creating the Projectile object
                    new Projectile(
	                    (Player.PlayerImage.Left + Player.PlayerImage.Width / 2), 
	                    Player.PlayerImage.Top,
	                    missile_speed, 
	                    this);
                }
                cool_down = false;
                weapon_delay_ctr = 0;
            }
            else
            {
                weapon_delay_ctr++;
            }
        }
```
### Enemy Class
To create an Enemy sprite, simply call the constructor.
```C#
// Enemy class constructor
public Enemy(
	int x_position, // position of left edge
	int y_position, // position of top edge
	int speed, // pixels per tick
	int width, // width of sprite
	int height, // height of sprite
	Form form, // form in which constructor is called
	Image image) // image of sprite
```
The example below demonstrates the .EnemyMovement function being called in a timer event handler.
```C#
        /* This event handler controls the movement of enemy sprites as well as the rate at which 
	       new Enemy sprites are generated. Updates the "miss" and "hit" score counters */
        private void Timer3_Tick(object sender, EventArgs e)
        {
            label2.Text = "Miss: "+missctr.ToString();
            label3.Text = "Hits: " + hitctr.ToString();

            Enemy.MoveEnemy(
	            game_over, // flag indicating end of game, if true, clears lists deleting all enemies
	            ref missctr, // int counts misses
	            ref scorectr, // int tracks score
	            this); // form in which method is called

            if (enemy_delay_ctr >= 10 && game_over == false)
            {
                // Increase speed of enemies as game progresses
                if(hitctr%10 == 0 && 0 < hitctr && hitctr < 120)
                {
                    enemy_speed += 10;
                }
                new Enemy(
	                Width/2, 
	                0, 
	                enemy_speed, 
	                100, 
	                90, 
	                this, 
	                global::SpaceGame2.Properties.Resources.enemy_ship);
	                
                enemy_delay_ctr = 0;
            }
            else
            {
                enemy_delay_ctr++;
            }
        }
```
### Explosion Class
The purpose of the Explosion class is to provide an animation for a collision between a projectile and enemy or enemy and the player. Like the Enemy and Projectile classes, the sprite is composed of an Explosion instance and a PictureBox instance stored in lists.

To add an Explosion sprite, call the constructor.
```C#
public Explosion(
	int x_position, // position of left edge
	int y_position, // position of top edge
	int time, // time in ms that the sprite exists
	int width, // width of sprite
	int height, // height of sprite
	Image image, // image of sprite (.gif)
	Form form) // form in which constructor is called
```

Example of explosion animation timing function:
```C#
        /* Event handler for explosion animation */
        private void Timer4_Tick(object sender, EventArgs e)
        {
	        /* Static function for controlling the time that 
		       each explosion image stays on screen */
            Explosion.ExplosionTime(this);
        }
```

### Other Functionality
All object/edge detection and interaction between sprites is managed in the Form class. A future project would be to migrate object detection into the Engine namespace classes.

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.