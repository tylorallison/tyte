using UnityEngine;

namespace TyTe {

    // =========================================================================
    public enum ToolKind {
        none,
        paint,
        fill,
        erase,
        pick,
    }

    // =========================================================================
    public class ToolMgrCtx {
        public UxToolMgrCtrl ctrl;
    }

    // =========================================================================
    public class ToolMgr : MonoBehaviour {
        // INSTANCE VARIABLES --------------------------------------------------
        public bool setup = false;
        ToolMgrCtx ctx;
        ToolKind selectedTool = ToolKind.none;

        // PROPERTIES ----------------------------------------------------------
        public bool ready { 
            get { return setup; }
        }
        public ToolKind selected { 
            get { return selectedTool; }
        }

        // UNITY METHODS -------------------------------------------------------
        void Update() {
            if (!setup) Setup();
        }

        // INSTANCE METHODS ----------------------------------------------------
        public void Setup() {
            if (ctx == null) return;
            AssignTool(ToolKind.paint);
            setup = true;
        }

        public void AssignCtx(
            ToolMgrCtx ctx
        ) {
            this.ctx = ctx;
            // assign ctrl ctx
            ctx.ctrl.AssignCtx( new UxToolMgrCtrlCtx {
                selectToolFcn = AssignTool,
            });
        }

        public void AssignTool(
            ToolKind tool
        ) {
            Debug.Log("selected tool: " + selectedTool + " new tool: " + tool);
            if (selectedTool != ToolKind.none) {
                ctx.ctrl.AssignSelected(selectedTool, false);
            }
            selectedTool = tool;
            ctx.ctrl.AssignSelected(selectedTool, true);
        }

    }

}