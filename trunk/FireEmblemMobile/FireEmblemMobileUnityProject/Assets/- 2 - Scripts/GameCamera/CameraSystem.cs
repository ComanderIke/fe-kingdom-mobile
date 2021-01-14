using System;
using System.Collections.Generic;
using GameEngine;
using UnityEngine;

namespace GameCamera
{
    public class CameraSystem : MonoBehaviour, IEngineSystem
    {
        public delegate void OnMoveToFinishedEvent();

        public static OnMoveToFinishedEvent MoveToFinishedEvent;
        public new Camera camera;
        private List<CameraMixin> mixins;
        public CameraData cameraData;
        public void Init()
        {
            mixins = new List<CameraMixin>();
        }

        public T AddMixin<T>()
        {
            if (!typeof(CameraMixin).IsAssignableFrom(typeof(T)))
                throw new Exception("Parameter was not of type CameraMixin");
            var mixin = (CameraMixin)gameObject.AddComponent(typeof(T));
            mixin.CameraSystem = this;
            mixins.Add(mixin);
            return (T)Convert.ChangeType(mixin, typeof(T));
        }

        public void RemoveMixin<T>()
        {
            var mixin = (CameraMixin)GetComponent(typeof(T));
            mixins.Remove(mixin);
            Destroy(GetComponent(mixin.GetType()));
        }

        public void DeactivateOtherMixins(CameraMixin mixin)
        {
            foreach (var m in mixins)
                if (m == mixin || m.IsLocked())
                    continue;
                else
                    m.enabled = false;
        }

        public void ActivateMixins()
        {
            foreach (var m in mixins) m.enabled = true;
        }

        public void ActivateMixin<T>()
        {
            var mixin = (CameraMixin)GetComponent(typeof(T));
            mixin.enabled = true;
        }
        public void DeactivateMixin<T>()
        {
            var mixin = (CameraMixin)GetComponent(typeof(T));
            mixin.enabled = false;
        }
    }
}