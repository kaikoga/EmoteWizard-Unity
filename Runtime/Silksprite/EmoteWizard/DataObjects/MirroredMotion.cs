using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.EmoteWizardTools;

#if EW_VRCSDK3_AVATARS
using VRC.SDK3.Avatars.ScriptableObjects;
#endif

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