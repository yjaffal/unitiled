using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Sorts 3D models in a grid format to prepare them for
/// sprite snapshots -- runs in edit mode. All 3D models must
/// be added as children to the object holding this script
/// </summary>
[ExecuteInEditMode]
public class Grid3DMaker : MonoBehaviour
{
    /// <summary>
    /// Scale of the largest possible tile
    /// </summary>
    public Vector3 tileSize = new Vector3(1, 1, 1);

    /// <summary>
    /// Vertical and horizontal spacing between tiles in (x,z) coordinates
    /// </summary>
    public float tileSpacing = 3;

    /// <summary>
    /// Used as reference scale for generated tiles.
    /// Use a tile that has standard scale and has identical width and height
    /// </summary>
    public Transform sizeReference;

    /// <summary>
    /// Whether the original pivot of the 3D model should be
    /// recalculated before positioning -- needs to be disabled
    /// in some cases in order to get the correct result
    /// </summary>

    [HideInInspector]
    public float maxDiff = 0;

    /// <summary>
    /// Runs in edit mode and checks if refresh is selected and
    /// in that case fixes the pivots (if enabled) and then refreshes
    /// the grid based on the selected values
    /// </summary>
    public void Refresh()
    {
        maxDiff = 0;
        if (sizeReference == null)
        {
            Debug.LogWarning("Reference transform needs to be set to a tile...");
            return;
        }
        if (FixPivots())
        {
            FixPivots();
        }
        RefreshGrid();
        if (Camera.main != null)
        {
            DestroyImmediate(Camera.main.gameObject);
        }

        GameObject prevCam = GameObject.Find("Tile Preview Cam");
        if (prevCam == null)
        {
            prevCam = new GameObject("Tile Preview Cam");
        }

        Camera prev = prevCam.GetComponent<Camera>();
        if (prev == null)
        {
            prev = prevCam.AddComponent<Camera>();
        }

        prev.orthographic = true;
        prev.depth = 1;
    }

    /// <summary>
    /// Repositions all models on the grid using the specified
    /// distance values and scales
    /// </summary>
    private void RefreshGrid()
    {
        int cols = (int)Mathf.Sqrt(transform.childCount);
        int col = 0;
        Vector3 pos = Vector3.zero;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).position = Vector3.zero;

            transform.GetChild(i).position = pos;
            pos.x += tileSize.x + tileSpacing;
            col++;
            if (col > cols)
            {
                pos.x = 0;
                pos.z -= tileSize.z + tileSpacing;
                col = 0;
            }
        }
    }

    /// <summary>
    /// Recalculates the center point of the models based on
    /// their settings in the attached Tile3D script and its geometry
    /// </summary>
    private bool FixPivots()
    {
        List<Transform> newParents = new List<Transform>();
        List<Transform> newChildren = new List<Transform>();
        List<Transform> processLater = new List<Transform>();
        for (int child = 0; child < transform.childCount; child++)
        {
            Transform meshTransform = transform.GetChild(child);
            MeshFilter filter = meshTransform.GetComponent<MeshFilter>();

            if (filter != null)
            {
                GameObject emptyParent = new GameObject();
                emptyParent.transform.position = meshTransform.position;

                emptyParent.name = meshTransform.name;
                if (meshTransform == sizeReference)
                {
                    sizeReference = emptyParent.transform;
                }

                Tile3D oldScript = meshTransform.GetComponent<Tile3D>();
                Tile3D newScript = emptyParent.AddComponent<Tile3D>();
                if (oldScript != null)
                {
                    newScript.alignTo = oldScript.alignTo;
                    DestroyImmediate(oldScript);
                }
                newChildren.Add(meshTransform);
                newParents.Add(emptyParent.transform);
            }
            else if (meshTransform.GetComponent<Tile3D>() == null)
            {
                processLater.Add(meshTransform);
            }
        }

        foreach (Transform child in processLater)
        {
            GameObject emptyParent = new GameObject();
            emptyParent.transform.position = child.position;
            if (child == sizeReference)
            {
                sizeReference = emptyParent.transform;
            }
            emptyParent.transform.parent = transform;
            emptyParent.name = child.name;
            while (child.childCount != 0)
            {
                child.GetChild(0).parent = emptyParent.transform;
            }
        }

        foreach (Transform child in processLater)
        {
            DestroyImmediate(child.gameObject);
        }

        if (newParents.Count > 0)
        {
            for (int i = 0; i < newParents.Count; i++)
            {
                newChildren[i].parent = newParents[i];
                newParents[i].parent = transform;
            }

            return true;
        }

        Dictionary<int, Vector3[]> borders = new Dictionary<int, Vector3[]>();
        Dictionary<int, Vector3[]> allShifts = new Dictionary<int, Vector3[]>();

        for (int child = 0; child < transform.childCount; child++)
        {
            float minX = float.MaxValue, minZ = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxZ = float.MinValue, maxY = float.MinValue;

            Transform parentTransform = transform.GetChild(child);
            parentTransform.localScale = Vector3.one;
            Transform childTransform;

            Tile3D tileScript = parentTransform.GetComponent<Tile3D>();

            if (tileScript == null)
            {
                tileScript = parentTransform.gameObject.AddComponent<Tile3D>();
            }

            Vector3[] shifts = new Vector3[parentTransform.childCount];
            Vector3 shiftPivot = Vector3.zero;
            for (int i = 0; i < parentTransform.childCount; i++)
            {
                childTransform = parentTransform.GetChild(i);
                Collider col = childTransform.GetComponent<Collider>();

                if (i == 0)
                {
                    shifts[i] = Vector3.zero;
                    shiftPivot = childTransform.localPosition;
                }
                else
                {
                    shifts[i] = childTransform.localPosition - shiftPivot;
                }

                childTransform.localPosition = Vector3.zero;

                if (col == null)
                {
                    if (tileScript.generateCollider)
                    {
                        col = childTransform.gameObject.AddComponent<MeshCollider>();
                    }
                }
                else
                {
                    if (!tileScript.generateCollider)
                    {
                        DestroyImmediate(col);
                    }
                }

                MeshFilter filter = childTransform.GetComponent<MeshFilter>();

                if (filter != null)
                {
                    Mesh mesh = filter.sharedMesh;
                    Vector3 min = shifts[i] + new Vector3(
                        mesh.bounds.min.x * childTransform.localScale.x,
                        mesh.bounds.min.y * childTransform.localScale.y,
                        mesh.bounds.min.z * childTransform.localScale.z
                        );
                    Vector3 max = shifts[i] + new Vector3(
                        mesh.bounds.max.x * childTransform.localScale.x,
                        mesh.bounds.max.y * childTransform.localScale.y,
                        mesh.bounds.max.z * childTransform.localScale.z
                        );
                    if (max.x > maxX)
                    {
                        maxX = max.x;
                    }
                    if (min.x < minX)
                    {
                        minX = min.x;
                    }
                    if (max.y > maxY)
                    {
                        maxY = max.y;
                    }
                    if (min.y < minY)
                    {
                        minY = min.y;
                    }
                    if (max.z > maxZ)
                    {
                        maxZ = max.z;
                    }
                    if (min.z < minZ)
                    {
                        minZ = min.z;
                    }
                }
            }

            float dx = maxX - minX;
            float dy = maxY - minY;
            float dz = maxZ - minZ;

            tileScript.height = dy;

            if (parentTransform == sizeReference)
            {
                if (dx > dz)
                {
                    maxDiff = dx;
                }
                else
                {
                    maxDiff = dz;
                }
            }

            Vector3 shapeCenter =
                            new Vector3(
                                minX + (maxX - minX) * 0.5f,
                                minY,
                                minZ + (maxZ - minZ) * 0.5f
                            );
            borders.Add(child, new Vector3[]{
                new Vector3(minX, minY, minZ),
                new Vector3(maxX, maxY, maxZ),
                shapeCenter
            });

            allShifts.Add(child, shifts);
        }

        for (int child = 0; child < transform.childCount; child++)
        {
            Transform parentTransform = transform.GetChild(child);
            Vector3 min = borders[child][0];
            Vector3 max = borders[child][1];
            Vector3 shapeCenter = borders[child][2];
            Vector3[] shifts = allShifts[child];
            Tile3D tileScript = parentTransform.GetComponent<Tile3D>();

            for (int i = 0; i < parentTransform.childCount; i++)
            {
                Transform trans = parentTransform.GetChild(i);
                trans.Translate(-shapeCenter);
                trans.Translate(shifts[i]);

                if (tileScript.alignTo == Alignment.left ||
                    tileScript.alignTo == Alignment.lowerLeft ||
                    tileScript.alignTo == Alignment.upperLeft)
                {
                    trans.Translate(-(maxDiff * 0.5f - (shapeCenter.x - min.x)), 0, 0);
                }
                else if (tileScript.alignTo == Alignment.right ||
                    tileScript.alignTo == Alignment.lowerRight ||
                    tileScript.alignTo == Alignment.upperRight)
                {
                    trans.Translate(maxDiff * 0.5f - (shapeCenter.x + max.x), 0, 0);
                }

                if (tileScript.alignTo == Alignment.top ||
                    tileScript.alignTo == Alignment.upperRight ||
                    tileScript.alignTo == Alignment.upperLeft)
                {
                    trans.Translate(0, 0, maxDiff * 0.5f + (shapeCenter.z + max.z));
                }
                else if (tileScript.alignTo == Alignment.bottom ||
                    tileScript.alignTo == Alignment.lowerRight ||
                    tileScript.alignTo == Alignment.lowerLeft)
                {

                    trans.Translate(0, 0, -(maxDiff * 0.5f - (shapeCenter.z - min.z)));
                }
            }
        }

        return false;
    }

    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Vector3 pos = child.position;

            Vector3 start = new Vector3(pos.x - maxDiff * 0.5f, 0.0f, pos.z + maxDiff * 0.5f);
            Debug.DrawLine(start, start = start + new Vector3(maxDiff, 0.0f, 0.0f));
            Debug.DrawLine(start, start = start + new Vector3(0.0f, 0.0f, -maxDiff));
            Debug.DrawLine(start, start = start + new Vector3(-maxDiff, 0.0f, 0.0f));
            Debug.DrawLine(start, start = start + new Vector3(0.0f, 0.0f, maxDiff));
        }
    }
}