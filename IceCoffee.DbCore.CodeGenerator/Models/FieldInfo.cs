namespace IceCoffee.DbCore.CodeGenerator.Models
{
    internal class FieldInfo
    {
        public bool IsPrimaryKey { get; set; }

        public string ColumnName { get; set; }

        public string TypeName { get; set; }

        public bool IsNullable { get; set; }

        public string Description { get; set; }
    }
}