using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Aufklarungspanzerwagen : Bot
{
    static void Main(string[] args)
    {
        new Aufklarungspanzerwagen().Start();
    }

    Aufklarungspanzerwagen() : base(BotInfo.FromFile("Aufklarungspanzerwagen.json")) { }

    private double lastEnemyX = 0;
    private double lastEnemyY = 0;
    private Random random = new Random();

    public override void Run()
    {
        BodyColor = Color.Black;
        GunColor = Color.White;
        RadarColor = Color.Red; 
        while (IsRunning)
        {
            // Dynamic, unpredictable movement
            double moveDistance = 100 + random.NextDouble() * 100; // 100-200 units
            double turnAngle = 30 + random.NextDouble() * 60; // 30-90 degrees

            SetTurnRight(turnAngle);
            Forward(moveDistance);

            // Randomly change direction to be less predictable
            if (random.NextDouble() > 0.5)
            {
                SetTurnLeft(turnAngle);
            }
            else
            {
                SetTurnRight(turnAngle);
            }

            Forward(moveDistance);
            
            // Slight gun movement to keep scanning
            TurnGunLeft(10);
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        lastEnemyX = e.X;
        lastEnemyY = e.Y;

        // Calculate gun bearing to the enemy
        var bearingFromGun = GunBearingTo(lastEnemyX, lastEnemyY);
        TurnGunLeft(bearingFromGun);

        // Calculate distance to enemy
        double distance = DistanceTo(lastEnemyX, lastEnemyY);

        // Adjust firepower based on distance
        double firepower;
        if (distance < 200)       // Close range
            firepower = 3;        // Maximum damage
        else if (distance < 500)  // Medium range
            firepower = 2;
        else                      // Long range
            firepower = 1;        // Save energy and improve accuracy

        // Fire only if the gun is properly aimed
        if (Math.Abs(bearingFromGun) <= 2 && GunHeat == 0)
        {
            Fire(Math.Min(firepower, Energy - 0.1));
        }

        // Rescan if perfectly aligned
        if (bearingFromGun == 0)
        {
            Rescan();
        }
    }

    // Calculate distance between bot and enemy
    private double DistanceTo(double enemyX, double enemyY)
    {
        return Math.Sqrt(Math.Pow(enemyX - X, 2) + Math.Pow(enemyY - Y, 2));
    }


    public override void OnHitByBullet(HitByBulletEvent e)
    {
        //SetTurnRight(90);
        //Forward(50);
        SetTurnRight(90 - random.NextDouble() * 60);
        Forward(100 + random.NextDouble() * 50);
    }

    public override void OnHitWall(HitWallEvent e)
    {
        Back(100);

        // SetTurnRight(90);
        SetTurnRight(45 + random.NextDouble() * 90);
        Forward(100);
    }
    public override void OnHitBot(HitBotEvent e)
    {
        Back(50);
        TurnRight(45); 
        //SetTurnRight(45 + random.NextDouble() * 90);
        Forward(50); 
    }

    private double GunBearingTo(double enemyX, double enemyY)
    {
        double angleToEnemy = Math.Atan2(enemyY - Y, enemyX - X) * (180 / Math.PI);
        return NormalizeBearing(angleToEnemy - GunDirection);
    }

    private double NormalizeBearing(double angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }
}