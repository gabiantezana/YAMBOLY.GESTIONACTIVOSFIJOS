using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.HELPER
{
    public static class ConvertHelper
    {
        public static String ToSafeString(this object val)
        {
            return (val ?? String.Empty).ToString();
        }

        public static String GetDBName(this Type type)
        {
            return "@" + type.Name;
        }

        public static string GettAttribute(this PropertyInfo propertyInfo)
        {
            return "";
        }

        public static string GetLastWord(this string nameof)
        {
            if (string.IsNullOrEmpty(nameof)) return string.Empty;
            if (nameof.Split('.').Count() <= 0) return nameof;
            return nameof.Split('.')[nameof.Split('.').Count() - 1];
        }

        public static String IdToString(this Enum enumType)
        {
            var asdf = (Int32)Enum.Parse(enumType.GetType(), enumType.ToString());
            return asdf.ToString();
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        #region GetMemberName

        public static string GetMemberName<T>(Expression<Func<T, object>> expression)
        {
            return GetMemberName(expression.Body);
        }
        private static string GetMemberName(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentException("Expresión can not be null");
            }

            if (expression is MemberExpression)
            {
                // Reference type property or field
                var memberExpression = (MemberExpression)expression;
                return memberExpression.Member.Name;
            }

            if (expression is MethodCallExpression)
            {
                // Reference type method
                var methodCallExpression = (MethodCallExpression)expression;
                return methodCallExpression.Method.Name;
            }

            if (expression is UnaryExpression)
            {
                // Property, field of method returning value type
                var unaryExpression = (UnaryExpression)expression;
                return GetMemberName(unaryExpression);
            }

            throw new ArgumentException("Error");
        }
        private static string GetMemberName(UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MethodCallExpression)
            {
                var methodExpression = (MethodCallExpression)unaryExpression.Operand;
                return methodExpression.Method.Name;
            }

            return ((MemberExpression)unaryExpression.Operand).Member.Name;
        }

        
        #endregion
    }
}