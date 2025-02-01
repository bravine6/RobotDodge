using System;
using SplashKitSDK;


public class Player
{
    private Bitmap _PlayerBitmap;
    private Bullet _Bullet;
    public int Lives = 5;

    //auto property for X
    public double X
    {
        get;
        private set;
    }
    //auto property for Y
    public double Y
    {
        get;
        private set;
    }
    //Quit property 
    public bool Quit
    {
        get;
        private set;
    }
    // Read-only property for Width
    public int Width
    {
        get
        {
            return _PlayerBitmap.Width;
        }
    }
    // Read-only properties Height
    public int Height
    {
        get
        {
            return _PlayerBitmap.Height;
        }
    }

    //Constructor for the player class
    public Player(Window gameWindow)
    {
        // Initialize the player's bitmap with the "Player.png" image
        _PlayerBitmap = new Bitmap("Player", "Resources/images/Player.png");
        
        // Set the player's initial position to the center of the game window
        X = (gameWindow.Width - Width) / 2;
        Y = (gameWindow.Height - Height) / 2;

        // Initialize the bullet object
        _Bullet = new Bullet(this);

        // Set the player's initial lives to 5
        Quit = false;
    }

    //Method to Draw the player and lives
    public void Draw(Window gameWindow)
    {        
        gameWindow.DrawBitmap(_PlayerBitmap, X, Y);
        _Bullet.Draw(gameWindow);

        //Game ends if player lives is less than 0
        //if (Lives <= 0)
        //{
          //  Quit = true;
            //gameWindow.Close();
        //}
    }

    //Handle input to check for clicks from user
    public void HandleInput(List<Bullet> bullets)
    {
        SplashKit.ProcessEvents();

        const int SPEED = 5;
        //Move up
        if (SplashKit.KeyDown(KeyCode.UpKey))
        {
            Y -= SPEED;
            //Move(4, 0);
        }
        //Move down
        else if (SplashKit.KeyDown(KeyCode.DownKey))
        {
            Y += SPEED;
            //Move(-4, 0);
        }
        //Move left
        else if (SplashKit.KeyDown(KeyCode.LeftKey))
        {
            X -= SPEED;
            //Rotate(-4);
        }
        //Move right
        else if (SplashKit.KeyDown(KeyCode.RightKey))
        {
            X += SPEED;
            //Rotate(4);
        }
        //If escape key is pressed
        if (SplashKit.KeyDown(KeyCode.EscapeKey))
        {
            Quit = true;
        }
        if (SplashKit.MouseDown(MouseButton.LeftButton))
        {            
            FireBullet(bullets);
        }
    }

    // Method to fire the bullet from the player
    public void FireBullet(List<Bullet> bullets)
    {
        if (!_Bullet.Active) // Only fire if the bullet is inactive
        {
            Bullet newBullet = new Bullet(this);
            newBullet.Shoot();
            newBullet.Active = true;
            bullets.Add(newBullet);
        }
    }

    // Method to keeo the player on the screen
    public Window StayOnWindow(Window limit)
    {
        const int GAP = 10;
        SplashKit.ProcessEvents();

        //Checking the left side
        if (X < GAP)
        {
            X = GAP;
        }
        //Checking the right side
        if (X + Width > limit.Width - GAP)
        {
            X = limit.Width - GAP - Width;
        }
        // Check top bound
        if (Y < GAP)
        {
            Y = GAP;
        }
        // Check bottom bound
        if (Y + Height > limit.Height - GAP)
        {
            Y = limit.Height - GAP - Height;
        }
        return limit;
    }

    //Method to check if the player has collided with a robot
    public bool CollidedWith(Robot other)
    {
        return _PlayerBitmap.CircleCollision(X, Y, other.CollisionCircle);
    }
}

