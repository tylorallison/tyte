using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace TyTe {

    // =========================================================================
    public class ZoneMgrCtx {
        // INSTANCE VARIABLES --------------------------------------------------
        public Project project;
        public UxZoneMgrCtrl ctrl;
        public Action<Zone> assignZoneFcn;
    }

    // =========================================================================
    public class ZoneMgr : MonoBehaviour {
        // INSTANCE VARIABLES --------------------------------------------------
        ZoneMgrCtx ctx;
        string zoneName = "test";
        Zone zone;
        public bool setup;
        public int dfltWidth = 32;
        public int dfltHeight = 32;

        // PROPERTIES ----------------------------------------------------------
        public bool ready { 
            get { return ctx != null && setup; }
        }

        // UNITY METHODS -------------------------------------------------------
        void Update() {
            if (!setup) Setup();
        }

        // INSTANCE METHODS ----------------------------------------------------
        public void AssignCtx(
            ZoneMgrCtx ctx
        ) {
            this.ctx = ctx;
            // assign ctrl ctx
            // FIXME
            ctx.ctrl.AssignCtx(new UxZoneMgrCtrlCtx {
                newNameFcn = (val) => {
                    if (zone != null) zone.name = val;
                    zoneName = val;
                },
                newFcn = NewZone,
                confirmFcn = ConfirmZone,
                openFcn = Open,
                selectFcn = Load,
                saveFcn = Save,
                reloadFcn = () => {
                    if (zone != null) {
                        Load(zone.name);
                    }
                }
            });
        }

        void Setup() {
            if (ctx == null) return;
            // FIXME
            Load("test");
            setup = true;
        }

        static Zone LoadJson(
            string filepath
        ) {
            using (StreamReader r = new StreamReader(filepath)) {
                string json = r.ReadToEnd();
                var zone = JsonUtility.FromJson<Zone>(json);
                return zone;
            }
        }

        static string[] GetZoneList(
            string path
        ) {
            // pull full paths to all zone files in zone folder
            var paths = Directory.GetFiles(path, "*.json");
            // pull filename from path, add to zone list
            var zones = new List<string>();
            for (var i=0; i<paths.Length; i++) {
                var name = Path.GetFileNameWithoutExtension(paths[i]);
                zones.Add(name);
            }
            return zones.ToArray();
        }

        void NewZone(
        ) {
            // instantiate new zone
            var zone = new Zone("zone", dfltWidth, dfltHeight);
            ctx.ctrl.SetupZone(zone);
        }

        void ConfirmZone(
            Zone zone
        ) {
            // add default layer to zone
            zone.AddLayer(new Layer("default", zone.width, zone.height));
            ctx.assignZoneFcn(zone);
            ctx.ctrl.AssignZone(zone);
            // assign as managed zone
            this.zone = zone;
        }

        void Open(
        ) {
            // pull zone list
            var path = Path.Combine(ctx.project.projectPath, ctx.project.zoneFolder);
            var zones = GetZoneList(path);
            // have ctrl pick a zone
            ctx.ctrl.SelectZone(zones);
        }

        void Load(
            string name
        ) {
            if (name != "") {
                var path = Path.Combine(ctx.project.projectPath, ctx.project.zoneFolder, name + ".json");
                var loadedZone = LoadJson(path);
                if (loadedZone != null) {
                    zone = loadedZone;
                    ctx.assignZoneFcn(zone);
                    ctx.ctrl.AssignZone(zone);
                }
            }
        }

        void Save() {
            var json = JsonUtility.ToJson(zone);
            var path = Path.Combine(ctx.project.projectPath, ctx.project.zoneFolder, zone.name + ".json");
            File.WriteAllText(path, json);
        }

    }

}