# Tubes1_rudal-andriana
# Tugas Besar Algoritma Greedy dalam Robocode

## Anggota Kelompok
1. Joel Hotlan Haris Siahaan - 13523025 
2. Boye Mangaratua Ginting - 13523127 
3. Ivant Samuel Silaban - 13523129

## Deskripsi Proyek
Proyek ini merupakan implementasi dari algoritma *Greedy* dalam pembuatan bot untuk permainan **Robocode Tank Royale**. Bot yang dibuat akan mengambil keputusan berdasarkan prinsip *Greedy*, yaitu selalu memilih tindakan yang memberikan keuntungan terbesar dalam jangka pendek, dengan harapan bahwa hasil akhirnya juga optimal.

## Tentang Robocode Tank Royale
Robocode Tank Royale adalah game simulasi pertarungan tank berbasis skrip program, di mana setiap bot harus bertarung melawan bot lain dengan strategi yang telah diprogram. Bot dapat melakukan berbagai aksi pada setiap siklus permainan (*turn*), seperti:
- Bergerak maju atau mundur
- Memutar badan tank, meriam, atau radar
- Menembak peluru dengan energi tertentu
- Menggunakan radar untuk mendeteksi lawan
- Merespons event seperti tertembak atau bertabrakan

## Implementasi Algoritma Greedy
Bot dalam proyek ini mengadopsi beberapa strategi *Greedy*, termasuk:
- **Greedy by Energy**: Memilih target yang paling mudah ditembak berdasarkan jarak dan pergerakan.
- **Greedy by Targeting**: Menghindari serangan musuh dengan pergerakan optimal untuk bertahan lebih lama.
- **Greedy by Movement**: Menyesuaikan daya tembak sesuai jarak musuh untuk meningkatkan akurasi dan efisiensi energi.
- **Greedy by Fire**: Mengoptimalkan pemindaian untuk memaksimalkan deteksi lawan dan akurasi tembakan.

## Cara Menjalankan Bot
### 1. Persiapan dan Setup Game Engine
Sebelum menjalankan bot, pastikan Anda telah menyiapkan game engine **Robocode Tank Royale** dengan langkah-langkah berikut:
1. Unduh starter pack dari repositori berikut:  
   [Starter Pack - GitHub](https://github.com/Ariel-HS/tubes1-if2211-starter-pack)
2. Download file `robocode-tankroyale-gui-0.30.0.jar` (game engine yang telah dimodifikasi).
3. Download dan ekstrak `TemplateBot.zip` sebagai template untuk bot.
4. (Opsional) Anda dapat mengunduh source code lengkap, yang mencakup:
   - Sample bot bawaan dari Robocode
   - Source code game engine yang dapat dibangun ulang menggunakan Gradle jika diperlukan

### 2. Cara Menjalankan Game Engine
Setelah persiapan selesai, ikuti langkah-langkah berikut untuk menjalankan game:
1. Jalankan aplikasi GUI dengan menjalankan file `.jar`:  
   ```bash
   java -jar robocode-tankroyale-gui-0.30.0.jar
   ```
2. Setup konfigurasi *booter* sesuai kebutuhan.
3. Mulai sebuah battle baru.
4. *Boot* bot yang ingin dimainkan.
5. Tambahkan bot ke dalam permainan dan jalankan battle.

## Kontribusi dan Pembagian Tugas
Setiap anggota kelompok bertanggung jawab atas bagian tertentu dalam proyek ini, seperti:
- **Joel Siahaan**: Implementasi strategi *Greedy by Energy dan Fire*.
- **Boye Ginting**: Implementasi strategi *Greedy by Movement and Fire*.
- **Ivant Silaban**: Implementasi strategi *Greedy by Targeting and Fire*.

## Lisensi
Proyek ini dibuat untuk pemenuhan tugas besar 1 dalam mata kuliah *IF 2240 - Strategi Algoritma*.

---

**🚀🤖**
