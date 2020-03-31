using System;
using UnityEngine;
using UnityEngine.UI;

namespace TyTe {

    // =========================================================================
    public class UxToolMgrCtrlCtx {
        public Action<ToolKind> selectToolFcn;
    }

    // =========================================================================
    public class UxToolMgrCtrl : MonoBehaviour {

        // INSTANCE VARIABLES --------------------------------------------------
        public GameObject paintSelectedBg;
        public GameObject fillSelectedBg;
        public GameObject eraseSelectedBg;
        public GameObject pickSelectedBg;
        public Button paintButton;
        public Button fillButton;
        public Button eraseButton;
        public Button pickButton;
        UxToolMgrCtrlCtx ctx;

        // INSTANCE METHODS ----------------------------------------------------
        public void AssignCtx(
            UxToolMgrCtrlCtx ctx
        ) {
            this.ctx = ctx;
            // wire listeners
            paintButton.onClick.AddListener( () => { ctx.selectToolFcn(ToolKind.paint); });
            fillButton.onClick.AddListener( () => { ctx.selectToolFcn(ToolKind.fill); });
            eraseButton.onClick.AddListener( () => { ctx.selectToolFcn(ToolKind.erase); });
            pickButton.onClick.AddListener( () => { ctx.selectToolFcn(ToolKind.pick); });

        }

        public void AssignSelected(
            ToolKind kind,
            bool selected
        ) {
            switch (kind) {
            case ToolKind.paint:
                paintSelectedBg.SetActive(selected);
                break;
            case ToolKind.fill:
                fillSelectedBg.SetActive(selected);
                break;
            case ToolKind.erase:
                eraseSelectedBg.SetActive(selected);
                break;
            case ToolKind.pick:
                pickSelectedBg.SetActive(selected);
                break;
            }
        }

    }
}
