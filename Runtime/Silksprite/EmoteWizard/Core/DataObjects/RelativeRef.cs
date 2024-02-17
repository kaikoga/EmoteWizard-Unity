using System;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    public abstract class RelativeRef<T>
    where T : Component
    {
        [SerializeField] public T target;
        [SerializeField] public string relativePath;

        public bool RefreshRelativePath(Transform root)
        {
            if (target == null) return false;
            var newRelativePath = RuntimeUtil.RelativePath(root, target.transform);
            if (newRelativePath == relativePath) return false;
            relativePath = newRelativePath;
            return true;
        }

        public T GetOrResolveTarget(Transform root)
        {
            if (target) return target;
            return target = ResolveTarget(root, relativePath);
        }

        public static T ResolveTarget(Transform root, string relativePath) => RuntimeUtil.FromRelativePath(root, relativePath)?.GetComponent<T>();
    }
    
    [Serializable] public class RelativeTransformRef : RelativeRef<Transform> { }
    [Serializable] public class RelativeSkinnedMeshRendererRef : RelativeRef<SkinnedMeshRenderer> { }
}