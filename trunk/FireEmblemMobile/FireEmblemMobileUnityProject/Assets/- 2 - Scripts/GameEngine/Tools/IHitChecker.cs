﻿using UnityEngine;

namespace GameEngine.Tools
{
    public interface IHitChecker
    {
        bool CheckHit(Ray ray);
        bool HasTagExcluded(string tag);
    }
}