using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class TiledEditorSettings
{
    [XmlElement("chunksize")]
    public TiledChunkSize[] chunkSizes { get; set; }

    [XmlElement("export")]
    public TiledExportElem[] exportElem { get; set; }
}