using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class GestureLayerWizard : AnimatorLayerWizardBase
    {
        public override LayerKind LayerKind => LayerKind.Gesture;

        public override AnimatorLayerContextBase GetContext(EmoteWizardEnvironment env) => new GestureLayerContext(env, this);


        protected override void Reset()
        {
            base.Reset();
            var context = GetContext(CreateEnv());
            defaultAvatarMask = context.DefaultAvatarMask;
        }
    }
}