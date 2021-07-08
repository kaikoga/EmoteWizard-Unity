using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.PropertyDrawerUITools;

namespace Silksprite.EmoteWizardSupport.Extensions
{
    public static class RectExtension
    {
        public static Rect Inset(this Rect rect, float left, float top, float right, float bottom)
        {
            return Rect.MinMaxRect(rect.xMin + left, rect.yMin + top, rect.xMax - right, rect.yMax - bottom);
        }

        static float ParseLineY(Rect position, int lineY)
        {
            return lineY >= 0 ? position.y + LineTop(lineY) : position.yMax - LineHeight(-lineY);
        }

        static float ParseLineHeight(Rect position, int lineHeight)
        {
            return lineHeight >= 0 ? LineHeight(lineHeight) : position.height - LineTop(-lineHeight);
        }

        public static Rect UISliceH(this Rect position, float x, float width)
        {
            return new Rect(position.x + position.width * x, position.y, position.width * width, position.height);
        }

        public static Rect UISliceV(this Rect position, int lineY, int lineHeight = 1)
        {
            return new Rect(position.x, ParseLineY(position, lineY), position.width, ParseLineHeight(position, lineHeight));
        }

        public static Rect UISlice(this Rect position, float x, float width, int lineY, int lineHeight = 1)
        {
            return new Rect(position.x + position.width * x, ParseLineY(position, lineY), position.width * width, ParseLineHeight(position, lineHeight));
        }

        public static Rect InsideBox(this Rect rect)
        {
            var border = GUI.skin.box.border;
            var padding = GUI.skin.box.padding;
            return new Rect(rect.x + border.left + padding.left,
                rect.y + border.top + padding.top,
                rect.width - border.horizontal - padding.horizontal,
                rect.height - border.vertical - padding.vertical);
        }
    }
}