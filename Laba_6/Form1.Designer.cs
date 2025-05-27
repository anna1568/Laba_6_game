namespace Laba_6
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            picDisplay = new PictureBox();
            timer1 = new System.Windows.Forms.Timer(components);
            trackBarParticleCount = new TrackBar();
            trackBarParticleSpeed = new TrackBar();
            ((System.ComponentModel.ISupportInitialize)picDisplay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarParticleCount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarParticleSpeed).BeginInit();
            SuspendLayout();
            // 
            // picDisplay
            // 
            picDisplay.Location = new Point(12, -4);
            picDisplay.Name = "picDisplay";
            picDisplay.Size = new Size(544, 342);
            picDisplay.TabIndex = 0;
            picDisplay.TabStop = false;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 40;
            timer1.Tick += Timer1_Tick;
            // 
            // trackBarParticleCount
            // 
            trackBarParticleCount.Location = new Point(562, 12);
            trackBarParticleCount.Name = "trackBarParticleCount";
            trackBarParticleCount.Size = new Size(130, 56);
            trackBarParticleCount.TabIndex = 1;
            // 
            // trackBarParticleSpeed
            // 
            trackBarParticleSpeed.Location = new Point(562, 74);
            trackBarParticleSpeed.Name = "trackBarParticleSpeed";
            trackBarParticleSpeed.Size = new Size(130, 56);
            trackBarParticleSpeed.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(889, 351);
            Controls.Add(trackBarParticleSpeed);
            Controls.Add(trackBarParticleCount);
            Controls.Add(picDisplay);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)picDisplay).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarParticleCount).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarParticleSpeed).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picDisplay;
        private System.Windows.Forms.Timer timer1;
        private TrackBar trackBarParticleCount;
        private TrackBar trackBarParticleSpeed;
    }
}
