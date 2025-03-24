using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;
using System;
using System.Drawing;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

public class Rudal_Andriana_Main : Bot
{
    static void Main(string[] args)
    {
        // Membaca berkas konfigurasi dari direktori saat ini
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("Rudal_Andriana_Main.json");

        // Membaca konfigurasi ke dalam instansi BotInfo
        var config = builder.Build();
        var botInfo = BotInfo.FromConfiguration(config);

        // Membuat dan memulai bot berdasarkan informasi bot
        new Rudal_Andriana_Main(botInfo).Start();
    }

    // Konstruktor yang menerima BotInfo dan meneruskannya ke kelas dasar
    private Rudal_Andriana_Main(BotInfo botInfo) : base(botInfo) {}

    private int jariJari = 500; // Radius awal pergerakan
    private int arahPerubahanJariJari = 100; // Kecepatan perubahan radius
    private int jariJariMinimum = 500; // Radius minimum pergerakan
    private int jariJariMaksimum = 1000; // Radius maksimum pergerakan

    private int arahSetelahTabrakDinding = 1; // Faktor pengali untuk arah belok setelah menabrak dinding

    // Dipanggil ketika ronde baru dimulai -> inisialisasi dan mulai bergerak
    public override void Run()
    {
        // Mengatur warna-warna bot
        BodyColor = Color.Black;
        TurretColor = Color.Black;
        RadarColor = Color.Black;
        ScanColor = Color.Yellow;

        // Mengulang selama bot berjalan
        while (IsRunning)
        {
            // Menghitung jarak ke dinding terdekat
            double jarakKeDinding = HitungJarakKeDinding();

            // Jika terlalu dekat dengan dinding, pilih arah yang paling aman
            if (jarakKeDinding < 200) // Jarak aman dari dinding
            {
                double arahAman = CariArahAman();
                SetTurnRight(arahSetelahTabrakDinding * arahAman);
                Forward(150); // Maju untuk menjauhi dinding
                Go();
            }
            else
            {
                // Gerakan normal dengan radius yang dinamis
                jariJari += arahPerubahanJariJari;
                if (jariJari > jariJariMaksimum || jariJari < jariJariMinimum)
                {
                    arahPerubahanJariJari = -arahPerubahanJariJari;
                }
                double kecepatanTarget = 6 + ((double)jariJari / jariJariMaksimum * 3.5);
                SetTurnRight(arahSetelahTabrakDinding * 5000);
                Forward(jariJari);
                Go();
            }
        }
    }

    // Menghitung jarak terdekat ke dinding arena
    private double HitungJarakKeDinding()
    {
        double jarakX = Math.Min(X, ArenaWidth - X);
        double jarakY = Math.Min(Y, ArenaHeight - Y);
        return Math.Min(jarakX, jarakY);
    }

    // Menemukan arah yang paling menjauhi dinding
    private double CariArahAman()
    {
        // Menghitung arah menuju pusat arena
        double arahKePusat = DirectionTo(ArenaWidth / 2, ArenaHeight / 2);
        return arahKePusat;
    }

    // Dipanggil ketika bot memindai bot lain
    public override void OnScannedBot(ScannedBotEvent e)
    {
        // Menghitung jarak ke target
        double jarak = DistanceTo(e.X, e.Y);

        // Memilih kekuatan tembakan berdasarkan jarak dan energi target
        double kekuatanTembak = HitungKekuatanTembak(jarak, e.Energy, Energy);

        // Menembak target
        Fire(kekuatanTembak);
    }

    // Menghitung kekuatan tembakan berdasarkan jarak dan energi target
    private double HitungKekuatanTembak(double jarak, double energiTarget, double energiSendiri)
    {
        // Jika target dekat dan memiliki energi rendah, tembak dengan kekuatan penuh
        if (jarak < 100 && energiTarget < 20)
        {
            return 3; // Kekuatan maksimum
        }
        // Jika target sangat dekat, tembak dengan kekuatan penuh
        else if (jarak < 50){
            return 3;
        }
        // Jika target cukup jauh, tembak dengan kekuatan sedang
        else if (jarak < 400)
        {
            return 2;
        }
        // Jika target sangat jauh, tembak dengan kekuatan rendah
        else
        {
            return 1;
        }
    }

    // Dipanggil ketika bot menabrak bot lain
    public override void OnHitBot(HitBotEvent e)
    {
        // Ketika menabrak bot lain, putar ke arah berlawanan
        SetTurnLeft(45);
        Back(80); // Mundur untuk menghindari tabrakan lebih lanjut
        Go();
    }

    // Dipanggil ketika bot menabrak dinding
    public override void OnHitWall(HitWallEvent e)
    {   
        // Menyesuaikan arah belok agar tidak menabrak dinding berulang-ulang
        if (X > (ArenaWidth-7) || Y < 7){
            arahSetelahTabrakDinding = -1;
        }
        else {
            arahSetelahTabrakDinding = 1;
        }
        SetTurnRight(arahSetelahTabrakDinding * 135); // Putar 135 derajat
        Back(100); // Mundur sedikit
        Go();
    }
}