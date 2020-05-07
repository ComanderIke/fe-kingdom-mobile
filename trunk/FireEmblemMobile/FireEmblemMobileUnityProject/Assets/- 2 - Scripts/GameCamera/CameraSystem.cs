
using Assets.GameEngine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameCamera
{
    public class CameraSystem : MonoBehaviour, IEngineSystem
    {
        public delegate void OnMoveToFinishedEvent();

        public static OnMoveToFinishedEvent MoveToFinishedEvent;

        private List<CameraMixin> mixins;
        public CameraData CameraData;
        private void Start()
        {
            mixins = new List<CameraMixin>();
        }

        public T AddMixin<T>()
        {
            if (!typeof(CameraMixin).IsAssignableFrom(typeof(T)))
                throw new Exception("Parameter was not of type CameraMixin");
            var mixin = (CameraMixin)gameObject.AddComponent(typeof(T));
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
    }
}