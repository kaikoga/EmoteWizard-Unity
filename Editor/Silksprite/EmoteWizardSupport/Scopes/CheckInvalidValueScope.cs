using System;

namespace Silksprite.EmoteWizardSupport.Scopes
{
    public class CheckInvalidValueScope : IDisposable
    {
        readonly int _lastInvalidCount = 0;

        public bool IsInvalid => _lastInvalidCount != InvalidValueScope.InvalidCount;

        public CheckInvalidValueScope()
        {
            _lastInvalidCount = InvalidValueScope.InvalidCount;
        }

        public void Dispose()
        {
        }
    }
}