using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledLayer
{
    [XmlAttribute("name")]
    public string name { set; get; }

    [XmlAttribute("width")]
    public int width { set; get; }

    [XmlAttribute("height")]
    public int height { set; get; }

    [XmlAttribute("visible")]
    [DefaultValue(1)]
    public int visible { set; get; }

    [XmlAttribute("locked")]
    [DefaultValue(0)]
    public int locked { set; get; }

    [XmlAttribute("opacity")]
    [DefaultValue(1.0f)]
    public float opacity { set; get; }

    [XmlAttribute("offsetx")]
    [DefaultValue(0.0f)]
    public float offsetX { set; get; }

    [XmlAttribute("offsety")]
    [DefaultValue(0.0f)]
    public float offsetY { set; get; }

    [XmlElement("data")]
    public TiledLayerData data { set; get; }

    [XmlArray("properties")]
    [XmlArrayItem("property", typeof(TiledCustomProperty))]
    public TiledCustomProperty[] customProperties { get; set; }
}