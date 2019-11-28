using System;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using Newtonsoft.Json;

namespace Silanis.ESL.SDK
{
    public class DocumentMetadataContractResolver : DefaultContractResolver
    {
        public static readonly DocumentMetadataContractResolver Instance = new DocumentMetadataContractResolver();

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (property.DeclaringType == typeof(Silanis.ESL.API.Document) )
            {
                if (property.PropertyName == "approvals" ||
                    property.PropertyName == "fields" || 
                    property.PropertyName == "pages")
                {
                    property.ShouldSerialize = instance =>
                            {
                                return false;
                            };
                }
            }

            return property;
        }
    }
}

