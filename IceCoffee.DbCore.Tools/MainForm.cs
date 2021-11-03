using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IceCoffee.DbCore.Tools
{
    public partial class MainForm : Form
    {
        private static SynchronizationContext _mainThreadContext;
        public static SynchronizationContext MainThreadContext => _mainThreadContext;
        public MainForm()
        {
            _mainThreadContext = SynchronizationContext.Current;

            InitializeComponent();

            base.Text = "数据库工具 v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();

            LoadUserControl();
        }

        private void LoadUserControl()
        {
            var types = Assembly.GetExecutingAssembly().GetExportedTypes().Where(p => p.IsSubclassOf(typeof(UserControl)));
            foreach (var type in types)
            {
                var control = (UserControl)Activator.CreateInstance(type);

                var tabPage = new TabPage()
                {
                    Text = string.IsNullOrEmpty(control.Text) ? control.Name : control.Text
                };

                control.Dock = DockStyle.Fill;
                tabPage.Controls.Add(control);
                this.tabControl.Controls.Add(tabPage);
            }
        }
    }
}
