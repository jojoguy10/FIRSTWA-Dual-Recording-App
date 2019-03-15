using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FIRSTWA_Recorder
{
    public partial class AudioSettings : Form
    {
        public MapMono wide = MapMono.None;
        public MapMono prog = MapMono.None;

        public AudioSettings(MapMono _wide, MapMono _prog)
        {
            InitializeComponent();

            wide = _wide;
            prog = _prog;

            if(prog == MapMono.Left)
            {
                rdoProgLeft.Checked = true;
            }
            else if (prog == MapMono.Right)
            {
                rdoProgRight.Checked = true;
            }
            else
            {
                rdoProgramStereo.Checked = true;
            }
            if(wide == MapMono.Left)
            {
                rdoWideLeft.Checked = true;
            }
            else if(wide == MapMono.Right)
            {
                rdoWideRight.Checked = true;
            }
            else
            {
                rdoWideStereo.Checked = true;
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (rdoProgLeft.Checked)
            {
                prog = MapMono.Left;
            }
            else if (rdoProgRight.Checked)
            {
                prog = MapMono.Right;
            }
            else
            {
                prog = MapMono.None;
            }
            if (rdoWideLeft.Checked)
            {
                wide = MapMono.Left;
            }
            else if (rdoWideRight.Checked)
            {
                wide = MapMono.Right;
            }
            else
            {
                wide = MapMono.None;
            }

            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
