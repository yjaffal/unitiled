using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledLayerData
{
    [System.Xml.Serialization.XmlAttribute("encoding")]
    public string encoding { set; get; }

    [System.Xml.Serialization.XmlText]
    public string Value { set; get; }
}