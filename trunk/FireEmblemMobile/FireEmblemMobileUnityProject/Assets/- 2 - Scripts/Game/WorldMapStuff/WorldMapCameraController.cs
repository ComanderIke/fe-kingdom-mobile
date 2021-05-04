using System.Collections;
using System.Collections.Generic;
using GameCamera;
using GameEngine.Input;
using GameEngine.Tools;
using UnityEngine;

public class WorldMapCameraController : MonoBehaviour
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
        cameraSystem.AddMixin<DragCameraMixin>().Construct(new WorldPosDragPerformer(1f, cameraSystem.camera),
            new ScreenPointToRayProvider(cameraSystem.camera), new HitChecker(),new MouseInputProvider());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
