using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledFrame
{
    [System.Xml.Serialization.XmlAttribute("tileid")]
    public int tileID { set; get; }

    [System.Xml.Serialization.XmlAttribute("duration")]
    public int duration { set; get; }
}