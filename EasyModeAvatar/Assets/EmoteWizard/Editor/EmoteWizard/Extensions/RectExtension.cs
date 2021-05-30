using UnityEngine;
using static EmoteWizard.Extensions.PropertyDrawerUITools;

namespace EmoteWizard.Extensions
{
    public static class RectExtension
    {
        static float ParseLineHeight(Rect position, int lineHeight)
        {
            return lineHeight >= 0 ? LineHeight(lineHeight) : position.height - LineHeight(-lineHeight);
        }

        public static Rect SliceH(this Rect position, float x, float width)
        {
            return new Rect(position.x + position.width * x, position.y, position.width * width, position.height);
        }

        public static Rect SliceV(this Rect position, int lineY, int lineHeight = 1)
        {
            return new Rect(position.x, position.y + LineTop(lineY), position.width, ParseLineHeight(position, lineHeight));
        }

        public static Rect Slice(this Rect position, float x, float width, int lineY, int lineHeight = 1)
        {
            return new Rect(position.x + position.width * x, position.y + LineTop(lineY), position.width * width, ParseLineHeight(position, lineHeight));
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