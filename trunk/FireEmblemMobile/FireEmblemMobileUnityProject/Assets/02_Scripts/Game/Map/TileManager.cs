using System.Collections.Generic;
using Game.GameActors.InteractableGridObjects;
using Game.Grid;
using UnityEngine;
using UnityEngine.Tilemaps;
using Tile = Game.Grid.Tiles.Tile;

namespace Game.Map
{
    public class TileManager : MonoBehaviour
    {
        public static TileManager Instance;

        [SerializeField]
        private List<Tilemap> tilemaps;
[SerializeField]
        private TileDataContainer tileDataContainer;

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

            
            foreach (var tileData in tileDataContainer.tileDatas)
            {
                if(tileData.tiles!=null&&tileData.tiles.Length>0)
                    foreach (var tile in tileData.tiles)
                    {
                        dataFromTiles.TryAdd(tile, tileData);
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
            if (GetDataFromDestroyables(x, y, out var gridTerrainData)) return gridTerrainData;
            if (GetDataFromProps(x, y, out var propTerrainData)) return propTerrainData;
            if (GetDataFromTileMaps(x, y, out var value)) return value;
            

            return tileDataContainer.standardTileData;
        }

        private bool GetDataFromDestroyables(int x, int y, out GridTerrainData gridTerrainData)
        {
            foreach (var destroyable in destroyables)
            {
                if (destroyable.IsOnPosition(x, y))
                {
                    gridTerrainData = destroyable.Destroyable.TerrainData;
                    return true;
                }
            }

            gridTerrainData = null;
            return false;
        }

        private bool GetDataFromProps(int x, int y, out GridTerrainData propTerrainData)
        {
            foreach (var prop in props)
            {
                if (prop.IsOnPosition(x, y))
                {
                    propTerrainData = prop.terrainData;
                    return true;
                }
            }

            propTerrainData = null;
            return false;
        }

        private bool GetDataFromTileMaps(int x, int y, out GridTerrainData value)
        {
            foreach (var tilemap in tilemaps)
            {
                TileBase tileBase = tilemap.GetTile(new Vector3Int(x, y, 0));
                if (tileBase == null)
                    continue;
                if (dataFromTiles.TryGetValue(tileBase, out var data))
                {
                    //Debug.Log("tileBase: "+tileBase.name+" "+dataFromTiles[tileBase].name);
                    {
                        value = data;
                        return true;
                    }
                }
            }

            value = null;

            return false;
        }
    }

   
}