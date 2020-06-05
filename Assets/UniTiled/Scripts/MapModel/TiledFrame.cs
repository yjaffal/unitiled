using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledFrame
{
    [XmlAttribute("tileid")]
    public int tileID { set; get; }

    [XmlAttribute("duration")]
    public int duration { set; get; }
}