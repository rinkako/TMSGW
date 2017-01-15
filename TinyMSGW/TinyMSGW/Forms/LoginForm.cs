﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TinyMSGW.Adapter;

namespace TinyMSGW.Forms
{
    /// <summary>
    /// 窗体：登录
    /// </summary>
    public partial class LoginForm : Form
    {
        /// <summary>
        /// 构造器
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 按钮：取消
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            this.adapter.Terminate();
        }

        /// <summary>
        /// 按钮：登录
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            // 登录验证
            bool allowFlag;
            bool linkFlag = this.adapter.LoginValid(this.textBox1.Text, Utils.CommonUtil.EncryptToSHA1(this.textBox2.Text), out allowFlag);
            if (linkFlag == false)
            {
                MessageBox.Show("无法连接到检验服务器或本地检验文件失效", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (allowFlag == false)
            {
                MessageBox.Show("用户名或密码不正确", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            // 登陆成功
            this.adapter.LoginSuccess(this.textBox1.Text);
            // 跳转到主页面
            Forms.MainForm mf = new MainForm();
            mf.Show(this);
            this.Hide();
        }

        /// <summary>
        /// 适配器
        /// </summary>
        IActionAdapter adapter = AdapterFactory.GetAdapter();
    }
}