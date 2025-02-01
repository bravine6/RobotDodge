using SplashKitSDK;
using System;

public abstract class Robot
{
    //autoproperty for X
    public double X
    {
        get;
        private set;
    }

    //auto propert for Y
    public double Y
    {
        get;
        private set;
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
            return 50;
        }
    }

    //height readonly property
    public int Height
    {
        get { return 50; }
    }

    //property for velocity that stores a vector2D
    public Vector2D Velocity
    {
        get;
        private set;
    }

    //Readonly property for collide
    public Circle CollisionCircle
    {
        get
        {
            double centerX = X + Width / 2;
            double centerY = Y + Height / 2;

            //using splashkit circleat method to create circle
            return SplashKit.CircleAt(centerX, centerY, 20);
        }
    }

    //constructor for the class
    public Robot(Window gameWindow, Player player)
    {
        //Velocity for vector
        //lets test with starting at top left
        //X = 0;
        //Y = 0;

        //randomly pick position of robot just off edges of game window
        if (SplashKit.Rnd() < 0.5)
        {
            //We picked...top/bottom
            //picking a random position left to right(X)
            X = SplashKit.Rnd(gameWindow.Width);

            //work out if we are top or bottom
            if (SplashKit.Rnd() < 0.5)
            {
                Y = -Height; // Top..so above
            }
            else
            {
                Y = gameWindow.Height; // Bottom.. so below the bottom
            }
        }
        else
        {
            //We picked... left/Right
            //random position for Y
            Y = SplashKit.Rnd(gameWindow.Height);

            //working out if we are left or right
            if (SplashKit.Rnd() < 0.5)
            {
                X = -Width; //left.. so left of window
            }
            else
            {
                X = gameWindow.Width;
            }
        }

        //assigning robot a main color
        MainColor = Color.RandomRGB(200);


        const int SPEED = 4;

        //Get a point for the robot
        Point2D fromPt = new Point2D()
        {
            X = X,
            Y = Y
        };

        //Get a point for the player
        Point2D toPt = new Point2D()
        {
            X = player.X,
            Y = player.Y
        };

        //Calculate direction to head.
        Vector2D dir;
        dir = SplashKit.UnitVector(SplashKit.VectorPointToPoint(fromPt, toPt));

        //Set the speed and assign to the Velocity
        Velocity = SplashKit.VectorMultiply(dir, SPEED);
    }

    // Abstract method to draw the robot
    public abstract void Draw();

    //is off screen method to make robot stay on screen
    public bool IsOffScreen(Window screen)
    {
        return X < -Width || X > screen.Width || Y < -Height || Y > screen.Height;
    }


    // Update method to move the robot
    public void Update()
    {
        X += Velocity.X;
        Y += Velocity.Y;
    }
}

//Boxy class that inherits from Robot
public class Boxy : Robot
{
    //constructor for the class to accept game window and and pass to base class
    public Boxy(Window gameWindow, Player player) : base(gameWindow, player) { }

    // Method to draw the  boxy robot
    public override void Draw()
    {
        double leftX;
        double rightX;

        double eyeY;
        double mouthY;

        leftX = X + 12;
        rightX = X + 27;

        eyeY = Y + 10;
        mouthY = Y + 30;

        SplashKit.FillRectangle(Color.Gray, X, Y, 50, 50);
        SplashKit.FillRectangle(MainColor, leftX, eyeY, 10, 10);
        SplashKit.FillRectangle(MainColor, rightX, eyeY, 10, 10);
        SplashKit.FillRectangle(MainColor, leftX, mouthY, 25, 10);
        SplashKit.FillRectangle(MainColor, leftX + 2, mouthY + 2, 21, 6);
    }
}

// Roundy class that inherits from Robot 
public class Roundy : Robot
{
    //constructor for the class to accept game window and and pass to base class
    public Roundy(Window gameWindow, Player player) : base(gameWindow, player) { }

    // Method to draw the roundy robot
    public override void Draw()
    {
        double leftX, midX, rightX;
        double midY, eyeY, mouthY;

        leftX = X + 17;
        midX = X + 25;
        rightX = X + 33;

        midY = Y + 25;
        eyeY = Y + 20;
        mouthY = Y + 35;

        SplashKit.FillCircle(Color.White, midX, midY, 25);
        SplashKit.DrawCircle(Color.Gray, midX, midY, 25);
        SplashKit.FillCircle(MainColor, leftX, eyeY, 5);
        SplashKit.FillCircle(MainColor, rightX, eyeY, 5);
        SplashKit.FillEllipse(Color.Gray, X, eyeY, 50, 30);
        SplashKit.DrawLine(Color.Black, X, mouthY, X + 50, Y + 35);
    }
}

// Tricky robot class that inherits from Robot
public class Tricky : Robot
{
    //constructor for the class to accept game window and and pass to base class
    public Tricky(Window gameWindow, Player player) : base(gameWindow, player) { }

    // Method to draw the tricky robot
    public override void Draw()
    {
        // Draw the triangle body
        SplashKit.FillTriangle(MainColor, X + 25, Y, X, Y + 50, X + 50, Y + 50);
        SplashKit.DrawTriangle(Color.Gray, X + 25, Y, X, Y + 50, X + 50, Y + 50);

        // Draw the eyes (small circles inside the triangle)
        SplashKit.FillCircle(Color.White, X + 15, Y + 20, 5); // Left eye
        SplashKit.FillCircle(Color.White, X + 35, Y + 20, 5); // Right eye
        SplashKit.FillCircle(Color.Black, X + 15, Y + 20, 2); // Left pupil
        SplashKit.FillCircle(Color.Black, X + 35, Y + 20, 2); // Right pupil

        // Draw the mouth (a small horizontal line)
        SplashKit.DrawLine(Color.Black, X + 15, Y + 35, X + 35, Y + 35);
    }
}