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
        UxSpriteMgrCtrlCtx mgrCtx = null;
        List<AvailSpriteCtx> ctxs = new List<AvailSpriteCtx>();
        AvailSpriteCtx currentCtx = null;

        public bool ready { 
            get { return mgrCtx != null; }
        }

        public void Assign(
            UxSpriteMgrCtrlCtx mgrCtx
        ) {
            this.mgrCtx = mgrCtx;
        }

        public void Clear() {
            for (var i=ctxs.Count-1; i>=0; i--) {
                DeleteCtx(ctxs[i]);
            }
            currentCtx = null;
        }

        void DeleteCtx(
            AvailSpriteCtx ctx
        ) {
            // clean up sprite gameobject
            if (ctx.ctrl != null) {
                Destroy(ctx.ctrl.gameObject);
            }
            // remove from ctx list
            ctxs.Remove(ctx);
            if (currentCtx == ctx) {
                currentCtx = null;
            }
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
            }
        }
    }

}