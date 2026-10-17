# LEARNING.md

## 1. Bu görevden önce bilmediğim şeyler ve nereden öğrendim

- **[Bölüm 2] decimal(18,2) ve Range:** EF6 `decimal` alanı varsayılan olarak decimal(18,2) yapıyor, ben yine de `OnModelCreating` içinde `HasPrecision(18, 2)` ile açıkça yazdım. Fiyat doğrulaması için `Range` kullanırken, tırnak içinde yazılan sayıların bilgisayarın dil ayarına göre okunduğunu öğrendim (ayrıntısı 3. bölümde). Kaynak: Microsoft Learn (RangeAttribute sayfası), makaleler ve hata mesajı.
- **[Bölüm 2] Seed metodu:** Seed, `Update-Database` her çalıştığında çalışıyor. Başında kontrol koymazsam ürünler her seferinde tekrar ekleniyor. Bu yüzden `if (context.Products.Any()) return;` koydum. Bunu önceden bilmiyordum. Kaynak: AI açıklaması ve kendi denemem.
- **[Bölüm 5] AJAX'ta dizi gönderme (`traditional: true`):** jQuery bir diziyi varsayılan olarak `productIds[]=1&productIds[]=2` diye gönderiyor, MVC ise `int[] productIds` parametresi için `productIds=1&productIds=2` bekliyor. `traditional: true` bu uyumsuzluğu çözüyor, onsuz diziler sunucuya bağlanmıyor. Önceden bilmiyordum. Kaynak: AI açıklaması.

## 2. Bölüm bölüm yaklaşık süre

- Bölüm 1 - Kurulum ve proje iskeleti: 10 dakika
- Bölüm 2 - Veri modeli, migration, seed: 25 dakika
- Bölüm 3 - Ürün listesi (sayfalama, arama): 30 Dakika
- Bölüm 4 - Ürün ekle/düzenle (doğrulama): 30 Dakika
- Bölüm 5 - Sipariş ekranı (AJAX, AntiForgeryToken): 45 Dakika
- Bölüm 6 - Tek transaction ile sipariş kaydı: 
- Bölüm 7 - Stok hareketleri ve sipariş listesi: 
- Bölüm 8 - A/B/C testleri, README, LEARNING.md: 

## 3. AI kullanımı ve beni yanlış yönlendirdiği yerler

AI'ı adım adım yönlendirme, kod örnekleri ve hata ayıklama için kullandım.

- **[Bölüm 2] Range hatası:** AI fiyat için `[Range(typeof(decimal), "0.01", "9999999999999999.99")]` yazmamı önerdi. `Update-Database` sırasında Seed adımında `FormatException` aldım. Sebep: tırnak içindeki "0.01" metni bilgisayarın dil ayarına göre okunuyor ve Türkçe ayarda ondalık ayırıcı virgül olduğu için çözülemiyor. Çözüm: `[Range(0.01, 9999999999999999.99)]` yazımına döndüm, hata gitti.