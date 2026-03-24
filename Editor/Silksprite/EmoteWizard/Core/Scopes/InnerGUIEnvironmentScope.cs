using System;
using System.Diagnostics.CodeAnalysis;
using Silksprite.EmoteWizard.Contexts;

namespace Silksprite.EmoteWizard.Scopes
{
    public class InnerGUIEnvironmentScope : IDisposable
    {
        static Lazy<EmoteWizardEnvironment>? _current;

        public static EmoteWizardEnvironment GetCurrentEnv() => _current?.Value ?? throw new InvalidOperationException();

        public static bool TryGetCurrentEnv([MaybeNullWhen(false)] out EmoteWizardEnvironment environment)
        {
            if (_current == null)
            {
                environment = null;
                return false;
            }
            environment = _current.Value;
            return true;
        }

        public InnerGUIEnvironmentScope(Func<EmoteWizardEnvironment> factory)
        {
            _current = new Lazy<EmoteWizardEnvironment>(factory);
        }

        void IDisposable.Dispose()
        {
            _current = null;
        }
    }
}