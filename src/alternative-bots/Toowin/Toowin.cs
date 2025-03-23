using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

// ------------------------------------------------------------------
// 
// ------------------------------------------------------------------
// A sample bot original made for Robocode by Mathew Nelson.
// Ported to Robocode Tank Royale by Flemming N. Larsen.
//
// This bot moves to a corner, then swings the gun back and forth.
// If it dies, it tries a new corner in the next round.
// ------------------------------------------------------------------
public class Toowin : Bot
{
    
    // The main method starts our bot
    static void Main(string[] args)
    {
        new Toowin().Start();
    }

    // Constructor, which loads the bot config file
    Toowin() : base(BotInfo.FromFile("Toowin.json")) { }

    // Called when a new round is started -> initialize and do some movement
    public override void Run()
    {
        // Set colors
        BodyColor = Color.Black;
        TurretColor = Color.White;
        RadarColor = Color.Green;
        BulletColor = Color.Red;
        ScanColor = Color.Red;

              
        // Spin gun back and forth
        while (IsRunning)
        {
            // Tell the game that when we take move, we'll also want to turn right... a lot
            SetTurnLeft(400);
                      
            // Start moving (and turning)
            Forward(200);
            SetTurnRight(400);
            Forward(200);
        }
    }

    // We saw another bot -> stop and fire!
    public override void OnScannedBot(ScannedBotEvent e)
    {
    
        if (DistanceTo(e.X, e.Y) < 100 && DistanceTo(e.X, e.Y) >= 0)
        {
            Fire(4);
        }else if (DistanceTo(e.X, e.Y) < 200 && DistanceTo(e.X, e.Y) >= 100){
            Fire(2);
        }else{
            
            Fire(3.5);
        }
        TurnToFaceTarget(e.X,e.Y);
        // Forward(20);
    }

    // Custom fire method that determines firepower based on distance.
    // distance: The distance to the bot to fire at.
    

    public override void OnHitBot(HitBotEvent e)
    {
        // Interruptible = true;
        var bearing = GunBearingTo(e.X, e.Y);
        if (bearing >= 140 || bearing <= -140)
        {
            SetTurnRight(80);
            Forward(80);
        }
        else if (bearing >= -30 && bearing <= 30)
        {
            TurnToFaceTarget(e.X, e.Y);
            SetTurnRight(80);
            Back(80);
        } else if (bearing >= 30 && bearing <=40 || bearing >= -40 && bearing <= -30){
            SetTurnRight(80);
            Back(80);
        }else {
            SetTurnRight(80);
            Forward(80);
        }
        
    }

    // public override void OnHitByBullet(HitByBulletEvent e){
    //     SetTurnRight(80);
    //     Back(80);
    // }

    // public override void OnBulletHit(BulletHitBotEvent e){
    //     TurnToFaceTarget(e.X,e.Y);
    //     Fire(1);
    // }
    

    public override void OnHitWall(HitWallEvent e)
    {
        Interruptible = true;
        TurnLeft(40);
        Forward(80);
        // Back(80);
        // SetTurnRight(160);
        // Back(80);
       
    }

    public override void OnWonRound(WonRoundEvent e)
    {
        // Victory dance turning right 360 degrees 100 times
        TurnLeft(36_000);
    }

    private void TurnToFaceTarget(double x, double y)
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
        // Fire(3);
    }
}