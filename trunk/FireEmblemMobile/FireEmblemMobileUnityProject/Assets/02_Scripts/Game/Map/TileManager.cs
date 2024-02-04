using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.Map;
using LostGrace;
using UnityEngine;
using UnityEngine.Tilemaps;
using Tile = Game.Grid.Tile;
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

    [SerializeField] private Transform propsParent;
    private List<PropOnGrid> props;
    private List<DestroyableController> destroyables;
    private List<GlowSpot> glowSpots;

    private Dictionary<TileBase, TileData> dataFromTiles;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        dataFromTiles = new Dictionary<TileBase, TileData>();
        props = new List<PropOnGrid>();
        destroyables = new List<DestroyableController>();
        glowSpots = new List<GlowSpot>();
        if(propsParent!=null)
            foreach (var prop in propsParent.GetComponentsInChildren<PropOnGrid>())
            {
                props.Add(prop);
            }
        foreach (var destroyable in FindObjectsOfType<DestroyableController>())
        {
            destroyables.Add(destroyable);
        }
        foreach (var glowSpot in FindObjectsOfType<GlowSpot>())
        {
            glowSpots.Add(glowSpot);
        }
        foreach (var tileData in tileDatas)
        {
            if(tileData.tiles!=null&&tileData.tiles.Length>0)
                foreach (var tile in tileData.tiles)
                {
                    dataFromTiles.Add(tile, tileData);
                }
        }
    }

    public void InitGlowSpots(Tile[,] tiles)
    {
        foreach (var glowSpot in glowSpots)
        {
            tiles[glowSpot.X, glowSpot.Y].SetGlowSpot(glowSpot);
        }
    }

    public GridTerrainData GetData(int x, int y)
    {
        foreach (var destroyable in destroyables)
        {
            if (destroyable.IsOnPosition(x,y))
                    return destroyable.Destroyable.TerrainData;
        }
        foreach (var prop in props)
        {
            if (prop.IsOnPosition(x, y))
                return prop.terrainData;
        }
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
