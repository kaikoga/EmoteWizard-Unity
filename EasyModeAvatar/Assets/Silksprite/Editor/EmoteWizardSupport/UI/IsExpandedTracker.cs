using System.Collections.Generic;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static class IsExpandedTracker
    {
        static readonly Dictionary<int, bool> ExpandedItems = new Dictionary<int, bool>();

        public static bool GetIsExpanded(object item)
        {
            return item != null && (!ExpandedItems.TryGetValue(item.GetHashCode(), out var value) || value);
        }

        public static bool SetIsExpanded(object item, bool value)
        {
            return ExpandedItems[item.GetHashCode()] = value;
        }
        
        public static void SetDefaultExpanded(object item, bool value)
        {
            if (item == null) return;
            var hashCode = item.GetHashCode();
            if (!ExpandedItems.ContainsKey(hashCode)) ExpandedItems[hashCode] = value;
        }

    }
}