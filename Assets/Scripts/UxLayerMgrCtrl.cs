using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TyTe {
    // =========================================================================
    public class UxLayerMgrCtrlCtx {
        // INSTANCE VARIABLES --------------------------------------------------
        public Action addFcn;
        public Action<Layer> removeFcn;
        public Action<Layer> raiseFcn;
        public Action<Layer> lowerFcn;
        public Action<Layer,bool> selectFcn;
        public Action<Layer,bool> visibleFcn;
    }

    // =========================================================================
    public class UxLayerMgrCtrl : MonoBehaviour {

        // INSTANCE VARIABLES --------------------------------------------------
        public RectTransform contentTransform;
        public GameObject layerPrefab;
        public Button addButton;

        Dictionary<Layer, UxLayerCtrl> layerMap = new Dictionary<Layer, UxLayerCtrl>();
        UxLayerMgrCtrlCtx ctx;

        public void Awake() {
            if (addButton != null) {
                addButton.onClick.AddListener(() => {
                    if (ctx != null) ctx.addFcn();
                });
            }
        }

        public void AssignCtx(
            UxLayerMgrCtrlCtx ctx
        ) {
            this.ctx = ctx;
        }

        public void Add(
            Layer layer
        ) {
            // instantite new layer
            var layerGO = UnityEngine.Object.Instantiate(layerPrefab, contentTransform);
            var ctrl = layerGO.GetComponent<UxLayerCtrl>();
            // assign ctx
            ctrl.AssignCtx( new UxLayerCtrlCtx {
                removeFcn = (l) => {
                    if (ctx != null) ctx.removeFcn(l);
                    Remove(l);
                },
                raiseFcn = (l) => {
                    if (ctx != null) ctx.raiseFcn(l);
                    Raise(layer);
                },
                lowerFcn = (l) => {
                    if (ctx != null) ctx.lowerFcn(l);
                    Lower(layer);
                },
                selectFcn = (l, val) => {
                    if (ctx != null) ctx.selectFcn(l, val);
                    Select(layer, val);

                },
                visibleFcn = (l, val) => {
                    if (ctx != null) ctx.visibleFcn(l, val);
                },
            });
            // assign layer
            ctrl.AssignLayer(layer);
            layerMap[layer] = ctrl;
        }

        public void Raise(
            Layer layer
        ) {
            var ctrl = layerMap[layer];
            if (ctrl == null) return;
            var idx = ctrl.transform.GetSiblingIndex();
            if (idx > 0) {
                ctrl.transform.SetSiblingIndex(idx-1);
            }
        }

        public void Lower(
            Layer layer
        ) {
            var ctrl = layerMap[layer];
            if (ctrl == null || ctrl.transform.parent == null) return;
            var idx = ctrl.transform.GetSiblingIndex();
            if (idx < ctrl.transform.parent.childCount-1) {
                ctrl.transform.SetSiblingIndex(idx+1);
            }
        }

        public void Remove(
            Layer layer
        ) {
            var ctrl = layerMap[layer];
            if (ctrl == null) return;
            // remove from layers
            layerMap.Remove(layer);
            // destroy layer
            Destroy(ctrl.gameObject);
        }

        public void Select(
            Layer layer,
            bool selected
        ) {
            var ctrl = layerMap[layer];
            if (ctrl == null) return;
            ctrl.Select(selected);
        }

        public void Clear() {
            var keys = new List<Layer>(layerMap.Keys);
            foreach (var key in keys) {
                Remove(key);
            }
        }

    }

}