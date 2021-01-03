using System;

namespace Utility
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PreJitAttribute : Attribute
    {
        public PreJitAttribute()
        {

        }
    }
}