using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledLayer
{
    [System.Xml.Serialization.XmlAttribute("name")]
    public string name { set; get; }

    [System.Xml.Serialization.XmlAttribute("width")]
    public int width { set; get; }

    [System.Xml.Serialization.XmlAttribute("height")]
    public int height { set; get; }

    [XmlElement("data")]
    public TiledLayerData data { set; get; }

    [XmlArray("properties")]
    [XmlArrayItem("property", typeof(TiledCustomProperty))]
    public TiledCustomProperty[] customProperties { get; set; }
}