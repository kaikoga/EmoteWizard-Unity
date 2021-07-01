using System;

namespace EmoteWizard.DataObjects.Internal
{
    [Flags]
    public enum ParameterValueTypeFlags
    {
        Bool = 1,
        Int = 2,
        Float = 4
    }
}