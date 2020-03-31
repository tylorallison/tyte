using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace TyTe {

    // =========================================================================
    [Serializable]
    public class RecordsRoot {
        public SpriteRecord[] records;
    }

    [Serializable]
    public class SpriteRecord {
        // INSTANCE VARIABLES --------------------------------------------------
        public int id;
        public string path;
        public int width;
        public int height;
        public Sprite sprite;
    }

    // =========================================================================
    public class SpriteMgrCtx {
        public Project project;
        public UxSpriteMgrCtrl ctrl;
    }

    // =========================================================================
    public class SpriteMgr : MonoBehaviour {
        // INSTANCE VARIABLES --------------------------------------------------
        public bool setup;
        string path;

        SpriteMgrCtx ctx;
        Dictionary<int,SpriteRecord> spriteMap;
        SpriteRecord selectedSprite = null;

        // PROPERTIES ----------------------------------------------------------
        public bool ready { 
            get { return setup; }
        }
        public SpriteRecord selected { 
            get { 
                return selectedSprite; 
            }
        }

        // STATIC METHODS ------------------------------------------------------
        static SpriteRecord[] LoadJson(
            string filepath
        ) {
            using (StreamReader r = new StreamReader(filepath)) {
                string json = r.ReadToEnd();
                var root = JsonUtility.FromJson<RecordsRoot>(json);
                return root.records;
            }
        }

        public static Sprite LoadSprite(
            string filePath
        ) {
            // load texture
            Texture2D tex = null;
            byte[] fileData;
            if (File.Exists(filePath))     {
                fileData = File.ReadAllBytes(filePath);
                tex = new Texture2D(2, 2);
                tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
                // instantiate sprite
                return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            }
            return null;
        }

        // INSTANCE METHODS ----------------------------------------------------
        // assign context
        public void AssignCtx(
            SpriteMgrCtx ctx
        ) {
            // save mgr context
            this.ctx = ctx;
            // assign ctrl context
            ctx.ctrl.Assign( new UxSpriteMgrCtrlCtx {
                project = ctx.project,
                selectSpriteFcn = (record) => { 
                    selectedSprite = record; 
                },
                reloadFcn = Load,
            });
        }

        // setup after assignment of context
        void Setup() {
            if (ctx == null) return;
            path = Path.Combine(ctx.project.projectPath, ctx.project.spriteFolder);
            // setup sprite map
            spriteMap = new Dictionary<int, SpriteRecord>();
            // load sprites
            Load();
            // mark setup as complete
            setup = true;
        }

        void Clear() {
            // clear controller
            if (ctx != null) {
                ctx.ctrl.Clear();
            }
            // clear current sprite map and selection
            if (spriteMap != null) {
                spriteMap.Clear();
            }
            selectedSprite = null;
        }

        void Load() {
            // clear current state
            Clear();
            // load sprite record json
            var records = LoadJson( Path.Combine(ctx.project.projectPath, ctx.project.spriteJson) );
            // load sprites
            for (var i=0; i<records.Length; i++) {
                // resolve path for sprite
                records[i].path = Path.Combine(path, records[i].path + ".png");
                // load the sprite from file
                records[i].sprite = LoadSprite(records[i].path);
                if (records[i].sprite == null) {
                    Debug.Log("failed to load sprite: " + records[i].path);
                    continue;
                }
                // save to sprite map
                spriteMap[records[i].id] = records[i];
                // tell controller to add sprite
                ctx.ctrl.AddSprite(records[i]);
            }
            // first sprite becomes selected sprite
            if (records.Length > 0) {
                Select(records[0].id);
            }
        }

        void Update() {
            if (!setup) Setup();
        }

        public void Walk(
            Action<SpriteRecord> func
        ) {
            foreach (var record in spriteMap.Values) {
                func(record);
            }
        }

        public SpriteRecord Lookup(
            int id
        ) {
            SpriteRecord record = null;
            spriteMap.TryGetValue(id, out record);
            return record;
        }

        public void Select(
            int id
        ) {
            SpriteRecord record = null;
            spriteMap.TryGetValue(id, out record);
            Debug.Log("trying to select: " + id + " w/ record: " + record);
            if (record != null) {
                selectedSprite = record;
                ctx.ctrl.SelectSprite(record);
            }
        }

    }

}