using System.Collections.Generic;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static class IsExpandedTracker
    {
        static readonly Dictionary<int, bool> CollapsedItems = new Dictionary<int, bool>();

        public static bool GetIsExpanded(object item)
        {
            return !CollapsedItems.TryGetValue(item.GetHashCode(), out _);
        }

        public static bool SetIsExpanded(object item, bool value)
        {
            if (!value)
            {
                CollapsedItems[item.GetHashCode()] = true;
            }
            else
            {
                CollapsedItems.Remove(item.GetHashCode());
            }
            return value;
        }
    }
}