using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledData
{
    [XmlAttribute("encoding")]
    [DefaultValue("")]
    public string encoding { set; get; }

    [XmlAttribute("compression")]
    [DefaultValue("")]
    public string compression { set; get; }

    [XmlElement("tile")]
    public TiledTilelayerTile[] tiles { get; set; }

    [XmlElement("chunk")]
    public TiledChunk[] chunks { get; set; }

    [XmlText]
    public string Value { set; get; }
}