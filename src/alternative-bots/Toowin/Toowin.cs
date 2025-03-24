using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Toowin : Bot
{
    
    static void Main(string[] args)
    {
        new Toowin().Start();
    }

    Toowin() : base(BotInfo.FromFile("Toowin.json")) { }

    public override void Run()
    {
        // Set colors
        BodyColor = Color.Black;
        TurretColor = Color.White;
        RadarColor = Color.Green;
        BulletColor = Color.Red;
        ScanColor = Color.Red;

              
        while (IsRunning)
        {
            SetTurnLeft(400);
            Forward(200);
            SetTurnRight(400);
            Forward(200);
        }
    }

   
    public override void OnScannedBot(ScannedBotEvent e)
    {
        Interruptible = true;
        if (DistanceTo(e.X, e.Y) < 100 && DistanceTo(e.X, e.Y) >= 0)
        {
            Fire(3);
        }else if (DistanceTo(e.X, e.Y) < 200 && DistanceTo(e.X, e.Y) >= 100){
            Fire(2.5);
        }else{
            
            Fire(1);
        }
        FaceTarget(e.X,e.Y);
        Forward(10);
    }
    

    public override void OnHitBot(HitBotEvent e)
    {
        var bearing = GunBearingTo(e.X, e.Y);
        if (bearing >= 140 || bearing <= -140)
        {
            SetTurnRight(80);
            Forward(80);
        }
        else 
        {
            FaceTarget(e.X, e.Y);
            SetTurnRight(80);
            Back(80);
        }
        
    }
    

    public override void OnHitWall(HitWallEvent e)
    {
        Interruptible = true;
        TurnLeft(40);
        Forward(80);
    }

    public override void OnWonRound(WonRoundEvent e)
    {
        TurnLeft(36_000);
    }

    private void FaceTarget(double x, double y)
    {
        var bearing = BearingTo(x, y);
        if (bearing >= 0)
        {
            TurnLeft(bearing);
        }
        else
        {
            TurnRight(-bearing);
        }
    }
}