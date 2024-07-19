using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using proje.Entity;

namespace proje.Formlar
{
    public partial class musteriduzenleme : Form
    {
        public musteriduzenleme()
        {
            InitializeComponent();
        }

        CRMEntities1 db = new CRMEntities1();

        private void musteriduzenleme_Load(object sender, EventArgs e)
        {
            var degerler = from x in db.TblMusteri
                           select new
                           {
                               x.musteriID,
                               x.musteriFirma,
                               x.musteriSifre,
                               x.musteriMail,
                               x.musteriTel,
                               x.musteriAddress,
                               x.yapilacak,
                               x.ekstrayap,
                               x.RegistrationDate
                           };
            dataGridView1.DataSource = degerler.ToList();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                // ID, Firma, Şifre, Mail, Tel ve Adres alanlarını TextBox'lara doldur
                TxtId.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value?.ToString();
                TxtFirma.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value?.ToString();
                TxtSifre.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value?.ToString();
                TxtMail.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value?.ToString();
                TxtTel.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value?.ToString();
                TxtAdres.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value?.ToString();

                // Ekstra alanını TextBox'a doldur
                TxtEkstra.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value?.ToString();

                // CheckBox'ları temizle
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;

                string cellValue = dataGridView1.Rows[e.RowIndex].Cells["yapilacak"].Value?.ToString();

                // CellValue null değilse ve virgül içeriyorsa işaretle
                if (!string.IsNullOrEmpty(cellValue))
                {
                    // CheckBox'ların metinlerini bir diziye al
                    string[] checkBoxTexts = { checkBox1.Text, checkBox2.Text, checkBox3.Text };

                    // CellValue içindeki her bir metin için kontrol yap
                    foreach (string checkBoxText in checkBoxTexts)
                    {
                        // CellValue içinde bu checkbox metni varsa işaretle
                        if (cellValue.Contains(checkBoxText))
                        {
                            if (checkBoxText == checkBox1.Text)
                                checkBox1.Checked = true;
                            if (checkBoxText == checkBox2.Text)
                                checkBox2.Checked = true;
                            if (checkBoxText == checkBox3.Text)
                                checkBox3.Checked = true;
                        }
                    }
                }
            }
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView1.SelectedRows[0].Index;
                int musteriID = Convert.ToInt32(dataGridView1.Rows[selectedIndex].Cells[0].Value);
                var musteri = db.TblMusteri.FirstOrDefault(m => m.musteriID == musteriID);
                if (musteri != null)
                {
                    // Müşteriyi veritabanından sil
                    db.TblMusteri.Remove(musteri);
                    db.SaveChanges();

                    // DataGridView'i güncelle
                    var degerler = from x in db.TblMusteri
                                   select new
                                   {
                                       x.musteriID,
                                       x.musteriFirma,
                                       x.musteriSifre,
                                       x.musteriMail,
                                       x.musteriTel,
                                       x.musteriAddress,
                                       x.yapilacak,
                                       x.ekstrayap,
                                       x.RegistrationDate
                                   };
                    dataGridView1.DataSource = degerler.ToList();

                    MessageBox.Show("Müşteri başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // TextBox'ları temizle
                    TxtId.Clear();
                    TxtFirma.Clear();
                    TxtSifre.Clear();
                    TxtMail.Clear();
                    TxtTel.Clear();
                    TxtAdres.Clear();
                    TxtEkstra.Clear();
                    checkBox1.Checked = false;
                    checkBox2.Checked = false;
                    checkBox3.Checked = false;
                }
                else
                {
                    MessageBox.Show("Müşteri bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek istediğiniz müşteriyi seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView1.SelectedRows[0].Index;
                int musteriID = Convert.ToInt32(dataGridView1.Rows[selectedIndex].Cells[0].Value);
                var musteri = db.TblMusteri.FirstOrDefault(m => m.musteriID == musteriID);

                if (musteri != null)
                {
                    musteri.musteriFirma = TxtFirma.Text;
                    musteri.musteriSifre = TxtSifre.Text;
                    musteri.musteriMail = TxtMail.Text;
                    musteri.musteriTel = TxtTel.Text;
                    musteri.musteriAddress = TxtAdres.Text;
                    musteri.ekstrayap = TxtEkstra.Text;

                    // CheckBox'ların değerlerini güncelle
                    List<string> yapilacaklar = new List<string>();
                    if (checkBox1.Checked) yapilacaklar.Add(checkBox1.Text);
                    if (checkBox2.Checked) yapilacaklar.Add(checkBox2.Text);
                    if (checkBox3.Checked) yapilacaklar.Add(checkBox3.Text);
                    musteri.yapilacak = string.Join(", ", yapilacaklar);

                    try
                    {
                        db.SaveChanges();
                        MessageBox.Show("Müşteri başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // DataGridView'i güncelle
                        var degerler = from x in db.TblMusteri
                                       select new
                                       {
                                           x.musteriID,
                                           x.musteriFirma,
                                           x.musteriSifre,
                                           x.musteriMail,
                                           x.musteriTel,
                                           x.musteriAddress,
                                           x.yapilacak,
                                           x.ekstrayap,
                                           x.RegistrationDate
                                       };
                        dataGridView1.DataSource = degerler.ToList();

                        // TextBox'ları temizle
                        TxtId.Clear();
                        TxtFirma.Clear();
                        TxtSifre.Clear();
                        TxtMail.Clear();
                        TxtTel.Clear();
                        TxtAdres.Clear();
                        TxtEkstra.Clear();
                        checkBox1.Checked = false;
                        checkBox2.Checked = false;
                        checkBox3.Checked = false;
                    }
                    catch (DbEntityValidationException ex)
                    {
                        StringBuilder sb = new StringBuilder();

                        foreach (var failure in ex.EntityValidationErrors)
                        {
                            sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                            foreach (var error in failure.ValidationErrors)
                            {
                                sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                                sb.AppendLine();
                            }
                        }

                        MessageBox.Show(sb.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Müşteri bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen güncellemek istediğiniz müşteriyi seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
            // DataGridView'i yenileme metodunu tanımla
            private void RefreshDataGridView()
            {
                var degerler = from x in db.TblMusteri
                               select new
                               {
                                   x.musteriID,
                                   x.musteriFirma,
                                   x.musteriSifre,
                                   x.musteriMail,
                                   x.musteriTel,
                                   x.musteriAddress,
                                   x.yapilacak,
                                   x.ekstrayap,
                                   x.RegistrationDate
                               };
                dataGridView1.DataSource = degerler.ToList();
            }

            // TextBox'ları temizleme metodunu tanımla
            private void ClearTextBoxes()
            {
                TxtId.Clear();
                TxtFirma.Clear();
                TxtSifre.Clear();
                TxtMail.Clear();
                TxtTel.Clear();
                TxtAdres.Clear();
                TxtEkstra.Clear();
            }

            // CheckBox'ları temizleme metodunu tanımla
            private void ClearCheckBoxes()
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
            }

        private void BtnListe_Click(object sender, EventArgs e)
        {
                // Seçilen CheckBox'ları kontrol et
                List<string> selectedCheckboxes = new List<string>();
                if (checkBox1.Checked) selectedCheckboxes.Add(checkBox1.Text);
                if (checkBox2.Checked) selectedCheckboxes.Add(checkBox2.Text);
                if (checkBox3.Checked) selectedCheckboxes.Add(checkBox3.Text);

                // TextBox'dan müşteri firmasını al
                string musteriFirma = TxtFirma.Text.Trim();

                // Tüm verileri veritabanından çek
                var allRecords = (from x in db.TblMusteri
                                  select new
                                  {
                                      x.musteriID,
                                      x.musteriFirma,
                                      x.musteriSifre,
                                      x.musteriMail,
                                      x.musteriTel,
                                      x.musteriAddress,
                                      x.yapilacak,
                                      x.ekstrayap,
                                      x.RegistrationDate
                                  }).ToList();

                // Bellek içinde filtreleme
                var filteredRecords = allRecords.Where(x =>
                    (selectedCheckboxes.Count == 0 || selectedCheckboxes.Count == x.yapilacak.Split(',').Count(y => selectedCheckboxes.Contains(y.Trim()))) &&
                    (string.IsNullOrEmpty(musteriFirma) || x.musteriFirma.Contains(musteriFirma))
                ).ToList();

                dataGridView1.DataSource = filteredRecords;
            }

            // DataGridView'i yenileme metodunu tanımla
            private void datayenileme()
            {
                var degerler = from x in db.TblMusteri
                               select new
                               {
                                   x.musteriID,
                                   x.musteriFirma,
                                   x.musteriSifre,
                                   x.musteriMail,
                                   x.musteriTel,
                                   x.musteriAddress,
                                   x.yapilacak,
                                   x.ekstrayap,
                                   x.RegistrationDate
                               };
                dataGridView1.DataSource = degerler.ToList();
            }

        private void button1_Click(object sender, EventArgs e)
        {
            Base.geri(this);
        }
    }
    }
   

