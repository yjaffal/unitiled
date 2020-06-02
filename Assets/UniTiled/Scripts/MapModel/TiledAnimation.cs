using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledAnimation
{

    [XmlElement("frame")]
    public TiledFrame[] frames { get; set; }
}