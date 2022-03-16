using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RainbowMage.OverlayPlugin.Controls
{
    public partial class ConfigErrorPrompt : Form
    {
        public ConfigErrorPrompt()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }
    }
}
