using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
[System.Xml.Serialization.XmlRoot("map")]
public class TiledMap
{

    [System.Xml.Serialization.XmlAttribute("version")]
    public string version { get; set; }

    [System.Xml.Serialization.XmlAttribute("orientation")]
    public string oriantation { get; set; }

    [System.Xml.Serialization.XmlAttribute("renderorder")]
    public string renderOrder { get; set; }

    [System.Xml.Serialization.XmlAttribute("width")]
    public int width { get; set; }

    [System.Xml.Serialization.XmlAttribute("height")]
    public int height { get; set; }

    [System.Xml.Serialization.XmlAttribute("tilewidth")]
    public int tileWidth { get; set; }

    [System.Xml.Serialization.XmlAttribute("tileheight")]
    public int tileHeight { get; set; }

    [System.Xml.Serialization.XmlAttribute("nextobjectid")]
    public int nextObjectID { get; set; }

    [XmlElement("tileset")]
    public TiledTileSetEntry[] tileSetEntries { get; set; }

    public TiledTileSetFile[] tileSets { get; set; }

    [XmlElement("layer")]
    public TiledLayer[] layers { get; set; }
}

