using GameEngine.Input;
using GameEngine.Tools;
using UnityEngine.EventSystems;

namespace GameCamera
{
    public class DragCameraMixin : CameraMixin
    {
        private IDragPerformer DragPerformer { get; set; }
        private IRayProvider RayProvider { get; set; }
        private IHitChecker HitChecker { get; set; }
        private ICameraInputProvider CameraInputProvider { get; set; }


        public void Construct(IDragPerformer dragPerformer, IRayProvider rayProvider, IHitChecker hitChecker,
            ICameraInputProvider cameraInputProvider)
        {
            DragPerformer = dragPerformer;
            RayProvider = rayProvider;
            HitChecker = hitChecker;
            CameraInputProvider = cameraInputProvider;
        }

        private void Update()
        {
            CameraSystem.IsDragging = DragPerformer.IsDragging;
            if (CameraInputProvider.InputPressed())
            {
                DragPerformer.Drag(transform,CameraInputProvider.InputPosition());
                
            }
            if (CameraInputProvider.InputPressedDown())
            {
                var ray = RayProvider.CreateRay(CameraInputProvider.InputPosition());
                if (HitChecker.CheckHit(ray) )
                {
                    // if (EventSystem.current.currentSelectedGameObject == null ||
                    //     !HitChecker.HasTagExcluded(EventSystem.current
                    //         .currentSelectedGameObject.tag))
                    if(!EventSystem.current.IsPointerOverGameObject())
                    {
                        DragPerformer.StartDrag(transform, CameraInputProvider.InputPosition());
                    }
                }
            }
            if (CameraInputProvider.InputPressedUp())
            {
                DragPerformer.EndDrag(transform);
                CameraSystem.ActivateMixins();
            }
        }
    }
}