using System.Collections.Generic;
using System.Linq;
using GameEngine.Tools;
using UnityEngine;

namespace Game.Utility
{
    public class HitChecker : IHitChecker
    {
        private List<string> excludeColliderTags;
        
        public HitChecker(params string[] excludeColliderTags)
        {
           this.excludeColliderTags = excludeColliderTags.ToList();
        }
        public bool CheckHit(Ray ray)
        {
            var hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            if (hit.collider != null)
            {
                if (!excludeColliderTags.Contains(hit.collider.gameObject.tag))
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
            return false;
        }

        public bool HasTagExcluded(string tag)
        {
            return excludeColliderTags.Contains(tag);
        }
    }
}