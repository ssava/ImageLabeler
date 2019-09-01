using System;
using System.Windows.Forms;

namespace ImageLabeler
{
    public partial class FilterOptions : Form
    {
        public bool UseBW { get; private set; }

        public FilterOptions()
        {
            InitializeComponent();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            switch(checkBox1.CheckState)
            {
                case CheckState.Checked:
                    UseBW = true;
                    break;
                case CheckState.Unchecked:
                    UseBW = false;
                    break;

                default:
                    UseBW = false;
                    break;
            }
        }
    }
}
