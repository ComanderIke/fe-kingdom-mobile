using System;
using System.Collections.Generic;
using Game.DataAndReferences.Data;
using UnityEngine;

namespace Game.Map
{
    [CreateAssetMenu(menuName = "GameData/TileDataContainer")]
    public class TileDataContainer : ScriptableObject
    {
        [SerializeField]
        public TileData standardTileData;
        [SerializeField]
        public TileData[] tileDatas;
#if UNITY_EDITOR
        public void OnValidate()
        {
            tileDatas= GameBPData.GetAllInstances<TileData>();
        }
        #endif
    }
}