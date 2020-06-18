using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledWangTile
{
    [XmlAttribute("tileid")]
    public int tileID { set; get; }

    [XmlAttribute("wangid")]
    [DefaultValue(0)]
    public int wangID { set; get; }

    [XmlAttribute("hflip")]
    [DefaultValue("false")]
    public string hflip { set; get; }

    [XmlAttribute("vflip")]
    [DefaultValue("false")]
    public string vflip { set; get; }

    [XmlAttribute("dflip")]
    [DefaultValue("false")]
    public string dflip { set; get; }
}