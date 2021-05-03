using System.Xml.Serialization;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Model
{
    [XmlRoot(ElementName = "parameters")]
    public class ParametersElem
    {
        public Parameter[] Parameters { get; set; }
    }

    public class Parameter
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }
}