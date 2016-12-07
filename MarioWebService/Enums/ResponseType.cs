using System.Runtime.Serialization;

namespace MarioWebService.Enums
{
    public enum ResponseType
    {
        [EnumMember(Value = "in_channel")]
        InChannel,
        [EnumMember(Value = "ephemeral")]
        Ephemeral
    }
}