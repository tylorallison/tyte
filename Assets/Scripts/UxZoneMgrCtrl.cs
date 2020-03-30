using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TyTe {

    // =========================================================================
    public class UxZoneMgrCtrlCtx {
        // INSTANCE VARIABLES --------------------------------------------------
        public Action newFcn;
        public Action saveFcn;
        public Action reloadFcn;
        public Action openFcn;
        public Action<string> newNameFcn;
        public Action<Zone> confirmFcn;
        public Action<string> selectFcn;
    }

    // =========================================================================
    public class UxZoneMgrCtrl : MonoBehaviour {

        public Button newButton;
        public Button saveButton;
        public Button openButton;
        public Button reloadButton;
        public InputField nameField;
        public UxPanel master;
        public GameObject zoneSelectorPrefab;
        public GameObject zoneSetupPrefab;

        UxZoneMgrCtrlCtx ctx;
        Zone zone;
        bool listen;

        public void AssignCtx(
            UxZoneMgrCtrlCtx ctx
        ) {
            // assign ctx
            this.ctx = ctx;
            // wire listeners
            // -- name
            nameField.onValueChanged.AddListener((val) => { if (listen) ctx.newNameFcn(nameField.text); });
            // -- newButton
            newButton.onClick.AddListener( () => { if (listen) ctx.newFcn(); });
            // -- saveButton
            saveButton.onClick.AddListener( () => { if (listen) ctx.saveFcn(); });
            // -- openButton
            openButton.onClick.AddListener( () => { if (listen) ctx.openFcn(); });
            // -- reloadButton
            reloadButton.onClick.AddListener( () => { if (listen) ctx.reloadFcn(); });
            this.listen = true;
        }

        public void SetupZone(
            Zone zone
        ) {
            StartCoroutine(PopupZoneSetup(zone));
        }

        IEnumerator PopupZoneSetup(
            Zone zone
        ) {
            var selectedZone = "";
            bool zoneOK = false;
            var panelGo = Instantiate(zoneSetupPrefab, GetComponentInParent<Canvas>().gameObject.transform);
            // setup ctrl
            var ctrl = panelGo.GetComponent<UxNewZoneCtrl>();
            ctrl.onDoneEvent.AddListener(master.Enable);
            // assign panel ctx
            ctrl.AssignCtx( new UxNewZoneCtrlCtx {
                chgNameFcn = (val) => { zone.name = val; },
                chgWidthFcn = (val) => { zone.width = val; },
                chgHeightFcn = (val) => { zone.height = val; },
                okFcn = () => { zoneOK = true; }
            });
            ctrl.AssignZone( zone );
            // now disable the master panel
            master.Disable();
            // wait for panel to be closed
            while( !master.active ) {
                yield return null;      // wait for next update
            }
            if (zoneOK) {
                ctx.confirmFcn(zone);
            }
        }

        public void AssignZone(
            Zone zone
        ) {
            Debug.Log("zonemgrctrl assign zone: " + zone);
            this.zone = zone;
            // assign values
            listen = false;
            this.nameField.text = zone.name;
            listen = true;
        }

        public void SelectZone(
            string[] zones
        ) {
            StartCoroutine(PopupSelectZone(zones));
        }

        IEnumerator PopupSelectZone(
            string[] zones
        ) {
            var selectedZone = "";
            var panelGo = Instantiate(zoneSelectorPrefab, GetComponentInParent<Canvas>().gameObject.transform);
            // setup a callback, so when the sub menu/panel is done, we display the current panel again
            var ctrl = panelGo.GetComponent<UxZoneSelectCtrl>();
            ctrl.onDoneEvent.AddListener(master.Enable);
            // assign panel ctx
            ctrl.AssignCtx( new UxZoneSelectCtrlCtx {
                selectZoneFcn = (val) => {
                    selectedZone = val;
                    Debug.Log("selected zone is " + selectedZone);
                }
            });
            ctrl.AssignZones( zones );
            // now disable the master panel
            master.Disable();
            // wait for panel to be closed
            while( !master.active ) {
                yield return null;      // wait for next update
            }
            // select zone
            if (selectedZone != "") {
                ctx.selectFcn(selectedZone);
            }
        }

    }

}