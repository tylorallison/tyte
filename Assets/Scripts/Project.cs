using System;
using System.IO;
using UnityEngine;

namespace TyTe {

    [Serializable]
    public class Project {
        [Tooltip("absolute path for project root")]
        public string projectPath;
        [Tooltip("relative path to zone level data folder")]
        public string zoneFolder = "src/zones";
        [Tooltip("relative path to sprite folder")]
        public string spriteFolder = "src/img";
        [Tooltip("sprite record filename, maps sprite ids to files and provides info on sprites")]
        public string spriteJson = "src/img/sprites.json";

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
            return true;
        }

    }

}
