using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using proje.Entity;
using static proje.Formlar.frmMesajlasma;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static proje.Formlar.frmnot;

namespace proje.Formlar
{
    public partial class frmnot : Form
    {
        private CRMEntities1 db = new CRMEntities1();
        private int aktifKullaniciID;
        public frmnot()
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

            
            NotlariYukle(aktifKullaniciID);
            ComboBoxID();
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
        private void ComboBoxID()
        {

            comboBox1.Items.Clear();

            // Kullanıcıya ait notları al
            var notlar = db.Notlar.Where(n => n.KullaniciID == aktifKullaniciID).ToList();

            // Her notu ComboBox'a ekle
            foreach (var not in notlar)
            {
                comboBox1.Items.Add(new ComboboxItem { Text = $"Not ID: {not.NotID}", Value = not.NotID });
            }
        }
        private void NotlariYukle(int kullaniciID)
        {// ListBox'ı temizle
            listBox1.Items.Clear();

            // Kullanıcıya ait notları al
            var notlar = db.Notlar.Where(n => n.KullaniciID == kullaniciID).ToList();

            // Her notu ListBox'a ekle
            foreach (var not in notlar)
            {
                listBox1.Items.Add($"Not ID: {not.NotID} - İçerik: {not.Notlar1}");
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = comboBox1.SelectedItem as ComboboxItem;

            if (selectedItem != null)
            {
                int selectedNotID = selectedItem.Value;

                // ListBox'ı temizle
                listBox1.Items.Clear();

                // Seçilen NotID'ye sahip notu al
                var not = db.Notlar.FirstOrDefault(n => n.NotID == selectedNotID);

                if (not != null)
                {
                    // ListBox'a notun içeriğini ekle
                    listBox1.Items.Add($"Not ID: {not.NotID} - İçerik: {not.Notlar1}");
                }
                else
                {
                    listBox1.Items.Add("Seçilen not bulunamadı.");
                }
            }
        }
        private void Not_Guncelle_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Lütfen güncellemek istediğiniz notu seçin.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Lütfen bir not içeriği girin.");
                return;
            }

            var secilenKullanici = (ComboboxItem)comboBox1.SelectedItem;
            int notID = secilenKullanici.Value;
            string yeniNotIcerigi = textBox1.Text;

            // Seçilen NotID'ye sahip notu al
            var not = db.Notlar.FirstOrDefault(n => n.NotID == notID);

            if (not != null)
            {
                // Notun içeriğini güncelle
                not.Notlar1 = yeniNotIcerigi;
                db.SaveChanges();

                // ListBox'ı güncelle
                NotlariYukle(aktifKullaniciID);

                // TextBox'ı temizle
                textBox1.Clear();

                MessageBox.Show("Not başarıyla güncellendi.");
            }
            else
            {
                MessageBox.Show("Seçilen not bulunamadı.");
            }
        }
        private void sil_btn_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Lütfen silmek istediğiniz notu seçin.");
                return;
            }

            var secilenKullanici = (ComboboxItem)comboBox1.SelectedItem;
            int notID = secilenKullanici.Value;

            // Kullanıcıdan silme işlemi için onay alın
            var result = MessageBox.Show("Bu notu tamamen silmek istediğinizden emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                return;
            }

            // Seçilen NotID'ye sahip notu al
            var not = db.Notlar.FirstOrDefault(n => n.NotID == notID);

            if (not != null)
            {
                // Notu veritabanından kaldır
                db.Notlar.Remove(not);
                db.SaveChanges();

                // ListBox'ı ve ComboBox'ı güncelle
                NotlariYukle(aktifKullaniciID);
                ComboBoxID();

                // TextBox'ı temizle
                textBox1.Clear();

                MessageBox.Show("Not başarıyla silindi.");
            }
            else
            {
                MessageBox.Show("Seçilen not bulunamadı.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // TextBox'tan not içeriğini al
            string yeniNotIcerigi = textBox1.Text;

            // İçeriğin boş olup olmadığını kontrol et
            if (string.IsNullOrWhiteSpace(yeniNotIcerigi))
            {
                MessageBox.Show("Lütfen bir not içeriği girin.");
                return;
            }

            // Yeni not nesnesi oluştur
            Notlar yeniNot = new Notlar
            {
                KullaniciID = aktifKullaniciID,
                Notlar1 = yeniNotIcerigi
            };

            // Veritabanına yeni notu ekle
            db.Notlar.Add(yeniNot);
            db.SaveChanges();

            // ListBox'ı ve ComboBox'ı güncelle
            NotlariYukle(aktifKullaniciID);
            ComboBoxID();

            // TextBox'ı temizle
            textBox1.Clear();

            // Kullanıcıya başarı mesajı göster
            MessageBox.Show("Yeni not başarıyla eklendi.");
        }
    }
}

