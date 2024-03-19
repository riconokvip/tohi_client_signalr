using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Tohi.Client.Signalr.Commons.Enums
{
    public static class EnumHelpers<T> where T : struct, Enum
    {
        /// <summary>
        /// Lấy tên của enum
        /// </summary>
        /// <param name="value">Enum</param>
        /// <returns></returns>
        public static string GetNameValue(T value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Lấy giá trị của enum
        /// </summary>
        /// <param name="value">Enum</param>
        /// <returns></returns>
        public static string GetDisplayValue(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes[0].ResourceType != null)
                return lookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);

            if (descriptionAttributes == null) return string.Empty;
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
        }

        private static string lookupResource(Type resourceManagerProvider, string resourceKey)
        {
            var resourceKeyProperty = resourceManagerProvider.GetProperty(resourceKey,
                BindingFlags.Static | BindingFlags.Public, null, typeof(string),
                new Type[0], null);
            if (resourceKeyProperty != null)
            {
                return (string)resourceKeyProperty.GetMethod.Invoke(null, null);
            }

            return resourceKey; // Fallback with the key name
        }
    }
}
