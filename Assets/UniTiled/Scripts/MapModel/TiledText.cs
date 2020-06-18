using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledText
{
    [XmlAttribute("fontfamily")]
    [DefaultValue("sans-serif")]
    public string fontFamily { set; get; }

    [XmlAttribute("pixelsize")]
    [DefaultValue(16)]
    public int pixelSize { set; get; }

    [XmlAttribute("wrap")]
    [DefaultValue(1)]
    public int wrap { set; get; }

    [XmlAttribute("color")]
    [DefaultValue("#000000")]
    public string color { set; get; }

    [XmlAttribute("bold")]
    [DefaultValue(0)]
    public int bold { set; get; }

    [XmlAttribute("italic")]
    [DefaultValue(0)]
    public int italic { set; get; }

    [XmlAttribute("underline")]
    [DefaultValue(0)]
    public int underline { set; get; }

    [XmlAttribute("strikeout")]
    [DefaultValue(0)]
    public int strikeout { set; get; }

    [XmlAttribute("kerning")]
    [DefaultValue(1)]
    public int kerning { set; get; }

    [XmlAttribute("halign")]
    [DefaultValue("left")]
    public string halign { set; get; }

    [XmlAttribute("valign")]
    [DefaultValue("top")]
    public string valign { set; get; }

    [XmlText]
    public string Value { set; get; }
}