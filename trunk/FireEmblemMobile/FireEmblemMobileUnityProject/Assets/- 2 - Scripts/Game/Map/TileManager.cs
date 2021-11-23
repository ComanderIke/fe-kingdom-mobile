using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TileData = Game.Map.TileData;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;

    [SerializeField]
    private List<Tilemap> tilemaps;
    [SerializeField]
    private TileData standardTileData;
    [SerializeField]
    private List<TileData> tileDatas;

    private Dictionary<TileBase, TileData> dataFromTiles;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        dataFromTiles = new Dictionary<TileBase, TileData>();
        foreach (var tileData in tileDatas)
        {
            if(tileData.tiles!=null&&tileData.tiles.Length>0)
                foreach (var tile in tileData.tiles)
                {
                    dataFromTiles.Add(tile, tileData);
                }
        }
    }

    public TileData GetData(int x, int y)
    {
        foreach (var tilemap in tilemaps)
        {
            TileBase tileBase = tilemap.GetTile(new Vector3Int(x, y, 0));
            if (tileBase == null)
                continue;
            if (dataFromTiles.ContainsKey(tileBase))
            {
                //Debug.Log("tileBase: "+tileBase.name+" "+dataFromTiles[tileBase].name);
                return dataFromTiles[tileBase];
            }
        }

     
        return standardTileData;
    }
}
