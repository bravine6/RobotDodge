using System;
using SplashKitSDK;

public class Bullet
{
    //private Bitmap _BulletBitmap;
    private Player _Player;

    //autoproperty for X
    public double X
    {
        get;
        set;
    }

    //auto property for Y
    public double Y
    {
        get;
        set;
    }

    //property for color
    public Color MainColor
    {
        get;
        private set;
    }

    //width readonly property
    public int Width
    {
        get
        {
            return 20;
            //return _BulletBitmap.Width;
        }
    }
    //height readonly property
    public int Height
    {
        get
        {
            return 20;
            //return _BulletBitmap.Height;
        }
    }
    //property for velocity that stores a vector2D
    public Vector2D Velocity
    {
        get;
        set;
    }

    // Active property for bullet
    public bool Active
    {
        get;
        set;
    }

    public Bullet(Player player)
    {
        // Initialize the player's bitmap with the "Player.png" image
        _Player = player;
        //_BulletBitmap = new Bitmap("Bullet", "Resources/images/bullet.png");

        // draw the bullet using fill circle
        SplashKit.FillCircle(MainColor, Width, Height, 10);
        MainColor = Color.Red;

        //set the bullet to the player
        X = _Player.X + _Player.Width / 2;
        Y = _Player.Y + _Player.Height / 2;
        Active = false;
    }
    //Method to draw the bullet
    public void Draw(Window gameWindow)
    {
        if (Active)
        {
            //SplashKit.DrawBitmap(_BulletBitmap, X, Y);
            SplashKit.FillCircle(MainColor, X, Y, 10);
        }
    }

    // Update the bullet's position based on its velocity
    public void Update()
    {
        if (Active)
        {
            // Update the bullet's position based on its velocity
            X += Velocity.X;
            Y += Velocity.Y;

            // Set bullet to false if it goes off screen
            if (X < 0 || X > SplashKit.ScreenWidth() || Y < 0 || Y > SplashKit.ScreenHeight())
            {
                Active = false;
            }
        }
    }

    // Method to shoot the bullet
    public void Shoot()
    {
        // Set the bullet to the player
        X = _Player.X + _Player.Width / 2;
        Y = _Player.Y + _Player.Height / 2;

        // Set the speed of the bullet
        const int SPEED = 7;

        // Get the point of the bullet
        Point2D fromPt = new Point2D()
        {
            X = X,
            Y = Y
        };
        // Get the point of the mouse
        Point2D mousePt = SplashKit.MousePosition();
        //Calculate the vector direction
        Vector2D direction;
        direction = SplashKit.UnitVector(SplashKit.VectorPointToPoint(fromPt, mousePt));

        // Set the speed and assign to the velocity
        Velocity = SplashKit.VectorMultiply(direction, SPEED);

        Active = true;
    }



    // Method to check if the bullet is off screen
    public bool IsOffScreen(Window screen)
    {
        return X < -Width || X > screen.Width || Y < -Height || Y > screen.Height;
    }

    // Method to check if the two circles have collided
    private bool CircleCollision(double x1, double y1, Circle robotCircle)
    {
        double dx = x1 - robotCircle.Center.X;
        double dy = y1 - robotCircle.Center.Y;
        double distance = Math.Sqrt(dx * dx + dy * dy);
        return distance <= robotCircle.Radius;
    }

    // Method to check if the bullet has collided with the robot
    public bool BulletCollidedWith(Robot attacker)
    {
        return CircleCollision(X, Y, attacker.CollisionCircle);
    }


}

