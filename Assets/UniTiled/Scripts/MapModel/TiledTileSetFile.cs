using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
[XmlRoot("tileset")]
public class TiledTileSetFile
{
    [XmlAttribute("version")]
    public string version { get; set; }

    [XmlAttribute("tiledversion")]
    public string tiledVersion { get; set; }

    [XmlAttribute("name")]
    public string name { get; set; }

    [XmlAttribute("tilewidth")]
    public int tileWidth { get; set; }

    [XmlAttribute("tileheight")]
    public int tileHeight { get; set; }

    [XmlAttribute("tilecount")]
    public int tileCount { get; set; }

    [XmlAttribute("columns")]
    public int columns { get; set; }

    [XmlAttribute("backgroundcolor")]
	[DefaultValue("")]
    public string backgroundColor { get; set; }

    [XmlAttribute("spacing")]
    public int spacing { get; set; }

    [XmlAttribute("margin")]
    public int margin { get; set; }

    [XmlArray("tileoffset")]
    [XmlArrayItem("tileoffset", typeof(TiledTileOffset))]
    public TiledTileOffset[] tileOffset { get; set; }

    [XmlArray("properties")]
    [XmlArrayItem("property", typeof(TiledCustomProperty))]
    public TiledCustomProperty[] customProperties { get; set; }

    [XmlArray("terraintypes")]
    [XmlArrayItem("terrain", typeof(TiledTerrain))]
    public TiledTerrain[] terrainTypes { get; set; }

    [XmlElement("tile")]
    public TiledTile[] tiles { get; set; }

    [XmlElement("image")]
    public TiledImage image { get; set; }
}