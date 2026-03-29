using System;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public enum ParameterWriteSourceKind
    {
        NoUI,
        Button,
        Toggle,
        SubMenu,
        TwoAxisPuppet,
        FourAxisPuppet,
        RadialPuppet
    }
    
    public static class ParameterWriteSourceKindExtensions
    {
        public static ParameterWriteSourceKind ToWriteSourceKind(this ExpressionItemKind kind) => kind switch
        {
            ExpressionItemKind.Button => ParameterWriteSourceKind.Button,
            ExpressionItemKind.Toggle => ParameterWriteSourceKind.Toggle,
            ExpressionItemKind.SubMenu => ParameterWriteSourceKind.SubMenu,
            ExpressionItemKind.TwoAxisPuppet => ParameterWriteSourceKind.TwoAxisPuppet,
            ExpressionItemKind.FourAxisPuppet => ParameterWriteSourceKind.FourAxisPuppet,
            ExpressionItemKind.RadialPuppet => ParameterWriteSourceKind.RadialPuppet,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
        };
    }
}