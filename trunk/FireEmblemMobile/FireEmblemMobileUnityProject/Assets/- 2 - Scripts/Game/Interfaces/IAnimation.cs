﻿using System;

namespace Game.States
{
    public interface IAnimation
    {
        void Play(Action finished);
    }
}