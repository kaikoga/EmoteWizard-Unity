using System.IO;
using Silksprite.EmoteWizard.Contexts;

namespace Silksprite.EmoteWizard.Utils
{
    public readonly struct GeneratedPath
    {
        readonly string _relativePath;

        public GeneratedPath(string relativePath) => _relativePath = relativePath;

        public string Resolve(EmoteWizardEnvironment environment)
        {
            if (!environment.Root)
            {
                return Path.GetFileNameWithoutExtension(_relativePath.Replace("@@@Generated@@@", ""));
            }

            var relativePath = _relativePath.Replace("@@@Generated@@@", environment.Root.generatedAssetPrefix);
            return Path.Combine(environment.Root.generatedAssetRoot, relativePath);
        }
    }
}