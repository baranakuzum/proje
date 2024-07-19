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
    public partial class frmnot : Form
    {
        private CRMEntities1 db = new CRMEntities1();
        private int aktifKullaniciID;
        public frmnot()
        {
            InitializeComponent();
        }
    }
}
