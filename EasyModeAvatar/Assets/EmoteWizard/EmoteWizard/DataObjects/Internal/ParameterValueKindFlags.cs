using System;

namespace EmoteWizard.DataObjects.Internal
{
    [Flags]
    public enum ParameterValueKindFlags
    {
        Bool = 1,
        Int = 2,
        Float = 4
    }
}