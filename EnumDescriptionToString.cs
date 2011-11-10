using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Common.Enums
{
    /// <summary>Enumerator extension method static class</summary>
    public static class EnumDescriptionToString
    {
        /// <summary>Extension method that wil genrate a String from an enum's Description Attribute</summary>
        /// <param name="me">Enum extended</param>
        /// <returns>A String containing either the Description string or the enumerator's ToString() value</returns>
        public static String GetDescription(this Enum me)
        {
            String description = null;
            
            object[] attributes = me.GetType().GetField(me.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                description = (attributes[0] as DescriptionAttribute).Description;
            else
                description = me.ToString();

            return description;
        }
    }
}