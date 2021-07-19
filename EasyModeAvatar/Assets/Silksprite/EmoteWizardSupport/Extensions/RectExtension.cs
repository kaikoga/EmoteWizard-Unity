using UnityEngine;

namespace Silksprite.EmoteWizardSupport.Extensions
{
    public static class RectExtension
    {
        public static Rect Inset(this Rect rect, float left, float top, float right, float bottom)
        {
            return Rect.MinMaxRect(rect.xMin + left, rect.yMin + top, rect.xMax - right, rect.yMax - bottom);
        }

        static float ParseY(Rect position, float y)
        {
            return y >= 0 ? position.y + y : position.yMax - y;
        }

        static float ParseHeight(Rect position, float height)
        {
            return height >= 0 ? height : position.height + height;
        }

        public static Rect SliceH(this Rect position, float x, float width)
        {
            return new Rect(position.x + position.width * x, position.y, position.width * width, position.height);
        }

        public static Rect SliceV(this Rect position, float y, float height)
        {
            return new Rect(position.x, ParseY(position, y), position.width, ParseHeight(position, height));
        }

        public static Rect Slice(this Rect position, float x, float width, float y, float height)
        {
            return new Rect(position.x + position.width * x, ParseY(position, y), position.width * width, ParseHeight(position, height));
        }


    }
}