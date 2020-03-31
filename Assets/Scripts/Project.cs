using System;
using System.IO;
using UnityEngine;

namespace TyTe {

    [Serializable]
    public class Project {
        public const int maxTileWidth = 128;
        public const int maxTileHeight = 128;
        public string projectPath;
        public string zoneFolder = "src/zones";
        public string spriteFolder = "src/img";
        public string spriteJson = "src/img/sprites.json";
        public int tileWidth = 32;
        public int tileHeight = 32;

        public static Project FromJson(
            string json
        ) {
            return JsonUtility.FromJson<Project>(json);
        }
        public static string AsJson(
            Project ctx
        ) {
            return JsonUtility.ToJson(ctx);
        }

        public bool IsValid() {
            if (!Directory.Exists(projectPath)) return false;
            if (!Directory.Exists(Path.Combine(projectPath, zoneFolder))) return false;
            if (!Directory.Exists(Path.Combine(projectPath, spriteFolder))) return false;
            if (!File.Exists(Path.Combine(projectPath, spriteJson))) return false;
            if (tileWidth <= 0 || tileWidth>maxTileWidth) return false;
            if (tileHeight <= 0 || tileHeight>maxTileHeight) return false;
            return true;
        }

    }

}
