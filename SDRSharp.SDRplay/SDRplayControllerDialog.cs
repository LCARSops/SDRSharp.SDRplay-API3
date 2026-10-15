using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace SDRSharp.SDRplay
{
    public partial class SDRplayControllerDialog : Form
    {
        private readonly SDRplayIO _Parent;

        // Bandwidth list: NativeMethods.SupportedBandwidths (single source of truth)

        // IF types come from the combo designer items (Zero IF / Low IF)

        // Sample rate and bandwidth are independent (no MinSampleRatesIndex)

        // Rates come from SDRplayDevice.SampleRatesKHz (single source of truth)
        private List<float> SupportedSampleRatesMHz
        {
            get
            {
                var list = new List<float>();
                foreach (float khz in SDRplayDevice.SampleRatesKHz)
                    list.Add(khz / 1000.0f);
                return list;
            }
        }

        public SDRplayControllerDialog(SDRplayIO parent)
        {
            InitializeComponent();
            _Parent = parent;
        }
        
        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
            {
                cboIFType.SelectedIndex = _Parent.IFType;
                {
                    int bwVal = (int)_Parent.Bandwidth;
                    int sel = 3;
                    var bws = NativeMethods.SupportedBandwidths;
                    for (int i = 0; i < bws.Length; i++)
                        if ((int)bws[i] == bwVal) { sel = i; break; }
                    cboBandwidth.SelectedIndex = sel;
                }
                numGR.Value = _Parent.GainReduction;
                FQCorrection.Value = (Decimal)_Parent.FQPPM;              
                // LNA state: 0 = max gain. Range depends on device (RSP1: 0..3, RSP1A: 0..9)
                int maxLna = _Parent.MaxLnaState;
                LNAGRSlider.Minimum = 0;
                LNAGRSlider.Maximum = maxLna;
                LNAGRSlider.TickFrequency = 1;
                int lna = Math.Max(0, Math.Min(maxLna, _Parent.LNAState));
                LNAGRSlider.Value = lna;
                LNAGRThreshTB.Text = lna.ToString();
                ADCsetpoint.Value = _Parent.AGCsetpoint;
                AGCEnabled.Checked = _Parent.AGCEnabled;
                AGCENState();
                // IF gain reduction range for API 3.x
                numGR.Minimum = 20;
                numGR.Maximum = 59;
                if (numGR.Value < 20) numGR.Value = 20;
                if (numGR.Value > 59) numGR.Value = 40;
                numGRSlider.Minimum = 0;
                numGRSlider.Maximum = 39; // maps 20..59
                numGRSlider.Value = 59 - (int)numGR.Value;
            }
        }

        private void SDRplayControlDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void SampleRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            float SR;

            SR = (float)Convert.ToDouble(SampleRate.Text);
            for (int i = 0; i < SupportedSampleRatesMHz.Count; i++)
            {
                if (SR == SupportedSampleRatesMHz[i])
                {
                    _Parent.InternalSampleRate = i;
                }
            }
        }


        private void FillAllSampleRates(bool preserveSelection)
        {
            string previous = SampleRate.Text;
            SampleRate.Items.Clear();
            var rates = SupportedSampleRatesMHz;
            for (int i = 0; i < rates.Count; i++)
                SampleRate.Items.Add(rates[i].ToString("0.00"));
            int restore = -1;
            if (preserveSelection)
            {
                for (int i = 0; i < rates.Count; i++)
                {
                    if (rates[i].ToString("0.00") == previous)
                    {
                        restore = i;
                        break;
                    }
                }
            }
            if (restore < 0)
            {
                int idx = _Parent.InternalSampleRate;
                restore = (idx >= 0 && idx < rates.Count) ? idx : Math.Min(8, rates.Count - 1);
            }
            SampleRate.SelectedIndex = restore;
            SampleRate.Enabled = true;
        }

        private void FillAllBandwidths(bool lowIfOnly)
        {
            string previous = cboBandwidth.SelectedItem != null ? cboBandwidth.SelectedItem.ToString() : "";
            cboBandwidth.Items.Clear();
            var bws = NativeMethods.SupportedBandwidths;
            int count = lowIfOnly ? Math.Min(4, bws.Length) : bws.Length;
            for (int i = 0; i < count; i++)
                cboBandwidth.Items.Add(NativeMethods.BandwidthLabel(bws[i]));
            int restore = -1;
            for (int i = 0; i < cboBandwidth.Items.Count; i++)
            {
                if (cboBandwidth.Items[i].ToString() == previous)
                {
                    restore = i;
                    break;
                }
            }
            if (restore < 0)
            {
                // Match parent bandwidth (mir enum value == api enum value)
                int bwVal = (int)_Parent.Bandwidth;
                restore = 3; // default 1.536
                for (int i = 0; i < count; i++)
                {
                    if ((int)bws[i] == bwVal)
                    {
                        restore = i;
                        break;
                    }
                }
            }
            cboBandwidth.SelectedIndex = restore;
        }

        private void cboBandwidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBandwidth.SelectedIndex < 0) return;
            _Parent.Bandwidth = NativeMethods.SupportedBandwidths[cboBandwidth.SelectedIndex];
            // Sample rate stays as the user set it — no interlock
        }

        private void cboIFType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _Parent.IFType = cboIFType.SelectedIndex;
            // Refresh bandwidth labels if needed; do not force sample rate
            FillAllBandwidths(lowIfOnly: false);
            FillAllSampleRates(preserveSelection: true);
        }

        private void numSampleRate_ValueChanged(object sender, EventArgs e)
        {

        }

        public void SR_IF_BW_OFF()
        {
            cboBandwidth.Enabled = false;
            cboIFType.Enabled = false;
        }

        public void SR_IF_BW_ON()
        {
            cboBandwidth.Enabled = true;
            cboIFType.Enabled = true;
        }

        private void numGR_ValueChanged(object sender, EventArgs e)
        {
            _Parent.GainReduction = (int)numGR.Value;
            numGRSlider.Value = (int)numGR.Maximum - (int)numGR.Value;
            Trace.WriteLine("Slider Value is " + numGRSlider.Value);
            Trace.WriteLine("Gain Reduction Value is " + numGR.Value);
        }

        private void numGRSlider_ValueChanged(object sender, EventArgs e)
        {
            numGR.Value = (int)numGR.Maximum - numGRSlider.Value;
            Trace.WriteLine("Gain Reduction Value is " + numGR.Value);
            Trace.WriteLine("Slider Value is " + numGRSlider.Value);
        }

        private void LNAGRSlider_Scroll(object sender, EventArgs e)
        {
            int maxLna = _Parent.MaxLnaState;
            LNAGRSlider.Minimum = 0;
            LNAGRSlider.Maximum = maxLna;
            int lna = Math.Max(0, Math.Min(maxLna, LNAGRSlider.Value));
            LNAGRThreshTB.Text = lna.ToString();
            _Parent.LNAState = lna;
        }

        public void LNAGRUpdate(int lnaState, int ignored = 0)
        {
            int maxLna = _Parent.MaxLnaState;
            LNAGRSlider.Minimum = 0;
            LNAGRSlider.Maximum = maxLna;
            int lna = Math.Max(0, Math.Min(maxLna, lnaState));
            LNAGRSlider.Value = lna;
            LNAGRThreshTB.Text = lna.ToString();
        }

        private void AGCEnabled_CheckedChanged(object sender, EventArgs e)
        {
            _Parent.AGCEnabled = AGCEnabled.Checked;
            AGCENState();
        }

        private void AGCENState()
        {
            if (AGCEnabled.Checked == true)
            {
                numGR.Enabled = false;
                numGRSlider.Enabled = false;
            }
            if (AGCEnabled.Checked == false)
            {
                numGR.Enabled = true;
                numGRSlider.Enabled = true;
            }
        }

        private void ADCsetpoint_ValueChanged(object sender, EventArgs e)
        {
            _Parent.AGCsetpoint = (int)ADCsetpoint.Value;
        }

        private void DefaultBtn_Click(object sender, EventArgs e)
        {
            cboIFType.SelectedIndex = 0;
            cboBandwidth.SelectedIndex = 3;
            ADCsetpoint.Value = -30;
            AGCEnabled.Checked = true;
            LNAState.Text = "LNA Off";
            LNAGRState.Text = "24";
            MixerStatus.Text = "Mixer On";
            BBGRState.Text = "36";
            numGR.Value = 60;
        }

        public void DefaultBtn_on()
        {
            DefaultBtn.Enabled = true;
        }

        public void DefaultBtn_off()
        {
            DefaultBtn.Enabled = false;
        }

        public void Defaults()
        {
            cboIFType.SelectedIndex = 0;
            cboBandwidth.SelectedIndex = 3;
            ADCsetpoint.Value = -15;
            AGCEnabled.Checked = true;
            LNAState.Text = "LNA Off";
            LNAGRState.Text = "24";
            MixerStatus.Text = "Mixer On";
            BBGRState.Text = "36";
            numGR.Value = 60;
        }
        
        public void InitaliseDisplayValues()
        {
            cboIFType.SelectedIndex = Math.Max(0, Math.Min(cboIFType.Items.Count - 1, _Parent.IFType));
            FillAllBandwidths(lowIfOnly: false);
            FillAllSampleRates(preserveSelection: false);
        }
                
        public void GRValues(long Frequency)
        {
            // API 3.x: IF gain reduction is always 20..59; LNA is independent 0..max
            numGR.Minimum = 20;
            numGR.Maximum = 59;
            numGRSlider.Minimum = 0;
            numGRSlider.Maximum = 39;

            int maxLna = _Parent.MaxLnaState;
            LNAGRSlider.Minimum = 0;
            LNAGRSlider.Maximum = maxLna;
            LNAGRSlider.TickFrequency = 1;

            LNAGRUpdate(_Parent.LNAState);
            int gr = _Parent.GainReduction;
            if (gr < 20) gr = 20;
            if (gr > 59) gr = 40;
            numGR.Value = gr;
            numGRSlider.Value = 59 - gr;
        }

        public void GRboxUpdate(int GRUpdate, long Frequency)
        {
            // Display current IF gain reduction (gRdB) and LNA state
            if (GRUpdate < 20) GRUpdate = 20;
            if (GRUpdate > 59) GRUpdate = 59;
            if (numGR.Value != GRUpdate)
                numGR.Value = GRUpdate;
            numGRSlider.Value = 59 - GRUpdate;

            BBGRState.Text = GRUpdate.ToString();
            LNAGRState.Text = _Parent.LNAState.ToString();
            LNAState.Text = _Parent.LNAState == 0 ? "LNA Max" : ("LNA " + _Parent.LNAState);
            MixerStatus.Text = "API 3.x";
            MXRGRState.Text = "-";
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void FQCorrection_ValueChanged(object sender, EventArgs e)
        {
            _Parent.FQPPM = (double)FQCorrection.Value;
        }
    }
}