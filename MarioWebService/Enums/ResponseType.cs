using System.ComponentModel;

namespace MarioWebService.Enums
{
    public enum ResponseType
    {
        [Description("in_channel")]
        InChannel,
        [Description("ephemeral")]
        Ephemeral
    }
}