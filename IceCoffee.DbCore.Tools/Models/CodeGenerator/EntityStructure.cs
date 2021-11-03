using System;
using System.Collections.Generic;
using System.Text;

namespace IceCoffee.DbCore.Tools.Models.CodeGenerator
{
    class EntityStructure
    {
        public string EntityName { get; set; }

        public IList<ColumnStructure> ColumnStructures { get; set; }

        public string Description { get; set; }

        public bool IsView { get; set; }
    }
}
