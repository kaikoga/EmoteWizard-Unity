using EmoteWizard.Base;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace EmoteWizard
{
    [DisallowMultipleComponent]
    public class ExpressionWizard : EmoteWizardBase
    {
        [SerializeField] public VRCExpressionsMenu outputAsset;
    }
}