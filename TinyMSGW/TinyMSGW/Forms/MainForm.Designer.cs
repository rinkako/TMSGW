namespace TinyMSGW.Forms
{
    partial class MainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button12 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button16 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.button20 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button13 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button25 = new System.Windows.Forms.Button();
            this.button21 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "欢迎你，[user]：";
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.Location = new System.Drawing.Point(15, 36);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(208, 94);
            this.button1.TabIndex = 1;
            this.button1.Text = "查询馆藏图书";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.AutoSize = true;
            this.button2.Location = new System.Drawing.Point(229, 36);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(208, 94);
            this.button2.TabIndex = 2;
            this.button2.Text = "自助还书";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.AutoSize = true;
            this.button3.Location = new System.Drawing.Point(15, 136);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(208, 94);
            this.button3.TabIndex = 3;
            this.button3.Text = "管理我的借书卡";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.AutoSize = true;
            this.button4.Location = new System.Drawing.Point(229, 136);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(208, 94);
            this.button4.TabIndex = 4;
            this.button4.Text = "退出登录";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button12);
            this.groupBox1.Controls.Add(this.button11);
            this.groupBox1.Controls.Add(this.button10);
            this.groupBox1.Controls.Add(this.button9);
            this.groupBox1.Controls.Add(this.button8);
            this.groupBox1.Controls.Add(this.button7);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Location = new System.Drawing.Point(15, 246);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(422, 100);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "柜员功能";
            this.groupBox1.Visible = false;
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(317, 57);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(97, 31);
            this.button12.TabIndex = 7;
            this.button12.Text = "收缴赔偿金";
            this.button12.UseVisualStyleBackColor = true;
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(317, 20);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(97, 31);
            this.button11.TabIndex = 6;
            this.button11.Text = "收缴滞纳金";
            this.button11.UseVisualStyleBackColor = true;
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(214, 57);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(97, 31);
            this.button10.TabIndex = 5;
            this.button10.Text = "注销借书卡";
            this.button10.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(214, 20);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(97, 31);
            this.button9.TabIndex = 4;
            this.button9.Text = "办理借书卡";
            this.button9.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(111, 57);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(97, 31);
            this.button8.TabIndex = 3;
            this.button8.Text = "修改图书信息";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(111, 20);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(97, 31);
            this.button7.TabIndex = 2;
            this.button7.Text = "查询图书信息";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(6, 57);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(97, 31);
            this.button6.TabIndex = 1;
            this.button6.Text = "下架图书";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(6, 20);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(97, 31);
            this.button5.TabIndex = 0;
            this.button5.Text = "上架图书";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button16);
            this.groupBox2.Controls.Add(this.button17);
            this.groupBox2.Controls.Add(this.button18);
            this.groupBox2.Controls.Add(this.button19);
            this.groupBox2.Controls.Add(this.button20);
            this.groupBox2.Location = new System.Drawing.Point(15, 352);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(422, 100);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "库管功能";
            this.groupBox2.Visible = false;
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(317, 20);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(97, 68);
            this.button16.TabIndex = 4;
            this.button16.Text = "查看所有库存";
            this.button16.UseVisualStyleBackColor = true;
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(162, 57);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(97, 31);
            this.button17.TabIndex = 3;
            this.button17.Text = "扔掉";
            this.button17.UseVisualStyleBackColor = true;
            // 
            // button18
            // 
            this.button18.Location = new System.Drawing.Point(162, 20);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(97, 31);
            this.button18.TabIndex = 2;
            this.button18.Text = "修改库存信息";
            this.button18.UseVisualStyleBackColor = true;
            // 
            // button19
            // 
            this.button19.Location = new System.Drawing.Point(6, 57);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(97, 31);
            this.button19.TabIndex = 1;
            this.button19.Text = "出库";
            this.button19.UseVisualStyleBackColor = true;
            // 
            // button20
            // 
            this.button20.Location = new System.Drawing.Point(6, 20);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(97, 31);
            this.button20.TabIndex = 0;
            this.button20.Text = "入库";
            this.button20.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button13);
            this.groupBox3.Controls.Add(this.button15);
            this.groupBox3.Controls.Add(this.button25);
            this.groupBox3.Controls.Add(this.button21);
            this.groupBox3.Location = new System.Drawing.Point(15, 458);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(422, 62);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "管理员功能";
            this.groupBox3.Visible = false;
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(317, 20);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(97, 31);
            this.button13.TabIndex = 15;
            this.button13.Text = "系统设置";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(214, 20);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(97, 31);
            this.button15.TabIndex = 13;
            this.button15.Text = "修改借书期限";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // button25
            // 
            this.button25.Location = new System.Drawing.Point(6, 20);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(97, 31);
            this.button25.TabIndex = 8;
            this.button25.Text = "用户管理";
            this.button25.UseVisualStyleBackColor = true;
            this.button25.Click += new System.EventHandler(this.button25_Click);
            // 
            // button21
            // 
            this.button21.Location = new System.Drawing.Point(111, 20);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(97, 31);
            this.button21.TabIndex = 12;
            this.button21.Text = "修改滞纳金";
            this.button21.UseVisualStyleBackColor = true;
            this.button21.Click += new System.EventHandler(this.button21_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 528);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tiny图书馆管理系统";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.Button button20;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button25;
        private System.Windows.Forms.Button button21;
    }
}