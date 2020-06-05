using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledObjectGroup
{
    [XmlAttribute("offsetx")]
    public float offsetX { set; get; }

    [XmlAttribute("offsety")]
    public float offsetY { set; get; }

    [XmlElement("object")]
    public TiledTObject[] objects { get; set; }
}