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

    public override void Run()
    {
        BodyColor = Color.DarkBlue;
        GunColor = Color.Black;
        RadarColor = Color.Red;

        while (IsRunning)
        {
            SetTurnRight(45);
            Forward(45);
            SetTurnLeft(45);
            Forward(45);
        }
    }


    public override void OnScannedBot(ScannedBotEvent e)
    {
        lastEnemyX = e.X;
        lastEnemyY = e.Y;

        var bearingFromGun = GunBearingTo(lastEnemyX, lastEnemyY);
        TurnGunLeft(bearingFromGun);

        if (Math.Abs(bearingFromGun) <= 3 && GunHeat == 0)
        {
            Fire(Math.Min(3 - Math.Abs(bearingFromGun), Energy - 0.1));
        }

        if (bearingFromGun == 0)
        {
            Rescan();
        }
    }

    public override void OnHitByBullet(HitByBulletEvent e)
    {
        SetTurnRight(90);
        Forward(50);
    }

    public override void OnHitWall(HitWallEvent e)
    {
        Back(100);

        SetTurnRight(90);

        Forward(100);
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