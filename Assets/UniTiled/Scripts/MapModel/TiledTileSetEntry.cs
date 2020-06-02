using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledTileSetEntry
{
    [System.Xml.Serialization.XmlAttribute("firstgid")]
    public int firstGID { get; set; }

    [System.Xml.Serialization.XmlAttribute("source")]
    public string source { get; set; }
}