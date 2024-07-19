using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using proje.Entity;

namespace proje.Formlar
{
    public partial class frmMesajlasma : Form
    {
        private CRMEntities1 db = new CRMEntities1();
        private int aktifKullaniciID;
        public frmMesajlasma()
        {
            InitializeComponent();
            if (Base.Customer)
            {
                aktifKullaniciID = Base.LoginUser.musteriID;
            }
            else
            {
                aktifKullaniciID = Base.LoginUser.personelID;
            }

            KullanicilariYukle();
            MesajlariYukle(aktifKullaniciID);
        }
        private void MesajlariYukle(int kullaniciID)
        {
            listBox1.Items.Clear(); // ListBox'ı temizleyelim

            // Veritabanından kullanıcının aldığı mesajları çekelim
            var mesajlar = db.tblMesajlar
                            .Where(m => m.AliciID == kullaniciID && m.GonderenID != kullaniciID) // Sadece gelen mesajları ve kendi gönderdiği mesajları gösterme
                            .ToList();

            // ListBox'a mesajları ekleyelim
            foreach (var mesaj in mesajlar)
            {
                string gonderenTur = "";

                // GonderenID'nin hangi tabloya ait olduğunu kontrol edelim
                var personel = db.TblPersonel.FirstOrDefault(p => p.personelID == mesaj.GonderenID);
                var musteri = db.TblMusteri.FirstOrDefault(m => m.musteriID == mesaj.GonderenID);

                if (personel != null)
                {
                    gonderenTur = personel.personelKullanici; // Burada personel tablosundan ilgili alanı kullanıyoruz
                }
                else if (musteri != null)
                {
                    gonderenTur = musteri.musteriFirma; // Burada müşteri tablosundan ilgili alanı kullanıyoruz
                }

                // Mesaj formatını isteğinize göre ayarlayabilirsiniz
                listBox1.Items.Add($"{gonderenTur}: {mesaj.Icerik}");
            }
        }
        



        private void KullanicilariYukle()
        {
            // Müşteri ve personelleri veritabanından çekerek ComboBox'a ekleyelim
            var musteriler = db.TblMusteri.Select(m => new { Id = m.musteriID, AdSoyad = m.musteriFirma }).ToList();
            var personeller = db.TblPersonel.Select(p => new { Id = p.personelID, AdSoyad = p.personelKullanici }).ToList();

            // ComboBox'a müşterileri ekleyelim
            foreach (var musteri in musteriler)
            {
                if (Base.Customer)
                {
                    if (Base.LoginUser.musteriID == musteri.Id)
                    {
                        continue;
                    }
                }
                comboBox1.Items.Add(new ComboboxItem { Text = musteri.AdSoyad, Value = musteri.Id });
            }

            // ComboBox'a personelleri ekleyelim
            foreach (var personel in personeller)
            {
                if (!Base.Customer)
                {
                    if (Base.LoginUser.personelID != null && Base.LoginUser.personelID == personel.Id)
                    {
                        continue;
                    }
                }

                comboBox1.Items.Add(new ComboboxItem { Text = personel.AdSoyad, Value = personel.Id });
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var secilenKisi = (ComboboxItem)comboBox1.SelectedItem;
            if (secilenKisi == null)
            {
                MessageBox.Show("Lütfen bir kişi seçin.");
                return;
            }

            int aliciID = (int)secilenKisi.Value; // Mesajı alacak kişinin ID'si
            string mesajIcerik = textBox1.Text; // TextBox'a girilen mesaj

            // ListBox'a gönderilen mesajı ekle
            listBox1.Items.Add($"Siz: {mesajIcerik}");

            // Veritabanına mesajı kaydet
            tblMesajlar yeniMesaj = new tblMesajlar
            {
                GonderenID = aktifKullaniciID,
                AliciID = aliciID,
                Icerik = mesajIcerik
            };

            db.tblMesajlar.Add(yeniMesaj);
            db.SaveChanges();

            // Mesaj gönderildikten sonra TextBox'ı temizle
            textBox1.Clear();
        }
        public class ComboboxItem
        {
            public string Text { get; set; }
            public int Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear(); // ListBox'ı temizleyelim

            var secilenKisi = (ComboboxItem)comboBox1.SelectedItem;
            if (secilenKisi == null)
            {
                return;
            }

            int kisiID = (int)secilenKisi.Value; // ComboBox'ta seçilen kişinin ID'si

            // Veritabanından seçilen kişinin gönderdiği veya aldığı mesajları çekelim
            var gelenMesajlar = db.tblMesajlar
                                .Where(m => (m.GonderenID == kisiID && m.AliciID == aktifKullaniciID) || (m.GonderenID == aktifKullaniciID && m.AliciID == kisiID))
                                .ToList();

            // ListBox'a mesajları ekleyelim
            foreach (var mesaj in gelenMesajlar)
            {
                string gonderenTur = "";

                // GonderenID'nin hangi tabloya ait olduğunu kontrol edelim
                var personel = db.TblPersonel.FirstOrDefault(p => p.personelID == mesaj.GonderenID);
                var musteri = db.TblMusteri.FirstOrDefault(m => m.musteriID == mesaj.GonderenID);

                if (personel != null)
                {
                    gonderenTur = personel.personelKullanici; // Burada personel tablosundan ilgili alanı kullanıyoruz
                }
                else if (musteri != null)
                {
                    gonderenTur = musteri.musteriFirma; // Burada müşteri tablosundan ilgili alanı kullanıyoruz
                }

                // Mesaj formatını isteğinize göre ayarlayabilirsiniz
                listBox1.Items.Add($"{gonderenTur}: {mesaj.Icerik}");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Base.geri(this);
        }
    }
    }
