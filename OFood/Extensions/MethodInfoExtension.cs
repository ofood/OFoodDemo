using System.Reflection;
using System.Text;

namespace OFood.Extensions
{
    public static class MethodInfoExtension
    {
        /// <summary>
        /// 获取签名字符串.
        /// </summary>
        /// <param name="method">方法.</param>
        /// <returns>签名字符串.</returns>
        public static string GetSignature(this MethodInfo method)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(method.ReturnType.GetSignature());
            sb.Append(" ");
            sb.Append(method.Name);
            if (method.IsGenericMethod)
            {
                sb.Append("[");
                var genericTypes = method.GetGenericArguments();
                int i = 0;
                foreach (var genericType in genericTypes)
                {
                    sb.Append(genericType.GetSignature());
                    if (i != genericTypes.Length - 1)
                        sb.Append(", ");
                    i++;
                }
                sb.Append("]");
            }
            sb.Append("(");
            var parameters = method.GetParameters();
            if (parameters != null && parameters.Length > 0)
            {
                int i = 0;
                foreach (var parameter in parameters)
                {
                    sb.Append(parameter.ParameterType.GetSignature());
                    if (i != parameters.Length - 1)
                        sb.Append(", ");
                    i++;
                }
            }
            sb.Append(")");
            return sb.ToString();
        }
    }
}
