using AutoMapper;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Insurance.BL.Util.AutoMapper
{
   public static class AutoMapperExtension
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>(this IMappingExpression<TSource, TDestination> config)
        {
            try
            {
                var source = typeof(TSource);
                var destination = typeof(TDestination);
                var destinationProperties = destination.GetProperties();
                var sourceProperties = source.GetProperties();

                foreach (var dproperty in destinationProperties)
                {
                    if (!sourceProperties.Any(x => x.Name.ToUpper() == dproperty.Name.ToUpper() && GetUnderlyingType(x) == GetUnderlyingType(dproperty)))
                    {
                        config.ForMember(dproperty.Name, opt => opt.Ignore());
                    }
                }

                return config;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw;
            }
        }

        public static void Unflatten<TSource, TDestination, TMember>(this IMemberConfigurationExpression<TSource, TDestination, TMember> config)
        {
            config.MapFrom((source, destination) =>
            {
                try
                {
                    var member = config.DestinationMember;
                    var targetProperties = destination.GetType().GetProperties();
                    var sourceProperties = source.GetType().GetProperties();

                    foreach (var p in sourceProperties)
                    {
                        var matched = targetProperties.Where(x => x.Name.ToUpper() == p.Name.ToUpper() && GetUnderlyingType(x).Equals(GetUnderlyingType(p))).FirstOrDefault();
                        matched?.SetValue(destination, p.GetValue(source));
                    }

                    return destination;
                }
                catch (Exception ex)
                {
                    LogManager.GetCurrentClassLogger().Error(ex);
                    throw;
                }
            });
        }

        private static Type GetUnderlyingType(PropertyInfo p)
        {
            Type t = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
            return t;
        }
    }
}
