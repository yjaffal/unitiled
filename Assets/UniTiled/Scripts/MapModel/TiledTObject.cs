using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledTObject
{
    [System.Xml.Serialization.XmlAttribute("id")]
    public int id { set; get; }

    [System.Xml.Serialization.XmlAttribute("x")]
    public float x { set; get; }

    [System.Xml.Serialization.XmlAttribute("y")]
    public float y { set; get; }

    [System.Xml.Serialization.XmlAttribute("width")]
    public float width { set; get; }

    [System.Xml.Serialization.XmlAttribute("height")]
    public float height { set; get; }

    [System.Xml.Serialization.XmlAttribute("rotation")]
    public float rotation { set; get; }

    [XmlElement("ellipse")]
    public TiledEllipse ellipse { get; set; }

    [XmlElement("polygon")]
    public TiledPolygon polygon { get; set; }

    [XmlElement("polyline")]
    public TiledPolyline polyline { get; set; }

}