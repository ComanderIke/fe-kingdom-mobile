using Game.GameInput.CameraInput;
using Game.Utility;
using GameCamera;
using UnityEngine;

namespace Game.EncounterAreas.Controller
{
    public class EncounterAreaCameraController : MonoBehaviour
    {
        [SerializeField]private float width;

        [SerializeField]private float height;
    
        private CameraSystem cameraSystem;
        // Start is called before the first frame update
        void Awake()
        {

            cameraSystem = FindObjectOfType<CameraSystem>();
            cameraSystem.Init();
            cameraSystem.AddMixin<ClampCameraMixin>().Construct(width, height);
            cameraSystem.AddMixin<FocusCameraMixin>().Construct();
            cameraSystem.ActivateMixin<FocusCameraMixin>();
            cameraSystem.AddMixin<DragCameraMixin>().Construct(new WorldPosDragPerformer(1f, cameraSystem.camera),
                new ScreenPointToRayProvider(cameraSystem.camera), new HitChecker(),new MouseCameraInputProvider());
       

        }

    
        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
