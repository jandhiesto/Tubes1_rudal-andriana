using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;


public class SDS : Bot
{

    // The main method starts our bot
    static void Main(string[] args)
    {
        new SDS().Start();
    }

    // Constructor, which loads the bot config file
    SDS() : base(BotInfo.FromFile("SDS.json")) { }
    int targetId = -1;
    double targetEnergy = 999;
    bool shot= false;
    int times = 0;
   
    public override void Run()
    {
        // Set colors
        BodyColor = Color.Yellow;
        TurretColor = Color.White;
        RadarColor = Color.Green;
        BulletColor = Color.Red;
        ScanColor = Color.Red;


        while (IsRunning)
        {
            TurnGunLeft(360);
            if (!shot){
                times++;
            }
            
            if (times >= 4 && !shot){
                targetId = -1;
                targetEnergy = 999;
                times = 0;
            }
            shot = false;
            Forward(100);

        }
    }

    // // We saw another bot -> 
    public override void OnScannedBot(ScannedBotEvent e)
    {
        // Ambil informasi tentang robot yang terdeteksi
        double energy = e.Energy;
        int id = e.ScannedBotId;

        // Jika ini adalah target pertama yang kita temui
        if (targetId == -1 || energy < targetEnergy) {
            targetId = id; // Set target baru
            targetEnergy = energy; // Update energi target
            
            Fire(3);
        }

    }
        
    private void SmartFire(double distance)
    {
        if (distance < 100 && distance >= 0)
        {
            Fire(3);
        }else if (distance < 200 && distance >= 100){
            Fire(2);
        }else{
            Fire(1);
        }
        
    }

    public override void OnBotDeath(BotDeathEvent e)
    {
        // Reset target jika target mati
        if (e.VictimId == targetId) {
            targetId = -1;
            targetEnergy = 999;
            times = 0;
        }
    }

    public override void OnHitWall(HitWallEvent e)
    {
        TurnRight(180);
        // Forward(50);
    }

    public override void OnHitBot(HitBotEvent e){
        Interruptible=true;
        TurnRight(180);
    }

    public override void OnWonRound(WonRoundEvent e)
    {
        TurnRight(36_000);
    }

    private void TurnGunToTarget(double x, double y)
    {
        var bearing = GunBearingTo(x, y);
        if (bearing >= 0)
        {
            TurnGunLeft(bearing);
        }
        else
        {
            TurnGunRight(-bearing);
        }
    }

    public override void OnBulletFired(BulletFiredEvent e){
        shot = true;
    }


}