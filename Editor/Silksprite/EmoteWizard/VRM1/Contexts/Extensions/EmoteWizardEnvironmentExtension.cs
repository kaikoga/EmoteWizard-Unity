#if EW_VRM1

using System;
using Silksprite.AdLib.Utils.VRM1;
using Silksprite.EmoteWizard.Contexts.Ephemeral;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizardSupport.Undoable;
using UniVRM10;

namespace Silksprite.EmoteWizard.Contexts.Extensions
{
    public static class EmoteWizardEnvironmentExtension
    {
        public static void BuildVrm1Avatar(this EmoteWizardEnvironment environment, IUndoable undoable, bool manualBuild)
        {
            if (manualBuild) throw new InvalidOperationException("manual build is not supported");

            var vrm10Instance = environment.AvatarRoot.GetComponent<Vrm10Instance>();
            if (!vrm10Instance) return;

            var vrm10Object = vrm10Instance.Vrm;
            if (!vrm10Object) return;

            vrm10Object = new CustomCloneVRM10Object().Clone(vrm10Object).mainAsset;

            var expression = vrm10Object.Expression;
            
            foreach (var genericEmoteItem in environment.GetContext<GenericEmoteItemContext>().GenericEmoteItems(Platform.VRM1))
            {
                genericEmoteItem.Trigger.TryGetVrm1ExpressionPreset(out var key);
                var clip = genericEmoteItem.ToVRM10Expression(environment, out var expressionPreset);
                expression.AddClip(expressionPreset, clip);
            }
        }
    }
}

#endif