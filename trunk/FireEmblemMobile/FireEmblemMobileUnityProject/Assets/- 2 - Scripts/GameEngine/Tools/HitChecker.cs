﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameEngine.Tools
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
    }
}