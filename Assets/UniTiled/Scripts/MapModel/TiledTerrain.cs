using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledTerrain
{
    [System.Xml.Serialization.XmlAttribute("name")]
    public string name { set; get; }

    [System.Xml.Serialization.XmlAttribute("tile")]
    public int tile { set; get; }
}