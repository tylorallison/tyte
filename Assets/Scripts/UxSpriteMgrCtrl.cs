using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace TyTe {

    class AvailSpriteCtx {
        public SpriteRecord record;
        public UxAvailSpriteCtrl ctrl;

        public AvailSpriteCtx(
            SpriteRecord record,
            UxAvailSpriteCtrl ctrl
        ) {
            this.record = record;
            this.ctrl = ctrl;
        }
    }

    // =========================================================================
    // contract between manager and controller
    public class UxSpriteMgrCtrlCtx {
        public Action<SpriteRecord> selectSpriteFcn;
        public Action reloadFcn;
    }

    public class UxSpriteMgrCtrl : MonoBehaviour {
        // Ux references
        public RectTransform contentTransform;
        public GameObject spritePrefab;
        public Button reloadButton;
        UxSpriteMgrCtrlCtx mgrCtx = null;
        AvailSpriteCtx currentCtx = null;
        Dictionary<int,AvailSpriteCtx> ctxMap = new Dictionary<int, AvailSpriteCtx>();

        public bool ready { 
            get { return mgrCtx != null; }
        }

        public void Assign(
            UxSpriteMgrCtrlCtx mgrCtx
        ) {
            this.mgrCtx = mgrCtx;
            // wire listeners
            reloadButton.onClick.AddListener( () => { this.mgrCtx.reloadFcn(); });
        }

        public void Clear() {
            foreach (var ctx in ctxMap.Values) {
                Destroy(ctx.ctrl.gameObject);
            }
            ctxMap.Clear();
            currentCtx = null;
        }

        public void AddSprite(
            SpriteRecord record
        ) {
            if (spritePrefab == null) return;
            // instantiate sprite prefab
            var spriteGO = UnityEngine.Object.Instantiate(spritePrefab, contentTransform);
            // grab controller
            var spriteCtrl = spriteGO.GetComponent<UxAvailSpriteCtrl>();
            if (spriteCtrl != null) {
                // create ctx
                var ctx = new AvailSpriteCtx(record, spriteCtrl);
                // assign sprite image
                spriteCtrl.AssignSprite(record.sprite);
                // wire handlers
                // -- select button
                spriteCtrl.selectButton.onClick.AddListener(() => {
                    if (currentCtx != null) {
                        currentCtx.ctrl.Select(false);
                    }
                    spriteCtrl.Select(true);
                    mgrCtx.selectSpriteFcn(record);
                    currentCtx = ctx;
                });
                // add to ctx map
                ctxMap[record.id] = ctx;
            }
        }

        public void SelectSprite(
            SpriteRecord record
        ) {
            AvailSpriteCtx ctx;
            if (ctxMap.TryGetValue(record.id, out ctx)) {
                if (currentCtx != null) {
                    currentCtx.ctrl.Select(false);
                }
                ctx.ctrl.Select(true);
                currentCtx = ctx;
            }
        }

    }

}