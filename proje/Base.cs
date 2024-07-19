using proje.Entity;
using proje.Formlar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proje
{
    public static class Base
    {
        public static dynamic LoginUser { get; set; }
        public static bool Customer { get; set; }

        public static void geri(Form currentForm)
        {
            frmPersonel frm = new frmPersonel();
            frm.Show();
            currentForm.Hide();
        }
    }
}
