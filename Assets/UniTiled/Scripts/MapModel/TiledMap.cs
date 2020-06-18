using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

// https://doc.mapeditor.org/en/stable/reference/tmx-map-format/
// https://github.com/bjorn/tiled/tree/master/docs/reference

[System.Serializable]
[XmlRoot("map")]
public class TiledMap
{
    [XmlAttribute("version")]
    public string version { get; set; }

    [XmlAttribute("tiledversion")]
    public string tiledVersion { get; set; }

    [XmlAttribute("orientation")]
    public string orientation { get; set; }

    [XmlAttribute("renderorder")]
    public string renderOrder { get; set; }

    [XmlAttribute("compressionlevel")]
    public int compressionLevel { get; set; }
    [DefaultValue(-1)]

    [XmlAttribute("width")]
    public int width { get; set; }

    [XmlAttribute("height")]
    public int height { get; set; }

    [XmlAttribute("tilewidth")]
    public int tileWidth { get; set; }

    [XmlAttribute("tileheight")]
    public int tileHeight { get; set; }

    [XmlAttribute("hexsidelength")]
    [DefaultValue(0)]
    public int hexSideLength { get; set; }

    [XmlAttribute("staggeraxis")]
    [DefaultValue("x")]
    public string staggerAxis { get; set; }

    [XmlAttribute("staggerindex")]
    public string staggerIndex { get; set; }

    [XmlAttribute("backgroundcolor")]
	[DefaultValue("")]
    public string backgroundColor { get; set; }

    [XmlAttribute("nextlayerid")]
    public int nextLayerID { get; set; }

    [XmlAttribute("nextobjectid")]
    public int nextObjectID { get; set; }

    [XmlAttribute("infinite")]
    [DefaultValue(0)]
    public int infinite { get; set; }

    [XmlArray("properties")]
    [XmlArrayItem("property", typeof(TiledProperty))]
    public TiledProperty[] properties { get; set; }

    [XmlElement("tileset")]
    public TiledTileSetElem[] tileSetEntries { get; set; }

    public TiledTileSetFile[] tileSets { get; set; }

    [XmlElement("layer")]
    public TiledLayer[] layers { get; set; }

    [XmlElement("objectgroup")]
    public TiledObjectGroup[] objectGroups { get; set; }

    [XmlElement("imagelayer")]
    public TiledImageLayer[] imageLayers { get; set; }

    [XmlElement("group")]
    public TiledGroup[] groups { get; set; }

    [XmlElement("editorsettings")]
    public TiledEditorSettings[] editorSettings { get; set; }
}