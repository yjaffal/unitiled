using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledWangSet
{
    [XmlAttribute("name")]
    [DefaultValue("")]
    public string name { set; get; }

    [XmlAttribute("tile")]
    public int tile { set; get; }

    [XmlArray("properties")]
    [XmlArrayItem("property", typeof(TiledProperty))]
    public TiledProperty[] customProperties { get; set; }

    [XmlElement("wangcornercolor", typeof(TiledWangCornerColor))]
    public TiledWangCornerColor[] wangCornerColors { set; get; }

    [XmlElement("wangedgecolor", typeof(TiledWangEdgeColor))]
    public TiledWangEdgeColor[] wangEdgeColors { set; get; }

    [XmlElement("wangtile", typeof(TiledWangTile))]
    public TiledWangTile[] wangTiles { set; get; }

}