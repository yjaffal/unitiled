using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledObjectGroup
{
    [System.Xml.Serialization.XmlAttribute("offsetx")]
    public float offsetX { set; get; }

    [System.Xml.Serialization.XmlAttribute("offsety")]
    public float offsetY { set; get; }

    [XmlElement("object")]
    public TiledTObject[] objects { get; set; }
}