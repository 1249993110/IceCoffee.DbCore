//using AutoMapper;
//using IceCoffee.DbCore.Primitives.Dto;
//using IceCoffee.DbCore.Primitives.Entity;
//using IceCoffee.DbCore.Primitives.Service;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace IceCoffee.DbCore.Primitives
//{
//    public static class EntityDtoMapper
//    {
//        internal static IMapper mapper;

//        /// <summary>
//        /// 初始化Entity与Dto之间的自动映射
//        /// </summary>
//        /// <param name="types"></param>
//        public static void InitMap(IEnumerable<Type> types)
//        {
//            MapperConfiguration config = new MapperConfiguration(cfg =>
//            {
//                var normalTypes = types.Where(t => t.IsSubclassOf(typeof(ServiceBase)));
//                var customTypes = types.Where(t => t.GetInterfaces().Any(i => typeof(ICustomMappings).IsAssignableFrom(t)))
//                                    .Select(t => (ICustomMappings)Activator.CreateInstance(t));

//                foreach (var normalType in normalTypes)
//                {
//                    var genericArgs = GetBaseGenericTypes(normalType);
//                    if (genericArgs != null)
//                    {
//                        var entityType = genericArgs.FirstOrDefault(p => typeof(IEntity).IsAssignableFrom(p));
//                        var dtoType = genericArgs.FirstOrDefault(p => typeof(IDtoBase).IsAssignableFrom(p));

//                        if (entityType != null && dtoType != null)
//                        {
//                            cfg.CreateMap(entityType, dtoType, MemberList.None);
//                            cfg.CreateMap(dtoType, entityType, MemberList.None);
//                        }
//                    }
//                }

//                foreach (var customType in customTypes)
//                {
//                    customType.CreateMappings(cfg);
//                }
//            });

//#if DEBUG
//            try
//            {
//                config.AssertConfigurationIsValid();
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("初始化Entity与Dto之间的自动映射异常", ex);
//            }
//#endif

//            mapper = config.CreateMapper();
//        }

//        /// <summary>
//        /// 递归获取基类的模板类型参数
//        /// </summary>
//        /// <param name="type"></param>
//        /// <returns></returns>
//        private static Type[] GetBaseGenericTypes(Type type)
//        {
//            if (type.BaseType.IsGenericType)
//            {
//                Type[] types = type.BaseType.GetGenericArguments();
//                if (types.Length > 1)
//                {
//                    return types;
//                }
//            }
//            else if (type == typeof(object))
//            {
//                return null;
//            }

//            return GetBaseGenericTypes(type.BaseType);
//        }
//    }
//}
