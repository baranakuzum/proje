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

namespace proje.Formlar
{
    public partial class FrmGiris : Form
    {
        CRMEntities1 db = new CRMEntities1();

        public FrmGiris()
        {
            InitializeComponent();
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = TxtKullanici.Text.Trim();
            string sifre = TxtSifre.Text.Trim();

            if (rbtnPersonel.Checked)
            {
                // Personel girişi kontrolü (Örneğin veritabanında TblPersonel tablosu kullanılarak yapılabilir)
                var personel = db.TblPersonel.FirstOrDefault(p => p.personelKullanici == kullaniciAdi && p.personelSifre == sifre);
                if (personel != null)
                {
                    frmPersonel personelForm = new frmPersonel();
                    Base.LoginUser = personel;
                    personelForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Personel adı veya şifre yanlış.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (rbtnKullanici.Checked)
            {
                // Kullanıcı girişi kontrolü
                var musteri = db.TblMusteri.FirstOrDefault(m => m.musteriFirma == kullaniciAdi && m.musteriSifre == sifre);
                if (musteri != null)
                {
                    frmKullanici kullaniciForm = new frmKullanici();
                    Base.Customer = true;

                    Base.LoginUser = musteri;
                    kullaniciForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre yanlış.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir rol seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
