using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;


public class Targetting : Bot
{
    int dist = 50;
    int targetId = -1;
    int rotation = 0;
    static void Main(string[] args)
    {
        new Targetting().Start();
    }

    Targetting() : base(BotInfo.FromFile("Targetting.json")) { }

    public override void Run()
    {
        BodyColor = Color.Red;
        TurretColor = Color.White;
        RadarColor = Color.Black;
        BulletColor = Color.Black;
        ScanColor = Color.Green;

              
        while (IsRunning)
        {
            TurnGunLeft(360);
            rotation++;
            if (rotation >= 2 && targetId != -1){ 
                targetId = -1;
                rotation = 0;
            }
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        if (targetId == -1 )
        {
            targetId = e.ScannedBotId;
        }
        
        if (e.ScannedBotId == targetId)
        {
            SmartFire(DistanceTo(e.X, e.Y));
            rotation = 0;
            TurnGunRight(0.1);
            TurnGunToFaceTarget(e.X,e.Y);
        }
    }    

    public override void OnHitByBullet(HitByBulletEvent e)
    {
        TurnLeft(NormalizeRelativeAngle(90 - (Direction - e.Bullet.Direction)));

        Forward(dist);
        dist *= -1; 

    }

    public override void OnHitBot(HitBotEvent e)
    {
        TurnGunToFaceTarget(e.X,e.Y);
        Fire(3);
    }

    public override void OnBotDeath(BotDeathEvent e)
    {
        if (e.VictimId == targetId) {
            targetId = -1;
            rotation = 0;
        }
    }

    private void TurnGunToFaceTarget(double x, double y)
    {
        var direction = DirectionTo(x, y);
        var gunBearing = NormalizeRelativeAngle(direction - GunDirection);
        if (gunBearing >= 0)
        {
            TurnGunLeft(gunBearing);
        }
        else
        {
            TurnGunRight(-gunBearing);
        }
       
    }

    private void SmartFire(double distance)
    {
        if (distance < 150 && distance >= 0)
        {
            Fire(3);
        }else if (distance < 400 && distance >= 150){
            Fire(2);
        }else{
            Fire(1);
        }
    }
}



