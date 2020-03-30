using System;
using UnityEngine;
using UnityEngine.UI;

namespace TyTe {

    // =========================================================================
    public class UxLayerCtrlCtx {
        public Action<Layer> removeFcn;
        public Action<Layer> raiseFcn;
        public Action<Layer> lowerFcn;
        public Action<Layer,bool> selectFcn;
        public Action<Layer,bool> visibleFcn;
    }

    // =========================================================================
    public class UxLayerCtrl : MonoBehaviour {

        // INSTANCE VARIABLES --------------------------------------------------
        public GameObject selectedPanel;
        public Toggle selectToggle;
        public Toggle visibleToggle;
        public Button upButton;
        public Button downButton;
        public Button deleteButton;
        public InputField nameField;
        UxLayerCtrlCtx ctx;

        Layer layer;
        bool listen = true;

        public void Awake() {
            Select(false);
            // setup wiring
            // -- select
            selectToggle.onValueChanged.AddListener((val) => { if (listen && ctx != null) ctx.selectFcn(this.layer, val); });
            // -- visible
            visibleToggle.onValueChanged.AddListener((val) => { if (listen && ctx != null) ctx.visibleFcn(this.layer, val); });
            // -- up
            upButton.onClick.AddListener(() => { if (listen && ctx != null) ctx.raiseFcn(this.layer); });
            // -- down
            downButton.onClick.AddListener(() => { if (listen && ctx != null) ctx.lowerFcn(this.layer); });
            // -- delete
            deleteButton.onClick.AddListener(() => { if (listen && ctx != null) ctx.removeFcn(this.layer); });
            listen = true;
        }

        public void AssignCtx(
            UxLayerCtrlCtx ctx
        ) {
            this.ctx = ctx;
        }

        public void AssignLayer(
            Layer layer
        ) {
            this.layer = layer;
            this.nameField.text = layer.name;
            // setup handlers
            // -- name
            nameField.onValueChanged.AddListener((val) => {
                layer.name = nameField.text;
            });
        }

        public void Select(
            bool selected
        ) {
            if (selected != selectToggle.isOn) {
                listen = false;
                selectToggle.isOn = selected;
                listen = true;
            }
            if (selectedPanel != null) {
                selectedPanel.SetActive(selected);
            }
        }

    }

}