using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledTileSetElem
{
    [XmlAttribute("firstgid")]
    public int firstGID { get; set; }

    [XmlAttribute("source")]
    public string source { get; set; }

    [XmlAttribute("name")]
    public string name { get; set; }

    [XmlAttribute("tilewidth")]
    public int tileWidth { get; set; }

    [XmlAttribute("tileheight")]
    public int tileHeight { get; set; }

    [XmlAttribute("spacing")]
    [DefaultValue(0)]
    public int spacing { get; set; }

    [XmlAttribute("margin")]
    [DefaultValue(0)]
    public int margin { get; set; }

    [XmlAttribute("tilecount")]
    public int tileCount { get; set; }

    [XmlAttribute("columns")]
    public int columns { get; set; }

    [XmlAttribute("objectalignment")]
    [DefaultValue("bottomleft")]
    public string objectAlignment { get; set; }

    [XmlElement("image")]
    public TiledImage image { get; set; }

    [XmlArray("tileoffset")]
    [XmlArrayItem("tileoffset", typeof(TiledTileOffset))]
    public TiledTileOffset[] tileOffset { get; set; }

    [XmlElement("grid")]
    public TiledGrid[] grids { get; set; }

    [XmlArray("properties")]
    [XmlArrayItem("property", typeof(TiledProperty))]
    public TiledProperty[] customProperties { get; set; }

    [XmlArray("terraintypes")]
    [XmlArrayItem("terrain", typeof(TiledTerrain))]
    public TiledTerrain[] terrainTypes { get; set; }

    [XmlElement("tile")]
    public TiledTile[] tiles { get; set; }

    [XmlArray("wangsets")]
    [XmlArrayItem("wangset", typeof(TiledWangSet))]
    public TiledWangSet[] wangsets { get; set; }
}