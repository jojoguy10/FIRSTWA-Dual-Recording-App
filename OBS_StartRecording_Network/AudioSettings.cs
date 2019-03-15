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
        }

    }
}
