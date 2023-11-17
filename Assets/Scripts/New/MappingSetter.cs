using UnityEngine;
using UnityEngine.Tilemaps;

public class MappingSetter : MonoBehaviour
{

    [Header("Mapping")]
    public Tilemap DarkMap;
    public Tilemap BlurredMap;
    public Tilemap BackgroundMap;
    public Tile DarkTile;
    public Tile BlurredTile;

    void Start()
    {
        #region Tilemap
        DarkMap.origin = BlurredMap.origin = BackgroundMap.origin;
        DarkMap.size = BlurredMap.size = BackgroundMap.size;

        foreach (Vector3Int p in DarkMap.cellBounds.allPositionsWithin)
        {
            DarkMap.SetTile(p, DarkTile);
        }
        foreach (Vector3Int p in BlurredMap.cellBounds.allPositionsWithin)
        {
            BlurredMap.SetTile(p, DarkTile);
        }
        #endregion

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
