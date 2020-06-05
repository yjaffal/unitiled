using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledLayerData
{
    [XmlAttribute("encoding")]
    public string encoding { set; get; }

    [XmlText]
    public string Value { set; get; }
}