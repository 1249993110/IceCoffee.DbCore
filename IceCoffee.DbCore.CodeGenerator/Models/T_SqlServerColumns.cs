using System;
using System.Collections.Generic;
using System.Text;

namespace IceCoffee.DbCore.CodeGenerator.Models
{
    class T_SqlServerColumns
    {
        public string Column_Name { get; set; }

        public string Type_Name { get; set; }

        public bool Nullable { get; set; }
    }
}
