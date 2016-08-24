namespace Spectrum
{
    partial class Monitor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonPause = new System.Windows.Forms.Button();
            this.buttonStep = new System.Windows.Forms.Button();
            this.buttonFrame = new System.Windows.Forms.Button();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.checkBoxS = new System.Windows.Forms.CheckBox();
            this.checkBoxZ = new System.Windows.Forms.CheckBox();
            this.checkBoxY = new System.Windows.Forms.CheckBox();
            this.checkBoxH = new System.Windows.Forms.CheckBox();
            this.checkBoxC = new System.Windows.Forms.CheckBox();
            this.checkBoxN = new System.Windows.Forms.CheckBox();
            this.checkBoxP = new System.Windows.Forms.CheckBox();
            this.checkBoxX = new System.Windows.Forms.CheckBox();
            this.labelAF = new System.Windows.Forms.Label();
            this.textBoxAF = new System.Windows.Forms.TextBox();
            this.textBoxBC = new System.Windows.Forms.TextBox();
            this.labelBC = new System.Windows.Forms.Label();
            this.textBoxDE = new System.Windows.Forms.TextBox();
            this.labelDE = new System.Windows.Forms.Label();
            this.textBoxHL = new System.Windows.Forms.TextBox();
            this.labelHL = new System.Windows.Forms.Label();
            this.textBoxIX = new System.Windows.Forms.TextBox();
            this.labelIX = new System.Windows.Forms.Label();
            this.textBoxIY = new System.Windows.Forms.TextBox();
            this.labelIY = new System.Windows.Forms.Label();
            this.textBoxHLa = new System.Windows.Forms.TextBox();
            this.labelHLa = new System.Windows.Forms.Label();
            this.textBoxDEa = new System.Windows.Forms.TextBox();
            this.labelDEa = new System.Windows.Forms.Label();
            this.textBoxBCa = new System.Windows.Forms.TextBox();
            this.labelBCa = new System.Windows.Forms.Label();
            this.textBoxAFa = new System.Windows.Forms.TextBox();
            this.labelAFa = new System.Windows.Forms.Label();
            this.textBoxSP = new System.Windows.Forms.TextBox();
            this.labelSP = new System.Windows.Forms.Label();
            this.textBoxPC = new System.Windows.Forms.TextBox();
            this.labelPC = new System.Windows.Forms.Label();
            this.textBoxR = new System.Windows.Forms.TextBox();
            this.labelR = new System.Windows.Forms.Label();
            this.textBoxI = new System.Windows.Forms.TextBox();
            this.labelI = new System.Windows.Forms.Label();
            this.buttonReset = new System.Windows.Forms.Button();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.textBoxA = new System.Windows.Forms.TextBox();
            this.labelA = new System.Windows.Forms.Label();
            this.labelFlags = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxInt = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonPause
            // 
            this.buttonPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPause.Location = new System.Drawing.Point(322, 12);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(72, 47);
            this.buttonPause.TabIndex = 1;
            this.buttonPause.Text = "[_]";
            this.buttonPause.UseVisualStyleBackColor = true;
            this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
            // 
            // buttonStep
            // 
            this.buttonStep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStep.Location = new System.Drawing.Point(400, 12);
            this.buttonStep.Name = "buttonStep";
            this.buttonStep.Size = new System.Drawing.Size(69, 47);
            this.buttonStep.TabIndex = 2;
            this.buttonStep.Text = "| |>";
            this.buttonStep.UseVisualStyleBackColor = true;
            this.buttonStep.Click += new System.EventHandler(this.buttonStep_Click);
            // 
            // buttonFrame
            // 
            this.buttonFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFrame.Location = new System.Drawing.Point(475, 12);
            this.buttonFrame.Name = "buttonFrame";
            this.buttonFrame.Size = new System.Drawing.Size(72, 47);
            this.buttonFrame.TabIndex = 3;
            this.buttonFrame.Text = "|>|";
            this.buttonFrame.UseVisualStyleBackColor = true;
            this.buttonFrame.Click += new System.EventHandler(this.buttonFrame_Click);
            // 
            // timerRefresh
            // 
            this.timerRefresh.Enabled = true;
            this.timerRefresh.Interval = 20;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 11;
            this.listBox1.Location = new System.Drawing.Point(13, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(303, 488);
            this.listBox1.TabIndex = 5;
            // 
            // checkBoxS
            // 
            this.checkBoxS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxS.AutoSize = true;
            this.checkBoxS.Location = new System.Drawing.Point(322, 89);
            this.checkBoxS.Name = "checkBoxS";
            this.checkBoxS.Size = new System.Drawing.Size(15, 14);
            this.checkBoxS.TabIndex = 6;
            this.checkBoxS.UseVisualStyleBackColor = true;
            // 
            // checkBoxZ
            // 
            this.checkBoxZ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxZ.AutoSize = true;
            this.checkBoxZ.Location = new System.Drawing.Point(341, 89);
            this.checkBoxZ.Name = "checkBoxZ";
            this.checkBoxZ.Size = new System.Drawing.Size(15, 14);
            this.checkBoxZ.TabIndex = 7;
            this.checkBoxZ.UseVisualStyleBackColor = true;
            // 
            // checkBoxY
            // 
            this.checkBoxY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxY.AutoSize = true;
            this.checkBoxY.Location = new System.Drawing.Point(360, 89);
            this.checkBoxY.Name = "checkBoxY";
            this.checkBoxY.Size = new System.Drawing.Size(15, 14);
            this.checkBoxY.TabIndex = 8;
            this.checkBoxY.UseVisualStyleBackColor = true;
            // 
            // checkBoxH
            // 
            this.checkBoxH.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxH.AutoSize = true;
            this.checkBoxH.Location = new System.Drawing.Point(379, 89);
            this.checkBoxH.Name = "checkBoxH";
            this.checkBoxH.Size = new System.Drawing.Size(15, 14);
            this.checkBoxH.TabIndex = 9;
            this.checkBoxH.UseVisualStyleBackColor = true;
            // 
            // checkBoxC
            // 
            this.checkBoxC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxC.AutoSize = true;
            this.checkBoxC.Location = new System.Drawing.Point(456, 89);
            this.checkBoxC.Name = "checkBoxC";
            this.checkBoxC.Size = new System.Drawing.Size(15, 14);
            this.checkBoxC.TabIndex = 13;
            this.checkBoxC.UseVisualStyleBackColor = true;
            // 
            // checkBoxN
            // 
            this.checkBoxN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxN.AutoSize = true;
            this.checkBoxN.Location = new System.Drawing.Point(437, 89);
            this.checkBoxN.Name = "checkBoxN";
            this.checkBoxN.Size = new System.Drawing.Size(15, 14);
            this.checkBoxN.TabIndex = 12;
            this.checkBoxN.UseVisualStyleBackColor = true;
            // 
            // checkBoxP
            // 
            this.checkBoxP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxP.AutoSize = true;
            this.checkBoxP.Location = new System.Drawing.Point(418, 89);
            this.checkBoxP.Name = "checkBoxP";
            this.checkBoxP.Size = new System.Drawing.Size(15, 14);
            this.checkBoxP.TabIndex = 11;
            this.checkBoxP.UseVisualStyleBackColor = true;
            // 
            // checkBoxX
            // 
            this.checkBoxX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxX.AutoSize = true;
            this.checkBoxX.Location = new System.Drawing.Point(398, 89);
            this.checkBoxX.Name = "checkBoxX";
            this.checkBoxX.Size = new System.Drawing.Size(15, 14);
            this.checkBoxX.TabIndex = 10;
            this.checkBoxX.UseVisualStyleBackColor = true;
            // 
            // labelAF
            // 
            this.labelAF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAF.AutoSize = true;
            this.labelAF.Location = new System.Drawing.Point(322, 113);
            this.labelAF.Name = "labelAF";
            this.labelAF.Size = new System.Drawing.Size(20, 13);
            this.labelAF.TabIndex = 14;
            this.labelAF.Text = "AF";
            // 
            // textBoxAF
            // 
            this.textBoxAF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAF.Location = new System.Drawing.Point(348, 110);
            this.textBoxAF.Name = "textBoxAF";
            this.textBoxAF.Size = new System.Drawing.Size(65, 20);
            this.textBoxAF.TabIndex = 15;
            // 
            // textBoxBC
            // 
            this.textBoxBC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBC.Location = new System.Drawing.Point(348, 136);
            this.textBoxBC.Name = "textBoxBC";
            this.textBoxBC.Size = new System.Drawing.Size(65, 20);
            this.textBoxBC.TabIndex = 17;
            // 
            // labelBC
            // 
            this.labelBC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBC.AutoSize = true;
            this.labelBC.Location = new System.Drawing.Point(322, 139);
            this.labelBC.Name = "labelBC";
            this.labelBC.Size = new System.Drawing.Size(21, 13);
            this.labelBC.TabIndex = 16;
            this.labelBC.Text = "BC";
            // 
            // textBoxDE
            // 
            this.textBoxDE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDE.Location = new System.Drawing.Point(348, 162);
            this.textBoxDE.Name = "textBoxDE";
            this.textBoxDE.Size = new System.Drawing.Size(65, 20);
            this.textBoxDE.TabIndex = 19;
            // 
            // labelDE
            // 
            this.labelDE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDE.AutoSize = true;
            this.labelDE.Location = new System.Drawing.Point(322, 165);
            this.labelDE.Name = "labelDE";
            this.labelDE.Size = new System.Drawing.Size(22, 13);
            this.labelDE.TabIndex = 18;
            this.labelDE.Text = "DE";
            // 
            // textBoxHL
            // 
            this.textBoxHL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxHL.Location = new System.Drawing.Point(348, 188);
            this.textBoxHL.Name = "textBoxHL";
            this.textBoxHL.Size = new System.Drawing.Size(65, 20);
            this.textBoxHL.TabIndex = 21;
            // 
            // labelHL
            // 
            this.labelHL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHL.AutoSize = true;
            this.labelHL.Location = new System.Drawing.Point(322, 191);
            this.labelHL.Name = "labelHL";
            this.labelHL.Size = new System.Drawing.Size(21, 13);
            this.labelHL.TabIndex = 20;
            this.labelHL.Text = "HL";
            // 
            // textBoxIX
            // 
            this.textBoxIX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxIX.Location = new System.Drawing.Point(348, 214);
            this.textBoxIX.Name = "textBoxIX";
            this.textBoxIX.Size = new System.Drawing.Size(65, 20);
            this.textBoxIX.TabIndex = 23;
            // 
            // labelIX
            // 
            this.labelIX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelIX.AutoSize = true;
            this.labelIX.Location = new System.Drawing.Point(322, 217);
            this.labelIX.Name = "labelIX";
            this.labelIX.Size = new System.Drawing.Size(17, 13);
            this.labelIX.TabIndex = 22;
            this.labelIX.Text = "IX";
            // 
            // textBoxIY
            // 
            this.textBoxIY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxIY.Location = new System.Drawing.Point(451, 214);
            this.textBoxIY.Name = "textBoxIY";
            this.textBoxIY.Size = new System.Drawing.Size(65, 20);
            this.textBoxIY.TabIndex = 33;
            // 
            // labelIY
            // 
            this.labelIY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelIY.AutoSize = true;
            this.labelIY.Location = new System.Drawing.Point(425, 217);
            this.labelIY.Name = "labelIY";
            this.labelIY.Size = new System.Drawing.Size(17, 13);
            this.labelIY.TabIndex = 32;
            this.labelIY.Text = "IY";
            // 
            // textBoxHLa
            // 
            this.textBoxHLa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxHLa.Location = new System.Drawing.Point(451, 188);
            this.textBoxHLa.Name = "textBoxHLa";
            this.textBoxHLa.Size = new System.Drawing.Size(65, 20);
            this.textBoxHLa.TabIndex = 31;
            // 
            // labelHLa
            // 
            this.labelHLa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHLa.AutoSize = true;
            this.labelHLa.Location = new System.Drawing.Point(425, 191);
            this.labelHLa.Name = "labelHLa";
            this.labelHLa.Size = new System.Drawing.Size(23, 13);
            this.labelHLa.TabIndex = 30;
            this.labelHLa.Text = "HL\'";
            // 
            // textBoxDEa
            // 
            this.textBoxDEa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDEa.Location = new System.Drawing.Point(451, 162);
            this.textBoxDEa.Name = "textBoxDEa";
            this.textBoxDEa.Size = new System.Drawing.Size(65, 20);
            this.textBoxDEa.TabIndex = 29;
            // 
            // labelDEa
            // 
            this.labelDEa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDEa.AutoSize = true;
            this.labelDEa.Location = new System.Drawing.Point(425, 165);
            this.labelDEa.Name = "labelDEa";
            this.labelDEa.Size = new System.Drawing.Size(24, 13);
            this.labelDEa.TabIndex = 28;
            this.labelDEa.Text = "DE\'";
            // 
            // textBoxBCa
            // 
            this.textBoxBCa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBCa.Location = new System.Drawing.Point(451, 136);
            this.textBoxBCa.Name = "textBoxBCa";
            this.textBoxBCa.Size = new System.Drawing.Size(65, 20);
            this.textBoxBCa.TabIndex = 27;
            // 
            // labelBCa
            // 
            this.labelBCa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBCa.AutoSize = true;
            this.labelBCa.Location = new System.Drawing.Point(425, 139);
            this.labelBCa.Name = "labelBCa";
            this.labelBCa.Size = new System.Drawing.Size(23, 13);
            this.labelBCa.TabIndex = 26;
            this.labelBCa.Text = "BC\'";
            // 
            // textBoxAFa
            // 
            this.textBoxAFa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAFa.Location = new System.Drawing.Point(451, 110);
            this.textBoxAFa.Name = "textBoxAFa";
            this.textBoxAFa.Size = new System.Drawing.Size(65, 20);
            this.textBoxAFa.TabIndex = 25;
            // 
            // labelAFa
            // 
            this.labelAFa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAFa.AutoSize = true;
            this.labelAFa.Location = new System.Drawing.Point(425, 113);
            this.labelAFa.Name = "labelAFa";
            this.labelAFa.Size = new System.Drawing.Size(22, 13);
            this.labelAFa.TabIndex = 24;
            this.labelAFa.Text = "AF\'";
            // 
            // textBoxSP
            // 
            this.textBoxSP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSP.Location = new System.Drawing.Point(348, 266);
            this.textBoxSP.Name = "textBoxSP";
            this.textBoxSP.Size = new System.Drawing.Size(65, 20);
            this.textBoxSP.TabIndex = 37;
            // 
            // labelSP
            // 
            this.labelSP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSP.AutoSize = true;
            this.labelSP.Location = new System.Drawing.Point(322, 269);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(21, 13);
            this.labelSP.TabIndex = 36;
            this.labelSP.Text = "SP";
            // 
            // textBoxPC
            // 
            this.textBoxPC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPC.Location = new System.Drawing.Point(348, 240);
            this.textBoxPC.Name = "textBoxPC";
            this.textBoxPC.Size = new System.Drawing.Size(65, 20);
            this.textBoxPC.TabIndex = 35;
            // 
            // labelPC
            // 
            this.labelPC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPC.AutoSize = true;
            this.labelPC.Location = new System.Drawing.Point(322, 243);
            this.labelPC.Name = "labelPC";
            this.labelPC.Size = new System.Drawing.Size(21, 13);
            this.labelPC.TabIndex = 34;
            this.labelPC.Text = "PC";
            // 
            // textBoxR
            // 
            this.textBoxR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxR.Location = new System.Drawing.Point(451, 266);
            this.textBoxR.Name = "textBoxR";
            this.textBoxR.Size = new System.Drawing.Size(43, 20);
            this.textBoxR.TabIndex = 41;
            // 
            // labelR
            // 
            this.labelR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelR.AutoSize = true;
            this.labelR.Location = new System.Drawing.Point(425, 269);
            this.labelR.Name = "labelR";
            this.labelR.Size = new System.Drawing.Size(15, 13);
            this.labelR.TabIndex = 40;
            this.labelR.Text = "R";
            // 
            // textBoxI
            // 
            this.textBoxI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxI.Location = new System.Drawing.Point(451, 240);
            this.textBoxI.Name = "textBoxI";
            this.textBoxI.Size = new System.Drawing.Size(43, 20);
            this.textBoxI.TabIndex = 39;
            // 
            // labelI
            // 
            this.labelI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelI.AutoSize = true;
            this.labelI.Location = new System.Drawing.Point(425, 243);
            this.labelI.Name = "labelI";
            this.labelI.Size = new System.Drawing.Size(10, 13);
            this.labelI.TabIndex = 38;
            this.labelI.Text = "I";
            // 
            // buttonReset
            // 
            this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReset.Location = new System.Drawing.Point(475, 458);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(72, 47);
            this.buttonReset.TabIndex = 42;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(322, 292);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(225, 160);
            this.listBox2.TabIndex = 43;
            // 
            // textBoxA
            // 
            this.textBoxA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxA.Location = new System.Drawing.Point(348, 458);
            this.textBoxA.Name = "textBoxA";
            this.textBoxA.Size = new System.Drawing.Size(43, 20);
            this.textBoxA.TabIndex = 45;
            // 
            // labelA
            // 
            this.labelA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelA.AutoSize = true;
            this.labelA.Location = new System.Drawing.Point(322, 461);
            this.labelA.Name = "labelA";
            this.labelA.Size = new System.Drawing.Size(14, 13);
            this.labelA.TabIndex = 44;
            this.labelA.Text = "A";
            // 
            // labelFlags
            // 
            this.labelFlags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFlags.AutoSize = true;
            this.labelFlags.Location = new System.Drawing.Point(322, 71);
            this.labelFlags.Name = "labelFlags";
            this.labelFlags.Size = new System.Drawing.Size(147, 13);
            this.labelFlags.TabIndex = 46;
            this.labelFlags.Text = "S    Z    5    H    3    V    N    C";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(528, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 13);
            this.label1.TabIndex = 47;
            this.label1.Text = "Int";
            // 
            // checkBoxInt
            // 
            this.checkBoxInt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxInt.AutoSize = true;
            this.checkBoxInt.Location = new System.Drawing.Point(531, 89);
            this.checkBoxInt.Name = "checkBoxInt";
            this.checkBoxInt.Size = new System.Drawing.Size(15, 14);
            this.checkBoxInt.TabIndex = 48;
            this.checkBoxInt.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(325, 482);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(66, 23);
            this.button1.TabIndex = 49;
            this.button1.Text = "Set";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Monitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 517);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBoxInt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelFlags);
            this.Controls.Add(this.textBoxA);
            this.Controls.Add(this.labelA);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.textBoxR);
            this.Controls.Add(this.labelR);
            this.Controls.Add(this.textBoxI);
            this.Controls.Add(this.labelI);
            this.Controls.Add(this.textBoxSP);
            this.Controls.Add(this.labelSP);
            this.Controls.Add(this.textBoxPC);
            this.Controls.Add(this.labelPC);
            this.Controls.Add(this.textBoxIY);
            this.Controls.Add(this.labelIY);
            this.Controls.Add(this.textBoxHLa);
            this.Controls.Add(this.labelHLa);
            this.Controls.Add(this.textBoxDEa);
            this.Controls.Add(this.labelDEa);
            this.Controls.Add(this.textBoxBCa);
            this.Controls.Add(this.labelBCa);
            this.Controls.Add(this.textBoxAFa);
            this.Controls.Add(this.labelAFa);
            this.Controls.Add(this.textBoxIX);
            this.Controls.Add(this.labelIX);
            this.Controls.Add(this.textBoxHL);
            this.Controls.Add(this.labelHL);
            this.Controls.Add(this.textBoxDE);
            this.Controls.Add(this.labelDE);
            this.Controls.Add(this.textBoxBC);
            this.Controls.Add(this.labelBC);
            this.Controls.Add(this.textBoxAF);
            this.Controls.Add(this.labelAF);
            this.Controls.Add(this.checkBoxC);
            this.Controls.Add(this.checkBoxN);
            this.Controls.Add(this.checkBoxP);
            this.Controls.Add(this.checkBoxX);
            this.Controls.Add(this.checkBoxH);
            this.Controls.Add(this.checkBoxY);
            this.Controls.Add(this.checkBoxZ);
            this.Controls.Add(this.checkBoxS);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.buttonFrame);
            this.Controls.Add(this.buttonStep);
            this.Controls.Add(this.buttonPause);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Monitor";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Monitor";
            this.Load += new System.EventHandler(this.Monitor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonPause;
        private System.Windows.Forms.Button buttonStep;
        private System.Windows.Forms.Button buttonFrame;
        private System.Windows.Forms.Timer timerRefresh;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.CheckBox checkBoxS;
        private System.Windows.Forms.CheckBox checkBoxZ;
        private System.Windows.Forms.CheckBox checkBoxY;
        private System.Windows.Forms.CheckBox checkBoxH;
        private System.Windows.Forms.CheckBox checkBoxC;
        private System.Windows.Forms.CheckBox checkBoxN;
        private System.Windows.Forms.CheckBox checkBoxP;
        private System.Windows.Forms.CheckBox checkBoxX;
        private System.Windows.Forms.Label labelAF;
        private System.Windows.Forms.TextBox textBoxAF;
        private System.Windows.Forms.TextBox textBoxBC;
        private System.Windows.Forms.Label labelBC;
        private System.Windows.Forms.TextBox textBoxDE;
        private System.Windows.Forms.Label labelDE;
        private System.Windows.Forms.TextBox textBoxHL;
        private System.Windows.Forms.Label labelHL;
        private System.Windows.Forms.TextBox textBoxIX;
        private System.Windows.Forms.Label labelIX;
        private System.Windows.Forms.TextBox textBoxIY;
        private System.Windows.Forms.Label labelIY;
        private System.Windows.Forms.TextBox textBoxHLa;
        private System.Windows.Forms.Label labelHLa;
        private System.Windows.Forms.TextBox textBoxDEa;
        private System.Windows.Forms.Label labelDEa;
        private System.Windows.Forms.TextBox textBoxBCa;
        private System.Windows.Forms.Label labelBCa;
        private System.Windows.Forms.TextBox textBoxAFa;
        private System.Windows.Forms.Label labelAFa;
        private System.Windows.Forms.TextBox textBoxSP;
        private System.Windows.Forms.Label labelSP;
        private System.Windows.Forms.TextBox textBoxPC;
        private System.Windows.Forms.Label labelPC;
        private System.Windows.Forms.TextBox textBoxR;
        private System.Windows.Forms.Label labelR;
        private System.Windows.Forms.TextBox textBoxI;
        private System.Windows.Forms.Label labelI;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.TextBox textBoxA;
        private System.Windows.Forms.Label labelA;
        private System.Windows.Forms.Label labelFlags;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxInt;
        private System.Windows.Forms.Button button1;
    }
}