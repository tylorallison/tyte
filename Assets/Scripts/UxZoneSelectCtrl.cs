using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TyTe {

    public class UxZoneSelectCtrlCtx {
        public Action<string> selectZoneFcn;
    }

    public class UxZoneSelectCtrl : UxPanel {
        public GameObject zoneSelectorPrefab;
        public ToggleGroup toggleGroup;
        public Button okButton;
        public Button cancelButton;
        public RectTransform contentTransform;

        UxZoneSelectCtrlCtx ctx;
        List<GameObject> zoneGOs = new List<GameObject>();
        string[] zones;
        string selectedZone;

        public void Awake() {
            // wire listeners
            okButton.onClick.AddListener(() => {
                if (ctx != null && selectedZone != "") {
                    ctx.selectZoneFcn(selectedZone);
                }
                Destroy(gameObject);
            });
            cancelButton.onClick.AddListener(() => {
                Destroy(gameObject);
            });
        }

        public void AssignCtx(
            UxZoneSelectCtrlCtx ctx
        ) {
            this.ctx = ctx;
        }

        public void AssignZones(
            string[] zones
        ) {
            Clear();
            this.zones = zones;
            // iterate through zones and add zone selector
            for (var i=0; i<zones.Length; i++) {
                var thisZone = zones[i];
                var zoneGO = UnityEngine.Object.Instantiate(zoneSelectorPrefab, contentTransform);
                // wire zone handler
                var zoneToggle = zoneGO.GetComponent<Toggle>();
                zoneToggle.group = toggleGroup;
                zoneToggle.onValueChanged.AddListener((val) => { if (val) selectedZone=thisZone; });
                var text = zoneGO.GetComponentInChildren<Text>();
                if (text != null) {
                    text.text = thisZone;
                }
            }
        }

        public void Clear() {
            for (var i=0; i<zoneGOs.Count; i++) {
                Destroy(zoneGOs[i]);
            }
            zoneGOs.Clear();
        }
    }

}