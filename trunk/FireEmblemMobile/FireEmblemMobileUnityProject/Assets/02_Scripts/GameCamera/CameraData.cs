using UnityEngine;
using UnityEngine.Serialization;

namespace GameCamera
{
    [CreateAssetMenu(menuName = "GameData/CameraSettings", fileName = "CameraSettings1")]
    public class CameraData : ScriptableObject
    {
        public Vector2 cameraBoundsBorder;
    }
}