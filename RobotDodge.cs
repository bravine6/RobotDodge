using System;
using SplashKitSDK;
using System.IO;
using System.Collections.Generic;

namespace SplashKitSDK
{

    public class RobotDodge
    {
        private Player _Player;
        private Window _Gamewindow;

        //declare new lists for robots
        private List<Robot> _Robots;
        private List<Robot> robotsToRemove = new List<Robot>();

        private List<Bullet> _Bullets;
        private List<Bullet> bulletsToRemove = new List<Bullet>();

        public Timer _GameTimer;
        public int Score;

        private Bitmap LivesBitmap = new Bitmap("Lives", "heart.png");


        //Read only property to ask the player if they have quit.
        public bool Quit
        {
            get { return _Player.Quit; }
        }

        //Constructor to initialize the player and gamewindow objects
        public RobotDodge(Window gamewindow)
        {
            _Gamewindow = gamewindow;
            _Player = new Player(gamewindow);
            _Robots = new List<Robot>();
            _Bullets = new List<Bullet>();
            _GameTimer = new Timer("Game Timer");
            _GameTimer.Start();
        }

        //Handle input method to ask the player to has input themselves
        public void HandleInput()
        {
            SplashKit.ProcessEvents();

            //Create a new player instance to enable player handle input
            _Player.HandleInput(_Bullets);

            //Pass in gamewindow to players stay on window
            _Player.StayOnWindow(_Gamewindow);
        }

        //Method to clear window and draw player, robots and bullets
        public void Draw()
        {
            // Clear and refresh the game window only if the game is still active
            if (Quit || _Player.Lives <= 0)
            {
                _Gamewindow.Close();
                return;
            }
            _Gamewindow.Clear(Color.Green);

            foreach (Robot robot in _Robots)
            {
                robot.Draw();
            }
            _Player.Draw(_Gamewindow);

            foreach (Bullet bullet in _Bullets)
            {
                bullet.Draw(_Gamewindow);
            }

            LivesAndScore();

            _Gamewindow.Refresh(60);
        }

        //Method to update the game
        public void Update()
        {
            // Check if the player quit or has no lives left
            if (Quit || _Player.Lives <= 0)
            {
                _Gamewindow.Close();
                return;
            }
            CheckCollisions();

            Score = Convert.ToInt32(_GameTimer.Ticks / 1000);

            //Update all robots in list
            foreach (Robot robot in _Robots)
            {
                robot.Update();
            }
            //Add new random robot to the list
            if (SplashKit.Rnd() < 0.01)
            {
                Robot newRobot = RandomRobot();
                _Robots.Add(newRobot);
            }
            // Update all bullets in the list
            foreach (Bullet bullet in _Bullets)
            {
                bullet.Update();
            }
        }

        //Method to check for collisions
        private void CheckCollisions()
        {
            //check each robot in main list
            foreach (Robot robot in _Robots)
            {
                //check if robot collides with player
                if (_Player.CollidedWith(robot) && _Player.Lives > 0)
                {
                    _Player.Lives--;
                    //add robot to the removal list
                    robotsToRemove.Add(robot);
                }
                if (robot.IsOffScreen(_Gamewindow))
                {
                    //add robot to the removal list
                    robotsToRemove.Add(robot);
                }

                foreach (Bullet bullet in _Bullets)
                {
                    if (bullet.BulletCollidedWith(robot))
                    {
                        //add robot to the removal list
                        robotsToRemove.Add(robot);
                        // bullet.Active = false;
                        bulletsToRemove.Add(bullet);
                        bullet.Active = false;
                    }
                }
            }

            // Loop through removal list and remove robots above from _Robots list
            foreach (Robot robot in robotsToRemove)
            {
                _Robots.Remove(robot);
            }
            // Remove bullets that are in the removal list
            foreach (Bullet bullet in bulletsToRemove)
            {
                _Bullets.Remove(bullet);
            }
        }
        //Method to return a new random robot
        public Robot RandomRobot()
        {
            // Randomly select a robot to be created 50% of the time
            // If the random number is less than 0.33, create a roundy robot, if less than 0.66, create a tricky robot, else create a boxy robot
            if (SplashKit.Rnd() < 0.33)
            {
                Robot roundy = new Roundy(_Gamewindow, _Player);
                return roundy;
            }
            else if (SplashKit.Rnd() < 0.66)
            {
                Robot tricky = new Tricky(_Gamewindow, _Player);
                return tricky;
            }
            else
            {
                Robot boxy = new Boxy(_Gamewindow, _Player);
                return boxy;
            }
        }

        // Method to draw the player's lives and score on the screen
        public void LivesAndScore()
        {
            DrawHearts(_Player.Lives);
            SplashKit.DrawText("Score: " + Score, Color.Black, 10, 50);
        }

        //Method to draw the hearts representing the player's lives
        public void DrawHearts(int numberOfHearts)
        {
            int heartY = 0;
            for (int i = 0; i < numberOfHearts; i++)
            {
                if (heartY < 300)
                {
                    SplashKit.DrawBitmap(LivesBitmap, heartY, 0);
                    heartY += 40;
                }
            }
        }
    }
}