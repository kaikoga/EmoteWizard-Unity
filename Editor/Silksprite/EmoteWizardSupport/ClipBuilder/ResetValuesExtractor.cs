using System.Collections.Generic;
using Silksprite.EmoteWizardSupport.Logger;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

namespace Silksprite.EmoteWizardSupport.ClipBuilder
{
    public static class ResetValuesExtractor
    {
        public static BoundValues ExtractFromAvatarRoot(CurveBindings bindings, GameObject avatarRoot)
        {
            void WarnBindingNotFound(EditorCurveBinding binding)
            {
                ErrorReportWrapper.LogWarningFormat(Loc("Warn::ResetClip::MissingProperty."), avatarRoot,
                    new Substitution
                    {
                        ["avatarRootName"] = avatarRoot.name,
                        ["path"] = binding.path,
                        ["type"] = $"{binding.type}",
                        ["propertyName"] = binding.propertyName    
                    }
                );
            }

            var boundFloats = new List<BoundValues.BoundFloatValue>();
            foreach (var binding in bindings.Floats)
            {
                if (AnimationUtility.GetFloatValue(avatarRoot, binding, out var value))
                {
                    boundFloats.Add(new BoundValues.BoundFloatValue(binding, value));
                }
                else
                {
                    WarnBindingNotFound(binding);
                }
            }

            var boundObjects = new List<BoundValues.BoundObjectValue>();
            foreach (var objectBinding in bindings.Objects)
            {
                if (AnimationUtility.GetObjectReferenceValue(avatarRoot, objectBinding, out var value))
                {
                    boundObjects.Add(new BoundValues.BoundObjectValue(objectBinding, value));
                }
                else
                {
                    WarnBindingNotFound(objectBinding);
                }
            }

            return new BoundValues(boundFloats, boundObjects);
        }
    }
}