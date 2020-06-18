using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledLayer : TiledMapLayer
{
    [XmlAttribute("x")]
    public int x { set; get; }

    [XmlAttribute("y")]
    public int y { set; get; }

    [XmlAttribute("width")]
    public int width { set; get; }

    [XmlAttribute("height")]
    public int height { set; get; }

    [XmlElement("data")]
    public TiledData data { set; get; }
}