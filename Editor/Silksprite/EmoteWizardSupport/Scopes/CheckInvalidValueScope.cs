using System;

namespace Silksprite.EmoteWizardSupport.Scopes
{
    public class CheckInvalidValueScope : IDisposable
    {
        readonly int _lastInvalidCount = InvalidValueScope.InvalidCount;

        public bool IsInvalid => _lastInvalidCount != InvalidValueScope.InvalidCount;

        public void Dispose()
        {
        }
    }
}