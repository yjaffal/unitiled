using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledCustomProperty
{
    [XmlAttribute("name")]
    public string name { set; get; }

    [XmlAttribute("type")]
    [DefaultValue("")] // default type is a string, type is not set
    public string type { set; get; }

    [XmlAttribute("value")]
    public string value { set; get; }
}