using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
[XmlRoot("map")]
public class TiledMap
{
    [XmlAttribute("version")]
    public string version { get; set; }

    [XmlAttribute("tiledversion")]
    public string tiledVersion { get; set; }

    [XmlAttribute("orientation")]
    public string orientation { get; set; }

    [XmlAttribute("renderorder")]
    public string renderOrder { get; set; }

    [XmlAttribute("width")]
    public int width { get; set; }

    [XmlAttribute("height")]
    public int height { get; set; }

    [XmlAttribute("tilewidth")]
    public int tileWidth { get; set; }

    [XmlAttribute("tileheight")]
    public int tileHeight { get; set; }

    [XmlAttribute("infinite")]
	[DefaultValue(0)]
    public int infinite { get; set; }

    [XmlAttribute("backgroundcolor")]
	[DefaultValue("")]
    public string backgroundColor { get; set; }

    [XmlAttribute("nextlayerid")]
    public int nextLayerID { get; set; }

    [XmlAttribute("nextobjectid")]
    public int nextObjectID { get; set; }

    [XmlElement("tileset")]
    public TiledTileSetEntry[] tileSetEntries { get; set; }

    [XmlArray("properties")]
    [XmlArrayItem("property", typeof(TiledCustomProperty))]
    public TiledCustomProperty[] customProperties { get; set; }

    public TiledTileSetFile[] tileSets { get; set; }

    [XmlElement("layer")]
    public TiledLayer[] layers { get; set; }
}