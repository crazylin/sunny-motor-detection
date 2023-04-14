using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Gemini.Modules.Utils;

namespace Gemini.Modules.Utils
{
    public static class EnumExtension
    {
        public static string GetDisplay<T>(this T value)
        {
            var type = value.GetType();
            FieldInfo field = type.GetField(System.Enum.GetName(type, value));
            DisplayAttribute descAttr =
                Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) as DisplayAttribute;
            if (descAttr == null)
            {
                return value.ToString();
            }

            return TranslationUtil.GetTranslate(descAttr.Name, descAttr.ResourceType);
        }
    }
}
