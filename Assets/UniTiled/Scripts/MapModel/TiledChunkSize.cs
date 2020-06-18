using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledChunkSize
{
    [XmlAttribute("width")]
    [DefaultValue(16)]
    public int width { get; set; }

    [XmlAttribute("height")]
    [DefaultValue(16)]
    public int height { get; set; }
}