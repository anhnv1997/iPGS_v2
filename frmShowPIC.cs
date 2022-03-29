using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kztek.iZCU
{
    public partial class frmShowPIC : Form
    {
        public frmShowPIC()
        {
            InitializeComponent();
        }
        public void setImage(Image _image) {
            pictureBox1.Image = _image;
        }
    }
}
