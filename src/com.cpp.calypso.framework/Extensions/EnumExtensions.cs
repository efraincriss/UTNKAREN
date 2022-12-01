using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.framework.Extensions
{
	 

	public static class EnumExtensions
	{
		/// <summary>
		/// Converts string to enum value (opposite to Enum.ToString()).
		/// </summary>
		/// <typeparam name="T">Type of the enum to convert the string into.</typeparam>
		/// <param name="s">string to convert to enum value.</param>
		public static T ToEnum<T>(this string s) where T : struct
		{
			T newValue;
			return Enum.TryParse(s, out newValue) ? newValue : default(T);
		}

        public static string GetDescription(this Enum GenericEnum) //Hint: Change the method signature and input paramter to use the type parameter T
        {
            Type genericEnumType = GenericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return GenericEnum.ToString();
        }
    }
}
