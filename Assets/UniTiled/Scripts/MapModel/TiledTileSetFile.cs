using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
[System.Xml.Serialization.XmlRoot("tileset")]
public class TiledTileSetFile
{
    [System.Xml.Serialization.XmlAttribute("name")]
    public string name { get; set; }

    [System.Xml.Serialization.XmlAttribute("tilewidth")]
    public int tileWidth { get; set; }

    [System.Xml.Serialization.XmlAttribute("tileheight")]
    public int tileHeight { get; set; }

    [System.Xml.Serialization.XmlAttribute("tilecount")]
    public int tileCount { get; set; }

    [System.Xml.Serialization.XmlAttribute("columns")]
    public int columns { get; set; }

    [System.Xml.Serialization.XmlAttribute("spacing")]
    public int spacing { get; set; }

    [System.Xml.Serialization.XmlAttribute("margin")]
    public int margin { get; set; }

    [XmlArray("terraintypes")]
    [XmlArrayItem("terrain", typeof(TiledTerrain))]
    public TiledTerrain[] terrainTypes { get; set; }

    [XmlElement("tile")]
    public TiledTile[] tiles { get; set; }

    [XmlElement("image")]
    public TiledImage image { get; set; }
}