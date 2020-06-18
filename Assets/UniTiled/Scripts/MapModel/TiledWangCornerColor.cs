using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledWangCornerColor
{
    [XmlAttribute("name")]
    public string name { set; get; }

    [XmlAttribute("color")]
    public string Name { set; get; }

    [XmlAttribute("tile")]
    public int tile { set; get; }

    [XmlAttribute("probability")]
    [DefaultValue(0.0F)]
    public float probability { set; get; }
}