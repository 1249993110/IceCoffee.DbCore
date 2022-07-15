using System;
using System.Collections.Generic;
using System.Text;

namespace IceCoffee.DbCore.CodeGenerator.Models
{
    class EntityStructure
    {
        public string EntityName { get; set; }

        public IEnumerable<FieldInfo> FieldInfos { get; set; }

        public bool IsView { get; set; }

        public string Description { get; set; }
    }
}
