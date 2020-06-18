using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
[XmlRoot("template")]
public class TiledTemplate
{
    [XmlElement("tileset")]
    public TiledTileSetElem[] tileSetEntries { get; set; }

    [XmlElement("object")]
    public TiledTObject[] objects { get; set; }
}