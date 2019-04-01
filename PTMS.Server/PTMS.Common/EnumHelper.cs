using System.ComponentModel;
using Enum = System.Enum;

namespace PTMS.Common
{
    public class EnumHelper
    {
        public static string GetDescription(Enum en)
        {
            var attr = GetAttribute<DescriptionAttribute>(en);

            if (attr != null)
            {
                return attr.Description;
            }
            else
            {
                return en.ToString();
            }
        }
        
        private static T GetAttribute<T>(Enum en)
        {
            var type = en.GetType();
            var memInfo = type.GetMember(en.ToString());
            if (memInfo.Length > 0)
            {
                var attrs = memInfo[0].GetCustomAttributes(typeof(T), false);

                if (attrs.Length > 0)
                {
                    return (T)attrs[0];
                }
            }

            return default(T);
        }
    }
}