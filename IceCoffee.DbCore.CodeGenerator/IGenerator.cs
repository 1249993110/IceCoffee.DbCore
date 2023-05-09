using IceCoffee.DbCore.CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.CodeGenerator
{
    internal interface IGenerator
    {
        string GenerateEntityClass(EntityInfo entityInfo);

        string GenerateIRepository(EntityInfo entityInfo);

        string GenerateRepository(EntityInfo entityInfo);

        string GetClassName(EntityInfo entityInfo);

        string GetTableAttribute(EntityInfo entityInfo);

        string GetRepositoryName(EntityInfo entityInfo);

        string GetCSharpType(FieldInfo fieldInfo);

        string GetPropertyName(FieldInfo fieldInfo);

        string GetColumnAttribute(FieldInfo fieldInfo);
    }
}
