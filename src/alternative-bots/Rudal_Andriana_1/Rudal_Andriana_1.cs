using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Rudal_Andriana_1 : Bot
{
    
    static void Main(string[] args)
    {
        new Rudal_Andriana_1().Start();
    }

    Rudal_Andriana_1() : base(BotInfo.FromFile("Rudal_Andriana_1.json")) { }

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
            // Greedy Movement: Move zig-zag
            SetTurnLeft(400);
            Forward(200);
            SetTurnRight(400);
            Forward(200);
        }
    }

   
    public override void OnScannedBot(ScannedBotEvent e)
    {   
        // Greedy Shooting: Shoot at the closer bot with higher power
        Interruptible = true;
        if (DistanceTo(e.X, e.Y) < 100 && DistanceTo(e.X, e.Y) >= 0)
        {
            Fire(3);
        }else if (DistanceTo(e.X, e.Y) < 200 && DistanceTo(e.X, e.Y) >= 100){
            Fire(2.5);
        }else{
            
            Fire(1);
        }

        // Greedy Movement: Move towards the scanned bot
        FaceTarget(e.X,e.Y);
        Forward(10);
    }
    

    public override void OnHitBot(HitBotEvent e)
    {
        // Greedy Evasion: Move away from the hit bot
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
        // Greedy Wall Avoidance: Move away from the wall
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
        // Greedy Movement: Turn towards the target
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