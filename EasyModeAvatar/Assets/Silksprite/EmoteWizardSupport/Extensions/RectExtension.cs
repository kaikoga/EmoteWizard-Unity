using UnityEngine;

namespace Silksprite.EmoteWizardSupport.Extensions
{
    public static class RectExtension
    {
        public static Rect Inset(this Rect rect, float left, float top, float right, float bottom)
        {
            return Rect.MinMaxRect(rect.xMin + left, rect.yMin + top, rect.xMax - right, rect.yMax - bottom);
        }
    }
}