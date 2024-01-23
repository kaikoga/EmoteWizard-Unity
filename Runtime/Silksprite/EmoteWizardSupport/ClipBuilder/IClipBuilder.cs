using System;
using System.Collections.Generic;
using UnityEngine;

namespace Silksprite.EmoteWizard.ClipBuilder
{
    public interface IClipBuilder
    {
        Motion Build(string clipName, IEnumerable<AnimatedValue<float>> floatValues);
    }
    
    public struct AnimatedValue<T>
    {
        public string Path;
        public string PropertyName;
        public Type Type;

        public T Value;
    }
}