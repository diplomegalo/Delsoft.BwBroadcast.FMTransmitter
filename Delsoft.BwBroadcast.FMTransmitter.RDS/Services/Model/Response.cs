using System.Xml.Serialization;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Model
{
    [XmlRoot(ElementName = "response")]
    public class Response
    {
        [XmlAttribute(AttributeName = "success")]
        public bool Success { get; set; }

        [XmlAttribute(AttributeName = "reason")]
        public string Reason { get; set; }
    }
}