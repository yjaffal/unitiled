using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledImageLayer : TiledMapLayer
{
    [XmlElement("image")]
    public TiledImage image { get; set; }
}