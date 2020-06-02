using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledImage
{
    [System.Xml.Serialization.XmlAttribute("width")]
    public int width { set; get; }

    [System.Xml.Serialization.XmlAttribute("height")]
    public int height { set; get; }

    [System.Xml.Serialization.XmlAttribute("source")]
    public string source { set; get; }

}