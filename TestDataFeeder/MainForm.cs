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

        uint instrumentId;
        int dataValue;

        public MainForm(ChordsInterface chords_)
        {
            InitializeComponent();
            chords = chords_;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            linkPortalUrl.Text = chords.PortalUrl;
            Logger.Instance.LogBox = textLog;
        }

        private void textInstrumentId_TextChanged(object sender, EventArgs e)
        {
            uint tmp;

            if (textInstrumentId.Text == string.Empty) return;

            if(UInt32.TryParse(textInstrumentId.Text, out tmp))
            {
                instrumentId = tmp;
            }
            else
            {
                Logger.Instance.LogError("Must enter a valid instrument ID.");
            }
        }

        private void textDataValue_TextChanged(object sender, EventArgs e)
        {
            int tmp;

            if (textDataValue.Text == string.Empty) return;

            if (Int32.TryParse(textDataValue.Text, out tmp))
            {
                dataValue = tmp;
            }
            else
            {
                Logger.Instance.LogError("Must enter a valid value.");
            }
        }

        private async void buttonSubmit_ClickAsync(object sender, EventArgs e)
        {
            await chords.CreateMeasurementAsync(instrumentId, dataValue);
        }
    }
}
