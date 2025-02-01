using System;
using SplashKitSDK;

namespace CompleteRobotDodge
{
    public class Program
    {
        public static void Main()
        {
            //Creating game window
            Window gameWindow;
            gameWindow = new Window("Player Scene by Bravine", 800, 600);

            // Create a Player instance
            Player player = new Player(gameWindow);

            //Robotdodge object
            RobotDodge game = new RobotDodge(gameWindow);

            while (!gameWindow.CloseRequested && !player.Quit)
            {
                //Process user inputs from Keyboard             
                SplashKit.ProcessEvents();

                //Game handles input
                game.HandleInput();

                //Game updates
                game.Update();

                //Draw on the window
                game.Draw();
            }
            gameWindow.Close();
        }
    }
}
