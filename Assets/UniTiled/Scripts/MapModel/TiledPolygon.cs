using System.Xml.Serialization;
using System.Xml;
using UnityEngine;

[System.Serializable]
public class TiledPolygon
{
    [System.Xml.Serialization.XmlAttribute("points")]
    public string points {get; set;}
    
    public Vector2[] GetPoints(){
        string[] vals = points.Split(' ');
        Vector2[] result = new Vector2[vals.Length];

        for(int i = 0; i < vals.Length; i++){
            string[] xy = vals[i].Split(',');
            result[i] = new Vector2(float.Parse(xy[0]), float.Parse(xy[1]));
        }

        return result;
    }
}