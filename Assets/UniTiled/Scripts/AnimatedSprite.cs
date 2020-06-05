using UnityEngine;
using System.Collections;

/// <summary>
/// Animates a sprite using an array of AnimationFrame objects
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    /// <summary>
    /// Frames of the animation
    /// </summary>
    public AnimationFrame[] frames;

    /// <summary>
    /// Internal time accumulator
    /// </summary>
    private float time = 0.0f;

    /// <summary>
    /// Internal index of current frame
    /// </summary>
    private int index = 0;

    /// <summary>
    /// Reference to the attached sprite renderer
    /// </summary>
    private SpriteRenderer sr;

    /// <summary>
    /// Starts by setting reference to sprite renderer
    /// and displaying the selected sprite index on it
    /// </summary>
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = frames[index].sprite;
    }

    /// <summary>
    /// Updates by accumulating delta time to internal
    /// time accumulator and switching to the next frame if necessary
    /// </summary>
    void Update()
    {
        time += Time.deltaTime * 1000;
        if (time >= frames[index].duration)
        {
            time = 0.0f;
            index = (index + 1) % frames.Length;
            sr.sprite = frames[index].sprite;
        }
    }
}

/// <summary>
/// A single frame in a sprite animation
/// </summary>
[System.Serializable]
public struct AnimationFrame
{
    /// <summary>
    /// Sprite of the frame
    /// </summary>
    public Sprite sprite;
    /// <summary>
    /// Duration of the frame in seconds
    /// </summary>
    public int duration;
}