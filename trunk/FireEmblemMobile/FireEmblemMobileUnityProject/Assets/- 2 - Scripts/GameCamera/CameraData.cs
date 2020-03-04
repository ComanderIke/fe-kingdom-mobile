using UnityEngine;

namespace Assets.GameCamera
{
    [CreateAssetMenu(menuName = "GameData/CameraSettings", fileName = "CameraSettings1")]
    public class CameraData : ScriptableObject
    {
        public float CameraBoundsBorder = 0;
    }
}