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
            ushort adr = Z80.PC;
            listBox1.Items.Clear();
            do
            {
                
                string b = Z80.Be[adr] ? "|" : " ";
                string com = adr.ToString("00000" + b + "- ");
                ushort startadr = adr;
                string asm = Assembler.GetCommand(ref adr);
                while (asm.Length < 15) asm += " ";
                for (ushort i = startadr; i < adr; i++)
                    asm += Z80.RAM[i].ToString(" 000");
                listBox1.Items.Add(com + asm);
                //Отступ если переход
                if (listBox1.Items.Count < 44)
                    switch (Z80.RAM[startadr])
                    {
                        case 16: case 24: case 32: case 40: case 48: case 56:
                        case 195:
                        case 200: case 201: case 208: case 233:
                            listBox1.Items.Add("");
                            break;
                    }
            } while (listBox1.Items.Count < 44);
            checkBoxS.Checked = Z80.fS;
            checkBoxZ.Checked = Z80.fZ;
            checkBoxY.Checked = Z80.f5;
            checkBoxH.Checked = Z80.fH;
            checkBoxX.Checked = Z80.f3;
            checkBoxP.Checked = Z80.fV;
            checkBoxN.Checked = Z80.fN;
            checkBoxC.Checked = Z80.fC;
            textBoxAF.Text = (Z80.A * 256 + Z80.F).ToString();
            textBoxA.Text = Z80.A.ToString(); //Потому-что задолбало...
            textBoxAFa.Text = (Z80.Aa * 256 + Z80.Fa).ToString();
            textBoxBC.Text = (Z80.B * 256 + Z80.C).ToString();
            textBoxBCa.Text = (Z80.Ba * 256 + Z80.Ca).ToString();
            textBoxDE.Text = (Z80.D * 256 + Z80.E).ToString();
            textBoxDEa.Text = (Z80.Da * 256 + Z80.Ea).ToString();
            textBoxHL.Text = (Z80.H * 256 + Z80.L).ToString();
            textBoxHLa.Text = (Z80.Ha * 256 + Z80.La).ToString();
            textBoxIX.Text = Z80.IX.ToString();
            textBoxIY.Text = Z80.IY.ToString();
            textBoxPC.Text = Z80.PC.ToString();
            textBoxSP.Text = Z80.SP.ToString();
            textBoxI.Text = Z80.I.ToString();
            textBoxR.Text = Z80.R.ToString();
            checkBoxInt.Checked = Z80.IM > 0;
            //Немножко кода в просмор памяти
            listBox2.Items.Clear();
            for (int i = 0; i < 12; i++)
                listBox2.Items.Add(i + Spectrum.View + " - " + Z80.RAM[i + Spectrum.View]);
            //Кнопочки
            PauseButtonRefresh();
        }

        void PauseButtonRefresh()
        {
            buttonPause.Text = Spectrum.Mode == Spectrum.Modes.Normal ? "[_]" : "|>";
            timerRefresh.Enabled = Spectrum.Mode != Spectrum.Modes.Stop;
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            if (Spectrum.Mode == Spectrum.Modes.Normal)
                Spectrum.Mode = Spectrum.Modes.Stop;
            else
            {
                Spectrum.Mode = Spectrum.Modes.Normal;
                Spectrum.BreakPoint = -1;
            }
            PauseButtonRefresh();
        }

        private void buttonStep_Click(object sender, EventArgs e)
        {
            Spectrum.Mode = Spectrum.Modes.Step;
            //buttonPause.Text = "|>";
            timerRefresh_Tick(null, null);
        }

        private void buttonFrame_Click(object sender, EventArgs e)
        {
            Spectrum.Mode = Spectrum.Modes.Frame;
            buttonPause.Text = "|>";
            timerRefresh_Tick(null, null);
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            //Spectrum.Init();
            Z80.Reset();
            Spectrum.Mode = Spectrum.Modes.Normal;
            timerRefresh_Tick(null, null);
        }

        private void Monitor_Load(object sender, EventArgs e)
        {
            PauseButtonRefresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Z80.A = Convert.ToByte(textBoxA.Text);
        }
    }
}
