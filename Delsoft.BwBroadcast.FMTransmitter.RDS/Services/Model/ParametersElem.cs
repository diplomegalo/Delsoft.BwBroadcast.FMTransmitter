using System.Xml.Serialization;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Model
{
    [XmlRoot("parameters")]
    public class ParametersElem
    {
        [XmlElement("parameter")]
        public Parameter[] Parameters { get; set; }
    }

    public class Parameter
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}