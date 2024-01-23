using System;
using System.Collections.Generic;
using System.Linq;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public enum ExpressionItemKind
    {
        Button,
        Toggle,
        SubMenu,
        TwoAxisPuppet,
        FourAxisPuppet,
        RadialPuppet
    }
    
    public static class ExpressionItemKindExtensions
    {
        public static IEnumerable<T> Take<T>(this IEnumerable<T> self, ExpressionItemKind kind) => self.Take(kind.SubItemCount());

        static int SubItemCount(this ExpressionItemKind self)
        {
            switch (self)
            {
                case ExpressionItemKind.Button:
                case ExpressionItemKind.Toggle:
                case ExpressionItemKind.SubMenu:
                    return 0;
                case ExpressionItemKind.TwoAxisPuppet:
                    return 2;
                case ExpressionItemKind.FourAxisPuppet:
                    return 4;
                case ExpressionItemKind.RadialPuppet:
                    return 1;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }
    }
}