using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledImage
{
    [XmlAttribute("format")]
    public string format { set; get; }

    // depricated since Tiled Java; ignore if set
    [XmlAttribute("id")]
    [DefaultValue(-1)]
    public int id { set; get; }

    [XmlAttribute("source")]
    [DefaultValue("")]
    public string source { set; get; }

    [XmlAttribute("trans")]
    public string trans { set; get; }

    [XmlAttribute("width")]
    public int width { set; get; }

    [XmlAttribute("height")]
    public int height { set; get; }

    [XmlElement("data")]
    public TiledData[] data { get; set; }
}