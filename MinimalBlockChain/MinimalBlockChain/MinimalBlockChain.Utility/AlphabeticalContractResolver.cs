using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace MinimalBlockChain.Utility
{
    public class AlphabeticalContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization).OrderBy(p => p.PropertyName).ToList();
        }
    }
}