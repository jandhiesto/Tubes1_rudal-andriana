using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class LowestEnergy : Bot
{
    int targetId = -1;
    double targetEnergy = 999;
    int times = 0;

    static void Main(string[] args)
    {
        new LowestEnergy().Start();
    }

    LowestEnergy() : base(BotInfo.FromFile("LowestEnergy.json")) { }
    
   
    public override void Run()
    {
        BodyColor = Color.Yellow;
        TurretColor = Color.White;
        RadarColor = Color.Green;
        BulletColor = Color.Red;
        ScanColor = Color.Red;

        while (IsRunning)
        {
            TurnGunLeft(360);
            times++

            if (times >= 3){
                targetId = -1;
                targetEnergy = 999;
                times = 0;
            }
            
            Forward(100);
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        double energy = e.Energy;
        int id = e.ScannedBotId;
        var distance = DistanceTo(e.X, e.Y);
      
        if (targetId == -1 || energy < targetEnergy) {
            targetId = id; 
            targetEnergy = energy; 
            
            SmartFire(distance);
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

    public override void OnBotDeath(BotDeathEvent e)
    {
        if (e.VictimId == targetId) {
            targetId = -1;
            targetEnergy = 999;
            times = 0;
        }
    }

    public override void OnHitWall(HitWallEvent e)
    {
        TurnRight(180);
    }

    public override void OnHitBot(HitBotEvent e){
        TurnRight(90);
    }

    public override void OnBulletFired(BulletFiredEvent e){
        times--;
    }
}