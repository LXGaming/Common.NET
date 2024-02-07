using System.Linq.Expressions;
using System.Reflection;

namespace LXGaming.Common.Utilities;

public static class ReflectionUtils {

    #region Expression
    public static BinaryExpression Assign(Expression left, Expression right) {
        if (left is MemberExpression { Member: FieldInfo { IsInitOnly: true } }) {
            return (BinaryExpression) Activator.CreateInstance(
                typeof(Expression).Assembly.GetType("System.Linq.Expressions.AssignBinaryExpression", true)!,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.CreateInstance,
                null,
                [left, right],
                null)!;
        }

        return Expression.Assign(left, right);
    }
    #endregion

    #region Field Getter
    public static Func<TReflectedType, TFieldType> CreateFieldGetter<TReflectedType, TFieldType>(string name) {
        var field = GetRequiredField<TReflectedType>(name);
        return CreateFieldGetter<TReflectedType, TFieldType>(field);
    }

    public static Func<TReflectedType, TFieldType> CreateFieldGetter<TReflectedType, TFieldType>(string name, BindingFlags bindingAttr) {
        var field = GetRequiredField<TReflectedType>(name, bindingAttr);
        return CreateFieldGetter<TReflectedType, TFieldType>(field);
    }

    public static Func<TReflectedType, TFieldType> CreateFieldGetter<TReflectedType, TFieldType>(FieldInfo field) {
        var instanceParameter = Expression.Parameter(typeof(TReflectedType), "instance");
        return Expression.Lambda<Func<TReflectedType, TFieldType>>(
            Expression.Field(field.IsStatic ? null : instanceParameter, field),
            instanceParameter
        ).Compile();
    }
    #endregion

    #region Field Setter
    public static Action<TReflectedType, TFieldType> CreateFieldSetter<TReflectedType, TFieldType>(string name) {
        var field = GetRequiredField<TReflectedType>(name);
        return CreateFieldSetter<TReflectedType, TFieldType>(field);
    }

    public static Action<TReflectedType, TFieldType> CreateFieldSetter<TReflectedType, TFieldType>(string name, BindingFlags bindingAttr) {
        var field = GetRequiredField<TReflectedType>(name, bindingAttr);
        return CreateFieldSetter<TReflectedType, TFieldType>(field);
    }

    public static Action<TReflectedType, TFieldType> CreateFieldSetter<TReflectedType, TFieldType>(FieldInfo field) {
        var instanceParameter = Expression.Parameter(typeof(TReflectedType), "instance");
        var valueParameter = Expression.Parameter(typeof(TFieldType), "value");
        return Expression.Lambda<Action<TReflectedType, TFieldType>>(
            Assign(
                Expression.Field(field.IsStatic ? null : instanceParameter, field),
                valueParameter
            ),
            instanceParameter, valueParameter
        ).Compile();
    }
    #endregion

    #region Field
    public static FieldInfo? GetField<TReflectedType>(string name, bool? @static = null) {
        return GetField(typeof(TReflectedType), name, @static);
    }

    public static FieldInfo GetRequiredField<TReflectedType>(string name, bool? @static = null) {
        return GetRequiredField(typeof(TReflectedType), name, @static);
    }

    public static FieldInfo? GetField(Type type, string name, bool? @static = null) {
        var flags = @static switch {
            true => BindingFlags.Static,
            false => BindingFlags.Instance,
            null => BindingFlags.Instance | BindingFlags.Static
        };

        return type.GetField(name, flags | BindingFlags.Public | BindingFlags.FlattenHierarchy)
               ?? type.GetField(name, flags | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
    }

    public static FieldInfo GetRequiredField(Type type, string name, bool? @static = null) {
        var field = GetField(type, name, @static);
        if (field == null) {
            throw new MissingFieldException(type.Name, name);
        }

        return field;
    }

    public static FieldInfo GetRequiredField<TReflectedType>(string name, BindingFlags bindingAttr) {
        return GetRequiredField(typeof(TReflectedType), name, bindingAttr);
    }

    public static FieldInfo GetRequiredField(Type type, string name, BindingFlags bindingAttr) {
        var field = type.GetField(name, bindingAttr);
        if (field == null) {
            throw new MissingFieldException(type.Name, name);
        }

        return field;
    }
    #endregion

    #region Method
    public static MethodInfo? GetMethod<TReflectedType>(string name, bool? @static = null) {
        return GetMethod(typeof(TReflectedType), name, @static);
    }

    public static MethodInfo GetRequiredMethod<TReflectedType>(string name, bool? @static = null) {
        return GetRequiredMethod(typeof(TReflectedType), name, @static);
    }

    public static MethodInfo? GetMethod(Type type, string name, bool? @static = null) {
        var flags = @static switch {
            true => BindingFlags.Static,
            false => BindingFlags.Instance,
            null => BindingFlags.Instance | BindingFlags.Static
        };

        return type.GetMethod(name, flags | BindingFlags.Public | BindingFlags.FlattenHierarchy)
               ?? type.GetMethod(name, flags | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
    }

    public static MethodInfo GetRequiredMethod(Type type, string name, bool? @static = null) {
        var method = GetMethod(type, name, @static);
        if (method == null) {
            throw new MissingMethodException(type.Name, name);
        }

        return method;
    }

    public static MethodInfo GetRequiredMethod<TReflectedType>(string name, BindingFlags bindingAttr) {
        return GetRequiredMethod(typeof(TReflectedType), name, bindingAttr);
    }

    public static MethodInfo GetRequiredMethod(Type type, string name, BindingFlags bindingAttr) {
        var method = type.GetMethod(name, bindingAttr);
        if (method == null) {
            throw new MissingMethodException(type.Name, name);
        }

        return method;
    }

    public static MethodInfo GetRequiredMethod<TReflectedType>(string name, BindingFlags bindingAttr, Type[] types) {
        return GetRequiredMethod(typeof(TReflectedType), name, bindingAttr, types);
    }

    public static MethodInfo GetRequiredMethod(Type type, string name, BindingFlags bindingAttr, Type[] types) {
        var method = type.GetMethod(name, bindingAttr, types);
        if (method == null) {
            throw new MissingMethodException(type.Name, name);
        }

        return method;
    }
    #endregion

    #region Property Getter
    public static Func<TReflectedType, TPropertyType> CreatePropertyGetter<TReflectedType, TPropertyType>(string name) {
        var property = GetRequiredProperty<TReflectedType>(name);
        return CreatePropertyGetter<TReflectedType, TPropertyType>(property);
    }

    public static Func<TReflectedType, TPropertyType> CreatePropertyGetter<TReflectedType, TPropertyType>(string name, BindingFlags bindingAttr) {
        var property = GetRequiredProperty<TReflectedType>(name, bindingAttr);
        return CreatePropertyGetter<TReflectedType, TPropertyType>(property);
    }

    public static Func<TReflectedType, TPropertyType> CreatePropertyGetter<TReflectedType, TPropertyType>(PropertyInfo property) {
        var getMethod = property.GetMethod;
        if (getMethod == null) {
            var field = GetRequiredField<TReflectedType>($"<{property.Name}>k__BackingField");
            return CreateFieldGetter<TReflectedType, TPropertyType>(field);
        }

        var instanceParameter = Expression.Parameter(typeof(TReflectedType), "instance");
        return Expression.Lambda<Func<TReflectedType, TPropertyType>>(
            Expression.Call(getMethod.IsStatic ? null : instanceParameter, getMethod),
            instanceParameter
        ).Compile();
    }
    #endregion

    #region Property Setter
    public static Action<TReflectedType, TPropertyType> CreatePropertySetter<TReflectedType, TPropertyType>(string name) {
        var property = GetRequiredProperty<TReflectedType>(name);
        return CreatePropertySetter<TReflectedType, TPropertyType>(property);
    }

    public static Action<TReflectedType, TPropertyType> CreatePropertySetter<TReflectedType, TPropertyType>(string name, BindingFlags bindingAttr) {
        var property = GetRequiredProperty<TReflectedType>(name, bindingAttr);
        return CreatePropertySetter<TReflectedType, TPropertyType>(property);
    }

    public static Action<TReflectedType, TPropertyType> CreatePropertySetter<TReflectedType, TPropertyType>(PropertyInfo property) {
        var setMethod = property.SetMethod;
        if (setMethod == null) {
            var field = GetRequiredField<TReflectedType>($"<{property.Name}>k__BackingField");
            return CreateFieldSetter<TReflectedType, TPropertyType>(field);
        }

        var instanceParameter = Expression.Parameter(typeof(TReflectedType), "instance");
        var valueParameter = Expression.Parameter(typeof(TPropertyType), "value");
        return Expression.Lambda<Action<TReflectedType, TPropertyType>>(
            Expression.Call(setMethod.IsStatic ? null : instanceParameter, setMethod, valueParameter),
            instanceParameter, valueParameter
        ).Compile();
    }
    #endregion

    #region Property
    public static PropertyInfo? GetProperty<TReflectedType>(string name, bool? @static = null) {
        return GetProperty(typeof(TReflectedType), name, @static);
    }

    public static PropertyInfo GetRequiredProperty<TReflectedType>(string name, bool? @static = null) {
        return GetRequiredProperty(typeof(TReflectedType), name, @static);
    }

    public static PropertyInfo? GetProperty(Type type, string name, bool? @static = null) {
        var flags = @static switch {
            true => BindingFlags.Static,
            false => BindingFlags.Instance,
            null => BindingFlags.Instance | BindingFlags.Static
        };

        return type.GetProperty(name, flags | BindingFlags.Public | BindingFlags.FlattenHierarchy)
               ?? type.GetProperty(name, flags | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
    }

    public static PropertyInfo GetRequiredProperty(Type type, string name, bool? @static = null) {
        var property = GetProperty(type, name, @static);
        if (property == null) {
            throw new MissingMemberException(type.Name, name);
        }

        return property;
    }

    public static PropertyInfo GetRequiredProperty<TReflectedType>(string name, BindingFlags bindingAttr) {
        return GetRequiredProperty(typeof(TReflectedType), name, bindingAttr);
    }

    public static PropertyInfo GetRequiredProperty(Type type, string name, BindingFlags bindingAttr) {
        var property = type.GetProperty(name, bindingAttr);
        if (property == null) {
            throw new MissingMemberException(type.Name, name);
        }

        return property;
    }
    #endregion
}