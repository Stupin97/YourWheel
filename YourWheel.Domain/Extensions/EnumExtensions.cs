namespace YourWheel.Domain.Extensions
{
    using System.ComponentModel;
    using System.Reflection;

    public static class EnumExtensions
    {
        public static object GetAmbientValue(this Enum enumVal)
        {
            Type type = enumVal.GetType();

            MemberInfo[] memInfo = type.GetMember(enumVal.ToString());

            object[] attributes = memInfo[0].GetCustomAttributes(typeof(AmbientValueAttribute), false);

            if (attributes == null || attributes.Length == 0) return default;

            return ((AmbientValueAttribute)attributes[0]).Value;
        }
    }
}
