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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace proje.Formlar
{
    public partial class frmPersonel : Form
    {
        
        public frmPersonel()
        {
            InitializeComponent();
            
        }   

        CRMEntities1 db = new CRMEntities1();
        private void ara_Click(object sender, EventArgs e)
        {
            string searchTerm = TxtSearch.Text.ToLower();

            // Arama sonuçlarını veritabanından çek
            var results = from x in db.TblMusteri
                          where x.musteriFirma.ToLower().Contains(searchTerm) ||
                                x.musteriMail.ToLower().Contains(searchTerm) ||
                                 x.yapilacak.ToLower().Contains(searchTerm) ||
                                x.musteriTel.Contains(searchTerm) ||
                                x.musteriAddress.ToLower().Contains(searchTerm)
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

            // Arama sonuçlarını DataGridView'e bağla
            dataGridView1.DataSource = results.ToList();
        }

        private void mesajform_Click(object sender, EventArgs e)
        {

           
            
            frmMesajlasma frm = new frmMesajlasma();
            frm.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            musteriduzenleme frm = new musteriduzenleme();
            frm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            musterikayit frm = new musterikayit();
            frm.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FrmGiris frm = new FrmGiris();
            frm.Show();
            this.Hide();
        }
    }
}
