using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Concurrency.Data
{
   public enum TaskResult
   {
        [Display(Name ="Completed")]
        TaskCompleted =1,
        [Display(Name ="Cancelled")]
        TaskCancelled =2,
        [Display(Name ="Error")]
        TaskError =3
   }

    public enum MilkshakeSize
    {
        [Display(Name ="S")]
        Small =1,
        [Display(Name ="M")]
        Medium = 2,
        [Display(Name ="L")]
        Large =3,
        [Display(Name ="XL")]
        ExtraLarge =4
    }
    
    

    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enu)
        {
            var attr = GetDisplayAttribute(enu);
            return attr.Name ?? enu.ToString();
        }

        public static string GetDescription(this Enum enu)
        {
            var attr = GetDisplayAttribute(enu);
            return attr.Description ?? enu.ToString();
        }

        private static DisplayAttribute GetDisplayAttribute(object value)
        {
            Type type = value.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException(string.Format("Type {0} is not an enum", type));
            }

            // Get the enum field.
            var field = type.GetField(value.ToString()?? throw new ArgumentException(nameof(value)));
            return field?.GetCustomAttribute<DisplayAttribute>() ?? throw new ArgumentException(nameof(field));
        }
    }
}

