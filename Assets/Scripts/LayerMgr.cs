using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace TyTe {

    // =========================================================================
    public class LayerMgrCtx {
        public UxLayerMgrCtrl layerCtrl;
        public UxGridMgrCtrl gridCtrl;
    }

    // =========================================================================
    public class LayerMgr : MonoBehaviour {
        // INSTANCE VARIABLES --------------------------------------------------
        LayerMgrCtx ctx;
        Layer selectedLayer = null;
        Zone zone;

        // PROPERTIES ----------------------------------------------------------
        public bool ready { get { return true; }}

        public void AssignCtx(
            LayerMgrCtx ctx
        ) {
            this.ctx = ctx;
            // assign ctrl ctx
            ctx.layerCtrl.AssignCtx( new UxLayerMgrCtrlCtx {
                addFcn = Add,
                removeFcn = Remove,
                raiseFcn = Raise,
                lowerFcn = Lower,
                selectFcn = Select,
                visibleFcn = Visible,
            });
        }

        public void AssignZone(
            Zone zone
        ) {
            // clear current state
            Clear();
            this.zone = zone;
            // add zone layers
            for (var i=0; i<zone.layers.Length; i++) {
                ctx.layerCtrl.Add(zone.layers[i]);
                ctx.gridCtrl.Add(zone.layers[i]);
            }
        }

        public void Add() {
            // instantiate new layer
            var layer = new Layer("layer", zone.width, zone.height);
            zone.AddLayer(layer);
            ctx.layerCtrl.Add(layer);
            ctx.gridCtrl.Add(layer);
        }


        public void Remove(
            Layer layer
        ) {
            ctx.layerCtrl.Remove(layer);
            ctx.gridCtrl.Remove(layer);
            zone.RemoveLayer(layer);
        }

        void Raise(
            Layer layer
        ) {
            ctx.layerCtrl.Raise(layer);
            ctx.gridCtrl.Raise(layer);
            zone.Raise(layer);
        }

        void Lower(
            Layer layer
        ) {
            ctx.layerCtrl.Lower(layer);
            ctx.gridCtrl.Lower(layer);
            zone.Lower(layer);
        }

        void Select(
            Layer layer,
            bool selected
        ) {
            if (selectedLayer != null) {
                ctx.layerCtrl.Select(selectedLayer, false);
                ctx.gridCtrl.Select(selectedLayer, false);

            }
            ctx.layerCtrl.Select(layer, selected);
            ctx.gridCtrl.Select(layer, selected);
            selectedLayer = layer;
        }

        void Visible(
            Layer layer,
            bool selected
        ) {
            ctx.gridCtrl.Visible(layer, selected);
        }

        void Clear() {
            // clear controller
            if (ctx != null) {
                ctx.layerCtrl.Clear();
                ctx.gridCtrl.Clear();
            }
            // clear zone and selected layer
            zone = null;
            selectedLayer = null;
        }


    }

}