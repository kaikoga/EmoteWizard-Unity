using System;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class MirroredMotion
    {
        [SerializeField] public bool useMirroredSettings;
        [SerializeField] public Motion clipLeft;
        [SerializeField] public Motion clipRight;
    }
}