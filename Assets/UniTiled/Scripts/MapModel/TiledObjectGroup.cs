using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledObjectGroup : TiledMapLayer
{
    [XmlAttribute("color")]
    public string color { set; get; }

    [XmlAttribute("x")]
    public int x { set; get; }

    [XmlAttribute("y")]
    public int y { set; get; }

    [XmlAttribute("width")]
    public int width { set; get; }

    [XmlAttribute("height")]
    public int height { set; get; }

    [XmlAttribute("draworder")]
    [DefaultValue("topdown")]
    public string drawOrder { set; get; }

    [XmlElement("object")]
    public TiledTObject[] objects { get; set; }
}