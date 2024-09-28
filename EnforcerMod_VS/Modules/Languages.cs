using BepInEx;
using System;
using System.IO;
using System.Linq;

namespace Modules {
    internal static class Languages {
        
        public static string TokensOutput = "";

        public static bool printingEnabled => false;
        
        internal static string languageRoot => System.IO.Path.Combine(Files.assemblyDir, "Language");
        
        public static void Init() {
            HookRegisterLanguageTokens();
        }

        private static void HookRegisterLanguageTokens() {
            On.RoR2.Language.SetFolders += SetFolders;
        }

        //Credits to Moffein for this credit
        //Credits to Anreol for this code
        private static void SetFolders(On.RoR2.Language.orig_SetFolders orig, RoR2.Language self, System.Collections.Generic.IEnumerable<string> newFolders) {
            if (System.IO.Directory.Exists(Languages.languageRoot)) {
                var dirs = System.IO.Directory.EnumerateDirectories(System.IO.Path.Combine(Languages.languageRoot), self.name);
                orig(self, newFolders.Union(dirs));
                return;
            }
            orig(self, newFolders);
        }

        public static void Add(string token, string text) {

            R2API.LanguageAPI.Add(token, text, "en");

            if (!printingEnabled) return;

            //add a token formatted to language file
            TokensOutput += $"\n    \"{token}\" : \"{text.Replace(Environment.NewLine, "\\n").Replace("\n", "\\n")}\",";
        }

        public static void PrintOutput(string fileName = "") {
            if (!printingEnabled) return;

            //wrap all tokens in a properly formatted language file
            string strings = $"{{\n    strings:\n    {{{TokensOutput}\n    }}\n}}";

            //spit out language dump in console for copy paste if you want
            Log.Warning($"{fileName}: \n{strings}");

            //write a language file txt next to your mod. must have a folder called Language next to your mod dll.
            if (!string.IsNullOrEmpty(fileName)) {
                string path = Path.Combine(Files.assemblyDir, "Language", "english", fileName);
                File.WriteAllText(path, strings);
            }

            //empty the output each time this is printed, so you can print multiple language files
            TokensOutput = "";
        }
    }
}