using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledCustomProperty
{
    [System.Xml.Serialization.XmlAttribute("name")]
    public string name { set; get; }

    [System.Xml.Serialization.XmlAttribute("value")]
    public string value { set; get; }
}