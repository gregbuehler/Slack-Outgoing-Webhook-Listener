using System.Collections.Generic;
using MarioWebService.Enums;

namespace MarioWebService.Models
{
    public class SlackResponse
    {
        public ResponseType ResponseType { get; set; }
        public string Text { get; set; }
        public List<Attachment> Attachments { get; set; }
        public bool SuppressMessageTextOnSlashCommandResponse { get; set; }
    }
}