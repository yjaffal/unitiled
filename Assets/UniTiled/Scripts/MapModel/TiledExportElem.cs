using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledExportElem
{
    [XmlAttribute("target")]
    public string target { get; set; }

    [XmlAttribute("format")]
    public string format { get; set; }
}