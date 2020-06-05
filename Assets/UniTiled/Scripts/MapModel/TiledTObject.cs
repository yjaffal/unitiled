using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledTObject
{
    [XmlAttribute("id")]
    public int id { set; get; }

    [XmlAttribute("x")]
    public float x { set; get; }

    [XmlAttribute("y")]
    public float y { set; get; }

    [XmlAttribute("width")]
    public float width { set; get; }

    [XmlAttribute("height")]
    public float height { set; get; }

    [XmlAttribute("rotation")]
    public float rotation { set; get; }

    [XmlElement("ellipse")]
    public TiledEllipse ellipse { get; set; }

    [XmlElement("polygon")]
    public TiledPolygon polygon { get; set; }

    [XmlElement("polyline")]
    public TiledPolyline polyline { get; set; }

}