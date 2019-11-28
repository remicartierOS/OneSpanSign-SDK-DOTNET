using System;
using System.Reflection;
using System.Collections.Generic;
using Silanis.ESL.SDK.Internal;

namespace Silanis.ESL.SDK
{
    public class GroupMemberType : EslEnumeration
    {
        private static ILogger log = LoggerFactory.get(typeof(GroupMemberType));

        public static GroupMemberType REGULAR = new GroupMemberType("REGULAR", "REGULAR", 0);
        public static GroupMemberType MANAGER = new GroupMemberType("MANAGER", "MANAGER", 1);

        private static Dictionary<string,GroupMemberType> allGroupMemberTypes = new Dictionary<string,GroupMemberType>();

        static GroupMemberType()
        {
            allGroupMemberTypes.Add(REGULAR.getApiValue(), GroupMemberType.REGULAR);
            allGroupMemberTypes.Add(MANAGER.getApiValue(), GroupMemberType.MANAGER);
        }

        private GroupMemberType(string apiValue, string sdkValue, int index):base(apiValue,sdkValue,index) 
        {           
        }

        internal static GroupMemberType valueOf (string apiValue)
        {
            if (!String.IsNullOrEmpty(apiValue) && allGroupMemberTypes.ContainsKey(apiValue))
            {
                return allGroupMemberTypes[apiValue];
            }
            log.Warn("Unknown API GroupMemberType {0}. The upgrade is required.", apiValue);
            return new GroupMemberType(apiValue, "UNRECOGNIZED", allGroupMemberTypes.Values.Count);
        }

        public static string[] GetNames()
        {
            string[] names = new string[allGroupMemberTypes.Count];
            int i = 0;
            foreach(GroupMemberType groupMemberType in allGroupMemberTypes.Values)
            {
                names[i] = groupMemberType.GetName();
                i++;
            }
            return names;
        }

        public static explicit operator GroupMemberType(Enum enumType)
        {
            return parse(enumType.ToString());
        }

        public static GroupMemberType[] Values()
        {
            return (new List<GroupMemberType>(allGroupMemberTypes.Values)).ToArray();
        }

        public static GroupMemberType parse(string value)
        {
            if (null == value)
            {
                throw new ArgumentNullException("value is null");
            }

            if (value.Length == 0 || value.Trim().Length==0)
            {
                throw new ArgumentException("value is either an empty string or only contains white space");
            }
            foreach(GroupMemberType groupMemberType in allGroupMemberTypes.Values)
            {
                if (String.Equals(groupMemberType.GetName(), value))
                {
                    return groupMemberType;
                }
            }
            throw new ArgumentException("value is a name, but not one of the named constants defined for the GroupMemberType");
        }
    }
}

