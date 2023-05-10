using IceCoffee.Common.WinForm;
using IceCoffee.DbCore.CodeGenerator.UserControls;
using System.Reflection;

namespace IceCoffee.DbCore.CodeGenerator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            LoadUserControl();
        }

        private void LoadUserControl()
        {
            var types = Assembly.GetExecutingAssembly().GetExportedTypes().Where(type => typeof(IView).IsAssignableFrom(type));

            var views = new List<IView>();

            foreach (var type in types)
            {
                if (Activator.CreateInstance(type) is IView view)
                {
                    views.Add(view);
                }
            }

            foreach (var view in views.OrderBy(i => i.Sort))
            {
                var tabPage = new TabPage()
                {
                    Text = view.Label
                };

                ((Control)view).Dock = DockStyle.Fill;
                tabPage.Controls.Add((Control)view);
                this.tabControl.Controls.Add(tabPage);
            }
        }
    }
}