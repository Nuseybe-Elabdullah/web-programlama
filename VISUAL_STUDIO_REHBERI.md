# Visual Studio'da Projeyi Açma Rehberi

## Hızlı Başlangıç

### Yöntem 1: Solution Dosyasını Çift Tıklama (En Kolay)
1. Dosya Gezgini'nde şu klasöre gidin:
   ```
   c:\Users\nusayba\Downloads\wetransfer_gym_2025-12-15_1709 (2)\gymv1\gym
   ```
2. **`GymManagementSystem.sln`** dosyasını çift tıklayın
3. Visual Studio otomatik olarak açılacak ve proje yüklenecek

### Yöntem 2: Visual Studio'dan Açma
1. Visual Studio'yu açın
2. **File** → **Open** → **Project/Solution** seçin
3. Şu dosyayı seçin:
   ```
   c:\Users\nusayba\Downloads\wetransfer_gym_2025-12-15_1709 (2)\gymv1\gym\GymManagementSystem.sln
   ```
4. **Open** tıklayın

## Visual Studio'da Çalıştırma

### Uygulamayı Başlatma
1. Üstte **GymManagementSystem** seçili olduğundan emin olun
2. Yeşil **▶ Play** butonuna tıklayın veya **F5** tuşuna basın
3. Tarayıcı otomatik olarak açılacak

### Debug Modu
- **F5** - Debug modunda çalıştır
- **Ctrl+F5** - Debug olmadan çalıştır (daha hızlı)

## Proje Yapısı

Solution Explorer'da göreceğiniz yapı:

```
📁 GymManagementSystem
├── 📁 Controllers          # MVC Controllers
├── 📁 Data                 # Database Context ve Migrations
├── 📁 Models               # Entity Models ve ViewModels
├── 📁 Services             # Business Logic
├── 📁 Views                # Razor Views
├── 📁 wwwroot              # Static files (CSS, JS, images)
├── 📄 Program.cs           # Uygulama başlangıç noktası
├── 📄 appsettings.json     # Yapılandırma dosyası
└── 📄 Dockerfile           # Docker yapılandırması
```

## Önemli Ayarlar

### Database Bağlantısı
`appsettings.json` dosyasında PostgreSQL bağlantı bilgileri:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=GymManagementDB;Username=postgres;Password=123456"
}
```

### Başlangıç URL'i
Uygulama varsayılan olarak şu adreste çalışır:
- **http://localhost:5000**

### Environment
- Development: `appsettings.Development.json`
- Production: `appsettings.Production.json`

## NuGet Paketleri

Proje otomatik olarak gerekli paketleri yükleyecek:
- Microsoft.AspNetCore.Identity.EntityFrameworkCore
- Npgsql.EntityFrameworkCore.PostgreSQL
- Microsoft.EntityFrameworkCore.Tools

Eğer paketler yüklenmezse:
1. Solution Explorer'da projeye sağ tık
2. **Restore NuGet Packages** seçin

## Database Migration

İlk çalıştırmada database otomatik oluşturulacak. Manuel olarak yapmak isterseniz:

### Package Manager Console'da:
```powershell
Update-Database
```

### Terminal'de:
```bash
dotnet ef database update
```

## Sorun Giderme

### "PostgreSQL bağlanamıyor" Hatası
1. PostgreSQL'in çalıştığından emin olun
2. `appsettings.json`'daki bağlantı bilgilerini kontrol edin
3. Şifre ve kullanıcı adını doğrulayın

### "Port 5000 kullanımda" Hatası
1. Eski process'i durdurun
2. Visual Studio'yu yeniden başlatın
3. Veya farklı bir port kullanın (`launchSettings.json`)

### NuGet Paket Hatası
1. **Tools** → **NuGet Package Manager** → **Package Manager Console**
2. Şu komutu çalıştırın:
   ```powershell
   Update-Package -reinstall
   ```

## Faydalı Kısayollar

- **F5** - Debug başlat
- **Ctrl+F5** - Debug olmadan çalıştır
- **Shift+F5** - Debug'ı durdur
- **Ctrl+Shift+B** - Solution'ı build et
- **Ctrl+K, Ctrl+D** - Kodu formatla
- **F12** - Tanıma git
- **Ctrl+.** - Quick actions

## Test Kullanıcıları

### Yönetici
- Email: admin@gym.com
- Şifre: Admin123!

### Üye
- Email: member@gym.com
- Şifre: Member123!

---

**Visual Studio'da başarılı çalışmalar! 🚀**
