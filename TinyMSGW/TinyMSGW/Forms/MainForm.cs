using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TinyMSGW.Forms
{
    /// <summary>
    /// 窗体：主窗体
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// 构造器
        /// </summary>
        public MainForm()
        {
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
        
    }
}
