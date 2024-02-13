using System.IO;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.Extensions
{
    public static class TransformExtension
    {
        public static string GetRelativePathFrom(this Transform child, Transform parent)
        {
            var cursor = child;
            var path = child.gameObject.name;
            while (true)
            {
                cursor = cursor.parent;
                if (cursor == parent) break;
                if (!cursor) break;
                path = Path.Combine(cursor.gameObject.name, path);
            }
            return path;
        }
    }
}