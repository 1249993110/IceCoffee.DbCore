using IceCoffee.Common.WinForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.CodeGenerator.UserControls
{
    public class UserControlBase : UserControl, IView
    {
        public virtual string Label { get; }
        public virtual int Sort { get; }

        protected virtual void InitDirectory(string rootDir)
        {
            Directory.CreateDirectory(Path.Combine(rootDir, "Entities"));
            Directory.CreateDirectory(Path.Combine(rootDir, "IRepositories"));
            Directory.CreateDirectory(Path.Combine(rootDir, "Repositories"));
        }
    }
}
