using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledTileSetEntry
{
    [XmlAttribute("firstgid")]
    public int firstGID { get; set; }

    [XmlAttribute("source")]
    public string source { get; set; }
}