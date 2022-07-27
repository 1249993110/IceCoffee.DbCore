using IceCoffee.DbCore.OptionalAttributes;
using IceCoffee.DbCore.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace IceCoffee.DbCore.CodeGenerator.Models
{
    class FieldInfo
    {
        public string ColumnName { get; set; }

        public string TypeName { get; set; }

        public bool IsNullable { get; set; }

        public bool IsPrimaryKey { get; set; }

        public string Description { get; set; }
    }
}
