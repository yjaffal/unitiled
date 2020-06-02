using UnityEngine;
using System.Collections;

/// <summary>
/// Used to deserialize the custom properties of a layer
/// as specified in the tmx file
/// </summary>
public class LayerCustomProperties : MonoBehaviour
{

    /// <summary>
    /// The preset constant y value of layer's position
    /// </summary>
    [HideInInspector]
    public float height = float.MinValue;
}
