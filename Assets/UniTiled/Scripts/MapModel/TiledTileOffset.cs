using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledTileOffset
{
    [XmlAttribute("x")]
	[DefaultValue(0)]
    public int x { get; set; }

    [XmlAttribute("y")]
	[DefaultValue(0)]
    public int y { get; set; }
}