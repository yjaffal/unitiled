using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledTile
{
    [XmlAttribute("id")]
    public int id { set; get; }

    // Refers to an object type and is used by tile objects. (optional) (since 1.0)
    [XmlAttribute("type")]
    [DefaultValue("")]
    public string type { set; get; }

    [XmlAttribute("terrain")]
    [DefaultValue("")]
    public string terrain { set; get; }

    // used by editor, safe to ignore
    [XmlAttribute("probability")]
    [DefaultValue(0.0f)]
    public float probability { set; get; }

    [XmlArray("properties")]
    [XmlArrayItem("property", typeof(TiledProperty))]
    public TiledProperty[] customProperties { get; set; }

    [XmlElement("image", typeof(TiledImage))]
    public TiledImage image { set; get; }

    [XmlElement("objectgroup")]
    public TiledObjectGroup objectsGroup { get; set; }

    [XmlElement("animation")]
    public TiledAnimation animation { get; set; }
}