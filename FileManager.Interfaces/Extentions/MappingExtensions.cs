using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace FileManager.Core.Extentions
{
    public static class MappingExtensions
    {
        public static IList<T> MapTo<T>(this IEnumerable src) where T : class, new()
        {
            if (src == null) return null;
            var list = new List<T>();
            var properties = typeof(T).GetProperties();
            foreach (var item in src)
            {
                var obj = new T();
                MapObject(item, obj, properties);
                list.Add(obj);
            }
            return list;
        }
        public static T MapTo<T>(this object src) where T : class, new()
        {
            if (src == null) return null;
            var obj = new T();
            MapObject(src, obj, typeof(T).GetProperties());
            return obj;
        }
        private static bool CanIgnore(PropertyInfo prop, bool isSource)
        {
            if (prop == null) return true;
            if (prop.GetCustomAttribute(typeof(IgnoreMapAttribute)) != null) return true;
            if (prop.PropertyType.GetCustomAttribute(typeof(IgnoreMapAttribute)) != null) return true;
            if (isSource)
            {
                if (prop.GetCustomAttribute(typeof(IgnoreSourceMapAttribute)) != null) return true;
                if (prop.PropertyType.GetCustomAttribute(typeof(IgnoreSourceMapAttribute)) != null) return true;
            }
            else
            {
                if (prop.GetCustomAttribute(typeof(IgnoreDestinationMapAttribute)) != null) return true;
                if (prop.PropertyType.GetCustomAttribute(typeof(IgnoreDestinationMapAttribute)) != null) return true;
            }
            return false;
        }

        private static void MapObject<T>(this object src, T dest, PropertyInfo[] properties) where T : class
        {
            foreach (var srcProp in src.GetType().GetProperties())
            {
                if (!CanIgnore(srcProp, true))
                {
                    var value = srcProp.GetValue(src);
                    var pi = properties.Where(x => x.Name == srcProp.Name).FirstOrDefault();
                    if (!CanIgnore(pi, false))
                    {
                        try
                        {
                            if (pi?.SetMethod != null)
                            {
                                if (srcProp.PropertyType != pi.PropertyType)
                                    throw new Exception($"Invalid type {srcProp.PropertyType.Name} => {pi.PropertyType.Name}");
                                if (value == null)
                                    pi.SetValue(dest, null);
                                else
                                    pi.SetValue(dest, value);
                            }
                        }
                        catch (Exception ex)
                        {
                            var destPropName = pi == null ? "?" : pi.Name;
                            throw new Exception($"Error in Mapping {src?.GetType().Name}.{srcProp?.Name} => {dest?.GetType().Name}.{destPropName}: {ex.Message}");
                        }
                    }
                }
            }
        }
    }
}
