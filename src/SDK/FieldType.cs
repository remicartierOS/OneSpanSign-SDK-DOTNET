using System;
using System.Collections.Generic;
using System.Reflection;
using Silanis.ESL.SDK.Internal;

namespace Silanis.ESL.SDK
{
    public class FieldType : EslEnumeration
    {
        private static ILogger log = LoggerFactory.get(typeof(FieldType));

        public static FieldType SIGNATURE = new FieldType("SIGNATURE", "SIGNATURE", 0);
        public static FieldType INPUT = new FieldType("INPUT", "INPUT", 1);
        public static FieldType IMAGE = new FieldType("IMAGE", "IMAGE", 2);
        private static Dictionary<string,FieldType> allFieldTypes = new Dictionary<string,FieldType>();

        static FieldType()
        {
            allFieldTypes.Add(IMAGE.getApiValue(), FieldType.IMAGE);
            allFieldTypes.Add(INPUT.getApiValue(), FieldType.INPUT);
            allFieldTypes.Add(SIGNATURE.getApiValue(), FieldType.SIGNATURE);
        }

        private FieldType(string apiValue, string sdkValue, int index):base(apiValue, sdkValue, index) 
        {           
        }

        internal static FieldType valueOf (string apiValue)
        {

            if (!String.IsNullOrEmpty(apiValue) && allFieldTypes.ContainsKey(apiValue))
            {
                return allFieldTypes[apiValue];
            }
            log.Warn("Unknown API FieldType {0}. The upgrade is required.", apiValue);
            return new FieldType(apiValue, "UNRECOGNIZED", allFieldTypes.Values.Count);
        }

        public static string[] GetNames()
        {
            string[] names = new string[allFieldTypes.Count];
            int i = 0;
            foreach(FieldType fieldType in allFieldTypes.Values)
            {
                names[i] = fieldType.GetName();
                i++;
            }
            return names;
        }

        public static explicit operator FieldType(Enum enumType)
        {
            return parse(enumType.ToString());
        }

        public static FieldType[] Values()
        {
            return (new List<FieldType>(allFieldTypes.Values)).ToArray();
        }

        public static FieldType parse(string value)
        {

            if (null == value)
            {
                throw new ArgumentNullException("value is null");
            }

            if (value.Length == 0 || value.Trim().Length==0)
            {
                throw new ArgumentException("value is either an empty string or only contains white space");
            }
            foreach(FieldType fieldType in allFieldTypes.Values)
            {
                if (String.Equals(fieldType.GetName(), value))
                {
                    return fieldType;
                }
            }
            throw new ArgumentException("value is a name, but not one of the named constants defined for the FieldType");
        }
    }
}