using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledTObject
{
    [XmlAttribute("id")]
    public int id { set; get; }

    [XmlAttribute("name")]
    [DefaultValue("")]
    public string name { set; get; }

    [XmlAttribute("type")]
    [DefaultValue("")]
    public string type { set; get; }

    [XmlAttribute("x")]
    [DefaultValue(0)]
    public float x { set; get; }

    [XmlAttribute("y")]
    [DefaultValue(0)]
    public float y { set; get; }

    [XmlAttribute("width")]
    [DefaultValue(0)]
    public float width { set; get; }

    [XmlAttribute("height")]
    [DefaultValue(0)]
    public float height { set; get; }

    [XmlAttribute("rotation")]
    [DefaultValue(0)]
    public float rotation { set; get; }

    [XmlAttribute("gid")]
    [DefaultValue(0)]
    public float gid { set; get; }

    [XmlAttribute("visible")]
    [DefaultValue(1)]
    public int visible { set; get; }

    [XmlElement("template")]
    public string template { get; set; }

    [XmlElement("ellipse")]
    public TiledEllipse ellipse { get; set; }

    [XmlElement("polygon")]
    public TiledPolygon polygon { get; set; }

    [XmlElement("polyline")]
    public TiledPolyline polyline { get; set; }

}