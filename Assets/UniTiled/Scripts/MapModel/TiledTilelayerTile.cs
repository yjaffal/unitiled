using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledTilelayerTile
{
    [XmlAttribute("gid")]
    [DefaultValue(0)]
    public int gid { set; get; }
}