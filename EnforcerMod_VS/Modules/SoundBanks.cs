using System.IO;

namespace Modules {

    internal static class SoundBanks {

        public static string SoundBankDirectory {
            get {
                return Path.Combine(Files.assemblyDir, "SoundBanks");
            }
        }

        public static void Init() {
            AKRESULT akResult = AkSoundEngine.AddBasePath(SoundBankDirectory);
            
            AkSoundEngine.LoadBank("EnforcerBank.bnk", out _);
            AkSoundEngine.LoadBank("Nemforcer.bnk", out _);
        }
    }
}