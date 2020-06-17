using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledMapLayer
{
    [XmlAttribute("id")]
    public int ID { set; get; }

    [XmlAttribute("name")]
    public string name { set; get; }

    [XmlAttribute("opacity")]
    [DefaultValue(1.0f)]
    public float opacity { set; get; }

    [XmlAttribute("visible")]
    [DefaultValue(1)]
    public int visible { set; get; }

    [XmlAttribute("locked")]
    [DefaultValue(0)]
    public int locked { set; get; }

    [XmlAttribute("tintcolor")]
    [DefaultValue("")]
    public string tintColor { set; get; }

    [XmlAttribute("offsetx")]
    [DefaultValue(0.0f)]
    public float offsetX { set; get; }

    [XmlAttribute("offsety")]
    [DefaultValue(0.0f)]
    public float offsetY { set; get; }

    [XmlArray("properties")]
    [XmlArrayItem("property", typeof(TiledProperty))]
    public TiledProperty[] customProperties { get; set; }
}