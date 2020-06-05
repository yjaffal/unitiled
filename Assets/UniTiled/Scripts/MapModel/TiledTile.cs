using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledTile
{
    [XmlAttribute("id")]
    public int id { set; get; }

    [XmlAttribute("terrain")]
    public string terrain { set; get; }

    [XmlElement("image", typeof(TiledImage))]
    public TiledImage image { set; get; }

    [XmlElement("animation")]
    public TiledAnimation animation { get; set; }

    [XmlElement("objectgroup")]
    public TiledObjectGroup objectsGroup { get; set; }

    [XmlArray("properties")]
    [XmlArrayItem("property", typeof(TiledCustomProperty))]
    public TiledCustomProperty[] customProperties { get; set; }
}