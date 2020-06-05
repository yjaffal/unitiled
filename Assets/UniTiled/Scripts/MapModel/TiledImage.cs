using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledImage
{
    [XmlAttribute("width")]
    public int width { set; get; }

    [XmlAttribute("height")]
    public int height { set; get; }

    [XmlAttribute("source")]
    public string source { set; get; }
}