using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledGroup : TiledMapLayer
{
    [XmlElement("layer")]
    public TiledLayer[] layers { get; set; }

    [XmlElement("objectgroup")]
    public TiledObjectGroup[] objectGroups { get; set; }

    [XmlElement("imagelayer")]
    public TiledImageLayer[] imageLayers { get; set; }

    [XmlElement("group")]
    public TiledGroup[] groups { get; set; }
}