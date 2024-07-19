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
using System.Data.SqlClient;
using System.Data.Entity.Validation;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
namespace proje.Formlar
{
    public partial class musterikayit : Form
    {
        public musterikayit()
        {
            InitializeComponent();
        }

        CRMEntities1 db = new CRMEntities1();

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (TxtSifre.Text==TxtSifteTekrar.Text)
            {
                try
                {
                    TblMusteri t = new TblMusteri();
                    t.musteriFirma = TxtFirma.Text;
                    t.musteriSifre = TxtSifre.Text;
                    t.musteriMail = TxtMail.Text;
                    t.musteriAddress = TxtAddress.Text;
                    t.musteriTel = TxtTel.Text;


                    // Seçili olan checkboxları eklemek
                    List<string> selectedTasks = new List<string>();
                    if (checkBox1.Checked) selectedTasks.Add(checkBox1.Text);
                    if (checkBox2.Checked) selectedTasks.Add(checkBox2.Text);
                    if (checkBox3.Checked) selectedTasks.Add(checkBox3.Text);


                    string yapilacakMetni = string.Join(" ", selectedTasks);
                    if (yapilacakMetni.Length > 30)
                    {
                        yapilacakMetni = yapilacakMetni.Substring(0, 30); // 30 karakter ile sınırlandırma
                    }
                    t.yapilacak = yapilacakMetni;

                    t.DateOfBirth = DateTime.Today;
                    t.RegistrationDate = DateTime.Now;
                    t.ekstrayap = TxtEkstra.Text;

                    db.TblMusteri.Add(t);
                    db.SaveChanges();
                    MessageBox.Show("Müşteri bilgileri sisteme başarılı bir şekilde kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
                catch (DbEntityValidationException ex)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            sb.AppendLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                        }
                    }
                    MessageBox.Show(sb.ToString(), "Doğrulama Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Şifreniz uyuşmuyor lütfen tekrar deneyiniz.","Uyarı",MessageBoxButtons.OK,MessageBoxIcon.Stop);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Base.geri(this);
        }
    }
}
