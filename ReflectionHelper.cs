using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace TK.BaseLib
{
    public static class ReflectionHelper
    {
        public static TypeBuilder GetTypeBuilder(int randomValue)
        {
            AssemblyName an = new AssemblyName("DynamicAssembly" + randomValue.ToString());
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");

            TypeBuilder tb = moduleBuilder.DefineType("DynamicType"
                                , TypeAttributes.Public |
                                TypeAttributes.Class |
                                TypeAttributes.AutoClass |
                                TypeAttributes.AnsiClass |
                                TypeAttributes.BeforeFieldInit |
                                TypeAttributes.AutoLayout
                                , typeof(object));
            return tb;
        }

        public static void CreateProperty(TypeBuilder builder, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = builder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);
            PropertyBuilder propertyBuilder = builder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);

            MethodBuilder getPropertyBuiler = CreatePropertyGetter(builder, fieldBuilder);
            MethodBuilder setPropertyBuiler = CreatePropertySetter(builder, fieldBuilder);

            propertyBuilder.SetGetMethod(getPropertyBuiler);
            propertyBuilder.SetSetMethod(setPropertyBuiler);
        }

        private static MethodBuilder CreatePropertyGetter(TypeBuilder typeBuilder, FieldBuilder fieldBuilder)
        {
            MethodBuilder getMethodBuilder =
                typeBuilder.DefineMethod("get_" + fieldBuilder.Name,
                    MethodAttributes.Public |
                    MethodAttributes.SpecialName |
                    MethodAttributes.HideBySig,
                    fieldBuilder.FieldType, Type.EmptyTypes);

            ILGenerator getIL = getMethodBuilder.GetILGenerator();

            getIL.Emit(OpCodes.Ldarg_0);
            getIL.Emit(OpCodes.Ldfld, fieldBuilder);
            getIL.Emit(OpCodes.Ret);

            return getMethodBuilder;
        }

        private static MethodBuilder CreatePropertySetter(TypeBuilder typeBuilder, FieldBuilder fieldBuilder)
        {
            MethodBuilder setMethodBuilder =
                typeBuilder.DefineMethod("set_" + fieldBuilder.Name,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new Type[] { fieldBuilder.FieldType });

            ILGenerator setIL = setMethodBuilder.GetILGenerator();

            setIL.Emit(OpCodes.Ldarg_0);
            setIL.Emit(OpCodes.Ldarg_1);
            setIL.Emit(OpCodes.Stfld, fieldBuilder);
            setIL.Emit(OpCodes.Ret);

            return setMethodBuilder;
        }

        public static object SetProperty(object Target, string Name, object value, bool ignoreIfTargetIsNull)
        {

            if (ignoreIfTargetIsNull && Target == null) return null;

            object[] values = { value };

            object oldProperty = GetProperty(Target, Name, false);

            PropertyInfo targetProperty = Target.GetType().GetProperty(Name);

            if (targetProperty == null)
            {
                throw new Exception("Object " + Target.ToString() + "   does not have Target Property " + Name);

            }

            targetProperty.GetSetMethod().Invoke(Target, values);

            return oldProperty;

        }

        public static object GetProperty(object Target, string Name, bool throwError)
        {
            PropertyInfo targetProperty = Target.GetType().GetProperty(Name);

            if (targetProperty == null)
            {
                if (throwError)
                {
                    throw new Exception("Object " + Target.ToString() + "   does not have Target Property " + Name);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return targetProperty.GetGetMethod().Invoke(Target, null);
            }

        }
    }
}
