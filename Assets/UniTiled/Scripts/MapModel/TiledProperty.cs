using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledProperty
{
    [XmlAttribute("name")]
    public string name { set; get; }

    [XmlAttribute("type")]
    [DefaultValue("")]
    public string type { set; get; }

    [XmlAttribute("value")]
    [DefaultValue("")]
    public string value { set; get; }

    // When a string property contains newlines, the current version
    // of Tiled (1.4) will write out the value as characters contained
    // inside the property element rather than as the value attribute
    [XmlText]
    [DefaultValue("")]
    public string content { set; get; }
}