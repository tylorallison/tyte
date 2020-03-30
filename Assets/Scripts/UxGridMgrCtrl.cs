using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TyTe {

    // =========================================================================
    public class UxGridMgrCtrlCtx {
        public UxGridCtrlCtx gridCtrlCtx;
    }

    // =========================================================================
    public class UxGridMgrCtrl : MonoBehaviour {

        // INSTANCE VARIABLES --------------------------------------------------
        public RectTransform contentTransform;
        public GameObject gridPrefab;

        Dictionary<Layer,UxGridCtrl> gridMap = new Dictionary<Layer, UxGridCtrl>();
        UxGridMgrCtrlCtx ctx;

        public void AssignCtx(
            UxGridMgrCtrlCtx ctx
        ) {
            this.ctx = ctx;
        }

        public void Add(
            Layer layer
        ) {
            // instantiate new grid, associate layer
            var gridGO = UnityEngine.Object.Instantiate(gridPrefab, contentTransform);
            var gridCtrl = gridGO.GetComponent<UxGridCtrl>();
            // assign ctx/layer to grid ctrl
            gridCtrl.AssignCtx(ctx.gridCtrlCtx);
            gridCtrl.AssignLayer(layer);
            // store controller to gridmap
            gridMap[layer] = gridCtrl;
        }

        public void Raise(
            Layer layer
        ) {
            var ctrl = gridMap[layer];
            if (ctrl == null) return;
            var idx = ctrl.transform.GetSiblingIndex();
            if (idx > 0) {
                ctrl.transform.SetSiblingIndex(idx-1);
            }
        }

        public void Lower(
            Layer layer
        ) {
            var ctrl = gridMap[layer];
            if (ctrl == null || ctrl.transform.parent == null) return;
            var idx = ctrl.transform.GetSiblingIndex();
            if (idx < ctrl.transform.parent.childCount-1) {
                ctrl.transform.SetSiblingIndex(idx+1);
            }
        }

        public void Remove(
            Layer layer
        ) {
            var ctrl = gridMap[layer];
            if (ctrl == null) return;
            // remove from layers
            gridMap.Remove(layer);
            // destroy grid
            Destroy(ctrl.gameObject);
        }

        public void Select(
            Layer layer,
            bool selected
        ) {
            var ctrl = gridMap[layer];
            if (ctrl == null) return;
            ctrl.Select(selected);
        }

        public void Visible(
            Layer layer,
            bool selected
        ) {
            var ctrl = gridMap[layer];
            if (ctrl == null) return;
            ctrl.Visible(selected);
        }

        public void Clear() {
            var keys = new List<Layer>(gridMap.Keys);
            foreach (var key in keys) {
                Remove(key);
            }
        }

    }

}