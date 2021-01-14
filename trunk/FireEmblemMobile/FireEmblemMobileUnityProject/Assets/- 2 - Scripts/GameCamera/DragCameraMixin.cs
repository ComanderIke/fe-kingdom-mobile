using GameEngine.Input;
using GameEngine.Tools;
using UnityEngine;

namespace GameCamera
{
    public class DragCameraMixin : CameraMixin
    {
        private IDragPerformer DragPerformer { get; set; }
        private IRayProvider RayProvider { get; set; }
        private IHitChecker HitChecker { get; set; }
        private IInputProvider InputProvider { get; set; }
        

        public void Construct(IDragPerformer dragPerformer, IRayProvider rayProvider, IHitChecker hitChecker,
            IInputProvider inputProvider)
        {
            DragPerformer = dragPerformer;
            RayProvider = rayProvider;
            HitChecker = hitChecker;
            InputProvider = inputProvider;
        }

        private void Update()
        {
            CameraSystem.IsDragging = DragPerformer.IsDragging;
            if (InputProvider.InputPressed())
            {
                DragPerformer.Drag(transform,InputProvider.InputPosition());
                
            }
            if (InputProvider.InputPressedDown())
            {
                var ray = RayProvider.CreateRay(InputProvider.InputPosition());
                if (HitChecker.CheckHit(ray))
                {
                    DragPerformer.StartDrag(transform,InputProvider.InputPosition());
                }
            }
            if (InputProvider.InputPressedUp())
            {
                DragPerformer.EndDrag(transform);
                CameraSystem.ActivateMixins();
            }
        }
    }
}