using System;
using UnityEditor;

namespace Silksprite.EmoteWizardSupport.Utils
{
    public static class EditorUtil
    {
        public static T ToEphemeralClone<T>(T asset, Func<T, T> customClone) where T : UnityEngine.Object
        {
            if (!asset || !EditorUtility.IsPersistent(asset))
            {
                return asset;
            } 
            var clone = customClone(asset);
#if EW_ABLET_SUPPORT
            Ablet.ErrorReporting.ObjectChain.Register(asset, clone);
#elif EW_NDMF_SUPPORT
            nadena.dev.ndmf.ObjectRegistry.RegisterReplacedObject(asset, clone);
#endif
            return clone;
        }
    }
}