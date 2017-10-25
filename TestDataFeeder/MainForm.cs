using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestDataFeeder
{
    public partial class MainForm : Form
    {
        ChordsInterface chords;

        public MainForm(ChordsInterface chords_)
        {
            InitializeComponent();
            chords = chords_;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Do nothing
        }
    }
}
