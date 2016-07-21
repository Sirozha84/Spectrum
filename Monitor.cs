using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum
{
    public partial class Monitor : Form
    {
        public Monitor()
        {
            InitializeComponent();
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            labelPC.Text = Z80.PC.ToString();
            int adr = Z80.PC;
            listBox1.Items.Clear();
            do
            {
                string com = adr.ToString("00000 - ");
                int startadr = adr;
                string asm = Assembler.GetCommand(ref adr);
                while (asm.Length < 20) asm += " ";
                for (int i = startadr; i < adr; i++)
                    asm += Spectrum.Memory[i].ToString(" 000");
                listBox1.Items.Add(com + asm);
                
            } while (listBox1.Items.Count < 30);

            labelPC.Text = Z80.PC.ToString();
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            if (Spectrum.Mode == Spectrum.Modes.Normal)
            {
                Spectrum.Mode = Spectrum.Modes.Stop;
                buttonPause.Text = "|>";
            }
            else
            {
                Spectrum.Mode = Spectrum.Modes.Normal;
                buttonPause.Text = "[_]";
            }
        }

        private void buttonStep_Click(object sender, EventArgs e)
        {
            Spectrum.Mode = Spectrum.Modes.Step;
            buttonPause.Text = "|>";
        }

        private void buttonFrame_Click(object sender, EventArgs e)
        {
            Spectrum.Mode = Spectrum.Modes.Frame;
            buttonPause.Text = "|>";
        }

    }
}
