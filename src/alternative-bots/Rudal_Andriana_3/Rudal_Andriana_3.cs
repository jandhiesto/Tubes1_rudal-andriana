using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Rudal_Andriana_3 : Bot
{
    static void Main(string[] args)
    {
        new Rudal_Andriana_3().Start();
    }

    Rudal_Andriana_3() : base(BotInfo.FromFile("Rudal_Andriana_3.json")) { }

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
            MoveToCenter();

            TurnGunLeft(10);
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        lastEnemyX = e.X;
        lastEnemyY = e.Y;

        var bearingFromGun = GunBearingTo(lastEnemyX, lastEnemyY);
        TurnGunLeft(bearingFromGun);

        double distance = DistanceTo(lastEnemyX, lastEnemyY);
        double firepower = CalculateFirepower(distance);

        if (Math.Abs(bearingFromGun) <= 2 && GunHeat == 0)
        {
            Fire(Math.Min(firepower, Energy - 0.1));
        }

        if (bearingFromGun == 0)
        {
            Rescan();
        }
    }

    public override void OnHitByBullet(HitByBulletEvent e)
    {
        SetTurnRight(60 - random.NextDouble() * 120);
        Back(80 + random.NextDouble() * 50);
        SetTurnRight(random.NextDouble() * 180);
        Forward(100 + random.NextDouble() * 50);
    }

    public override void OnHitWall(HitWallEvent e)
    {
        Back(100);
        SetTurnRight(45 + random.NextDouble() * 90);
        Forward(100);
    }

    public override void OnHitBot(HitBotEvent e)
    {
        if (e.IsRammed)
        {
            Fire(3);
        }

        if (e.Energy < 20)
        {
            Forward(30);
            Fire(3);
        }
        else
        {
            Back(50);
            SetTurnRight(random.NextDouble() * 90);
            Forward(50);
        }
    }

    private void MoveToCenter()
    {
        double centerX = ArenaWidth / 2;
        double centerY = ArenaHeight / 2;
        double angleToCenter = Math.Atan2(centerY - Y, centerX - X) * (180 / Math.PI);
        double turnAngle = NormalizeBearing(angleToCenter - Direction);

        SetTurnRight(turnAngle);
        Forward(DistanceTo(centerX, centerY) * 0.5); // Move halfway to the center
    }

    private double CalculateFirepower(double distance)
    {
        if (distance < 200)
            return 3;
        else if (distance < 500)
            return 2;
        else
            return 1;
    }

    private double DistanceTo(double enemyX, double enemyY)
    {
        return Math.Sqrt(Math.Pow(enemyX - X, 2) + Math.Pow(enemyY - Y, 2));
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