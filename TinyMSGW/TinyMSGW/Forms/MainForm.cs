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
    /// 窗体：主窗体
    /// </summary>
    public partial class  MainForm : Form
    {
        /// <summary>
        /// 适配器
        /// </summary>
        private IActionAdapter adapter = AdapterFactory.GetAdapter();

        /// <summary>
        /// 构造器
        /// </summary>
        public MainForm()
        {
            

            // DEBUG
            //GlobalDataPackage.CurrentUser = new Entity.User()
            //{
            //    UserName = "MyAdmin",
            //    Type = Enums.UserType.Admin
            //};

            InitializeComponent();
            
            
            // 根据用户的权限修改窗体控件的可访问性和可视效果
            this.label1.Text = String.Format("欢迎你，{0}：", GlobalDataPackage.CurrentUser.UserName, 
                GlobalDataPackage.CurrentUser.Type == Enums.UserType.Customer ? "" : " (" +
                Utils.CommonUtil.ParseUserETypeToCString(GlobalDataPackage.CurrentUser.Type) + ")");
            switch (GlobalDataPackage.CurrentUser.Type)
            {
                case Enums.UserType.Librarian:
                    this.groupBox1.Visible = true;
                    this.groupBox2.Visible = false;
                    this.Height = 395;
                    break;
                case Enums.UserType.Keeper:
                    this.groupBox2.Location = this.groupBox1.Location;
                    this.groupBox1.Visible = false;
                    this.groupBox2.Visible = true;
                    this.Height = 395;
                    break;
                case Enums.UserType.Admin:
                    this.groupBox1.Visible = this.groupBox2.Visible = this.groupBox3.Visible = true;
                    break;
                default:
                    this.Height = 290;
                    break;
            }
        }

        /// <summary>
        /// 按钮：查询馆藏图书
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            RetrieveBookForm rbf = new RetrieveBookForm();
            rbf.Show(this);
            this.Hide();
        }

        /// <summary>
        /// 按钮：退出登录
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            // 后台退出登录
            this.adapter.Logout();
            // 切换到登录窗体
            this.Owner.Show();
            this.Close();
        }

        /// <summary>
        /// 按钮：管理用户
        /// </summary>
        private void button25_Click(object sender, EventArgs e)
        {
            // 切换窗体
            UserManageForm umf = new UserManageForm();
            umf.Show(this);
            this.Hide();
        }

        /// <summary>
        /// 按钮：修改滞纳金
        /// </summary>
        private void button21_Click(object sender, EventArgs e)
        {
            DelayFeeSettingForm dfsf = new DelayFeeSettingForm();
            dfsf.ShowDialog(this);
        }

        /// <summary>
        /// 按钮：修改归还最长期限
        /// </summary>
        private void button15_Click(object sender, EventArgs e)
        {
            ReturnDaySettingForm rdsf = new ReturnDaySettingForm();
            rdsf.ShowDialog(this);
        }

        /// <summary>
        /// 按钮：系统设置
        /// </summary>
        private void button13_Click(object sender, EventArgs e)
        {
            OtherSettingForm osf = new OtherSettingForm();
            osf.ShowDialog(this);
        }
    }
}
