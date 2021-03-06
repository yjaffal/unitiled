using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledTerrain
{
    [XmlAttribute("name")]
    public string name { set; get; }

    [XmlAttribute("tile")]
    public int tile { set; get; }

    [XmlArray("properties")]
    [XmlArrayItem("property", typeof(TiledProperty))]
    public TiledProperty[] customProperties { get; set; }
}