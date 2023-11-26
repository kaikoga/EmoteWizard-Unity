using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class FxLayerWizard : AnimatorLayerWizardBase
    {
        public override LayerKind LayerKind => LayerKind.FX;


        public override AnimatorLayerContextBase GetContext(EmoteWizardEnvironment env) => new FxLayerContext(env, this);
    }
}