using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace TyTe {

    // =========================================================================
    struct TilePos {
        // INSTANCE VARIABLES --------------------------------------------------
        public int x;
        public int y;

        // CONSTRUCTORS --------------------------------------------------------
        public TilePos(
            int x,
            int y
        ) {
            this.x = x;
            this.y = y;
        }

        // STATIC METHODS ------------------------------------------------------
        public static bool operator ==(TilePos a, TilePos b) {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(TilePos a, TilePos b) {
            return a.x != b.x || a.y != b.y;
        }

        // INSTANCE METHODS ----------------------------------------------------
        public override bool Equals(
            System.Object obj
        ) {
            return obj is TilePos && this == (TilePos)obj;
        }

        public override int GetHashCode() {
            return x.GetHashCode() ^ y.GetHashCode();
        }

        public override string ToString() {
            return "[" + x + "," + y + "]";
        }
    }

    // =========================================================================
    public class UxGridCtrlCtx {
        // INSTANCE VARIABLES --------------------------------------------------
        public Func<ToolKind> getCurrentToolFcn;
        public Func<SpriteRecord> getCurrentSpriteFcn;
        public Func<int,SpriteRecord> lookupSpriteFcn;
        public Action<int> setSpriteFcn;
    }

    // =========================================================================
    public class UxGridCtrl : MonoBehaviour {
        // INSTANCE VARIABLES --------------------------------------------------
        public RectTransform contentTransform;
        public GameObject spritePrefab;
        Layer layer;
        UxGridCtrlCtx ctrlCtx;
        Dictionary<TilePos,Image> imageMap = new Dictionary<TilePos, Image>();

        // UNITY METHODS -------------------------------------------------------
        void Awake() {
            Select(false);
        }

        // INSTANCE METHODS ----------------------------------------------------
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
                var pos = new TilePos(x,y);
                var spriteGO = UnityEngine.Object.Instantiate(spritePrefab, contentTransform);
                var img = spriteGO.GetComponent<Image>();
                var id = layer.Get(x,y);
                if (img != null) {
                    imageMap[pos] = img;
                    AssignTile(img, pos, ctrlCtx.lookupSpriteFcn(id));
                }
                // wire handlers
                var btn = spriteGO.GetComponent<Button>();
                if (btn != null) {
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

        void ClearTile(
            Image img,
            TilePos pos
        ) {
            if (img != null) {
                img.sprite = null;
                img.color = new Color(0,0,0,0);
            }
            layer.Set(pos.x, pos.y, 0);
        }

        void OnTileClick(
            Image img,
            TilePos pos
        ) {
            switch (ctrlCtx.getCurrentToolFcn()) {
            case ToolKind.paint:
                AssignTile(img, pos, ctrlCtx.getCurrentSpriteFcn());
                break;
            case ToolKind.fill:
                Fill(pos, ctrlCtx.getCurrentSpriteFcn());
                break;
            case ToolKind.erase:
                ClearTile(img, pos);
                break;
            case ToolKind.pick:
                var id = layer.Get(pos.x, pos.y);
                if (id != 0) {
                    ctrlCtx.setSpriteFcn(id);
                }
                break;
            }
        }

        void Fill(
            TilePos startPos,
            SpriteRecord sprite
        ) {
            // get starting tile kind
            var matchID = layer.Get(startPos.x, startPos.y);
            var visited = new HashSet<TilePos>();
            var traverse = new List<TilePos>();
            // add starting node to traverse list
            traverse.Add(startPos);
            // iterate through list of nodes to traverse
            while (traverse.Count > 0) {
                var pos = traverse[0];
                traverse.RemoveAt(0);
                // update current node
                AssignTile(imageMap[pos], pos, sprite);
                visited.Add(pos);
                // check/add neighbors
                var neighbors = new TilePos[] {
                    new TilePos(pos.x-1,pos.y),
                    new TilePos(pos.x+1,pos.y),
                    new TilePos(pos.x,pos.y-1),
                    new TilePos(pos.x,pos.y+1),
                };
                foreach (var neighbor in neighbors) {
                    // bounds
                    if (neighbor.x < 0 || neighbor.x >= layer.width || neighbor.y < 0 || neighbor.y >= layer.height) continue;
                    // visited
                    if (visited.Contains(neighbor)) continue;
                    // already set to traverse
                    if (traverse.Contains(neighbor)) continue;
                    // non-matching tile
                    if (layer.Get(neighbor.x, neighbor.y) != matchID) continue;
                    // tile matches
                    traverse.Add(neighbor);
                }
            }
        }

    }

}