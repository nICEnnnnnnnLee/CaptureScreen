namespace CaptureScreen
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbMouseX = new System.Windows.Forms.Label();
            this.lbMouseY = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbMouseStatus = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipText = "Ctrl + Alt + Shift + Space 截屏; \r\n双击图标退出；截图保存在同级文件夹下";
            this.notifyIcon1.BalloonTipTitle = "看这里！=，=";
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Ctrl + Alt + Shift + Space 截屏;\r\n 双击图标退出";
            this.notifyIcon1.Visible = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(84, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "鼠标x";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(84, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "鼠标y";
            // 
            // lbMouseX
            // 
            this.lbMouseX.AutoSize = true;
            this.lbMouseX.Location = new System.Drawing.Point(147, 51);
            this.lbMouseX.Name = "lbMouseX";
            this.lbMouseX.Size = new System.Drawing.Size(0, 12);
            this.lbMouseX.TabIndex = 2;
            // 
            // lbMouseY
            // 
            this.lbMouseY.AutoSize = true;
            this.lbMouseY.Location = new System.Drawing.Point(149, 95);
            this.lbMouseY.Name = "lbMouseY";
            this.lbMouseY.Size = new System.Drawing.Size(0, 12);
            this.lbMouseY.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(86, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "状态";
            // 
            // lbMouseStatus
            // 
            this.lbMouseStatus.AutoSize = true;
            this.lbMouseStatus.Location = new System.Drawing.Point(151, 148);
            this.lbMouseStatus.Name = "lbMouseStatus";
            this.lbMouseStatus.Size = new System.Drawing.Size(41, 12);
            this.lbMouseStatus.TabIndex = 5;
            this.lbMouseStatus.Text = "label4";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "JPG文件|*.jpg|BMP文件|*.bmp";
            this.saveFileDialog1.RestoreDirectory = true;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 282);
            this.Controls.Add(this.lbMouseStatus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbMouseY);
            this.Controls.Add(this.lbMouseX);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbMouseX;
        private System.Windows.Forms.Label lbMouseY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbMouseStatus;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Timer timer1;
    }
}

