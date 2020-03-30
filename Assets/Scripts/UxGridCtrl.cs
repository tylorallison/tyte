using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace TyTe {

    struct TilePos {
        public int x;
        public int y;
        public TilePos(
            int x,
            int y
        ) {
            this.x = x;
            this.y = y;
        }
    }

    public class UxGridCtrlCtx {
        public Func<SpriteRecord> getCurrentSpriteFcn;
        public Func<int,SpriteRecord> lookupSpriteFcn;
    }

    public class UxGridCtrl : MonoBehaviour {
        public RectTransform contentTransform;
        public GameObject spritePrefab;

        Layer layer;
        UxGridCtrlCtx ctrlCtx;

        void Awake() {
            Select(false);
        }

        public void AssignCtx(
            UxGridCtrlCtx ctrlCtx
        ) {
            this.ctrlCtx = ctrlCtx;
        }

        public void AssignLayer(
            Layer layer
        ) {
            this.layer = layer;
            if (spritePrefab == null) return;
            // setup grid layout
            var grid = GetComponent<GridLayoutGroup>();
            if (grid == null) return;
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = layer.width;
            // iterate through grid
            // -- start 0,0 representing top-left corner
            for (var y=0; y<layer.height; y++)
            for (var x=0; x<layer.width; x++) {
                var spriteGO = UnityEngine.Object.Instantiate(spritePrefab, contentTransform);
                var img = spriteGO.GetComponent<Image>();
                var id = layer.Get(x,y);
                if (img != null) {
                    var record = ctrlCtx.lookupSpriteFcn(id);
                    if (record != null) {
                        img.sprite = record.sprite;
                        img.color = Color.white;
                    }
                }
                // wire handlers
                var btn = spriteGO.GetComponent<Button>();
                if (btn != null) {
                    var pos = new TilePos(x,y);
                    btn.onClick.AddListener(() => OnTileClick(img, pos));
                }
            }
        }

        public void Select(
            bool val
        ) {
            // pull canvas group
            var cvsGrp = GetComponent<CanvasGroup>();
            if (cvsGrp != null) {
                cvsGrp.blocksRaycasts = val;
                cvsGrp.interactable = val;
            }
        }

        public void Visible(
            bool val
        ) {
            gameObject.SetActive(val);
        }

        void AssignTile(
            Image img,
            TilePos pos,
            SpriteRecord record
        ) {
            if (record == null) return;
            // assign sprite to image
            if (img != null) {
                img.sprite = record.sprite;
                img.color = Color.white;
            }
            // assign record id to layer
            layer.Set(pos.x, pos.y, record.id);
        }

        void OnTileClick(
            Image img,
            TilePos pos
        ) {
            if (ctrlCtx.getCurrentSpriteFcn != null) {
                AssignTile(img, pos, ctrlCtx.getCurrentSpriteFcn());
            }
        }

    }

}