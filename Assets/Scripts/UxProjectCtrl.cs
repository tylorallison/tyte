using System;
using UnityEngine;
using UnityEngine.UI;

namespace TyTe {
    public class UxProjectCtrlCtx {
        public Action<string> chgProjectPathFcn;
        public Action<string> chgZoneFolderFcn;
        public Action<string> chgSpriteFolderFcn;
        public Action<string> chgSpriteJsonFcn;
    }

    public class UxProjectCtrl : UxPanel {
        public Button okButton;
        public InputField projectPathField;
        public InputField zoneFolderField;
        public InputField spriteFolderField;
        public InputField spriteJsonField;
        bool listen = true;
        Project project;

        UxProjectCtrlCtx ctx;

        void Awake() {
            // wire listeners
            projectPathField.onValueChanged.AddListener((val) => { 
                if (listen && ctx != null) ctx.chgProjectPathFcn(projectPathField.text); 
                okButton.interactable = project.IsValid();
            });
            zoneFolderField.onValueChanged.AddListener((val) => { 
                if (listen && ctx != null) ctx.chgZoneFolderFcn(zoneFolderField.text); 
                okButton.interactable = project.IsValid();
            });
            spriteFolderField.onValueChanged.AddListener((val) => { 
                if (listen && ctx != null) ctx.chgSpriteFolderFcn(spriteFolderField.text); 
                okButton.interactable = project.IsValid();
            });
            spriteJsonField.onValueChanged.AddListener((val) => { 
                if (listen && ctx != null) ctx.chgSpriteJsonFcn(spriteJsonField.text); 
                okButton.interactable = project.IsValid();
            });
            okButton.onClick.AddListener(() => { Destroy(gameObject); });
            listen = true;
        }

        public void AssignCtx(
            UxProjectCtrlCtx ctx
        ) {
            this.ctx = ctx;
        }

        public void AssignProject(
            Project project
        ) {
            this.project = project;
            // disable listeners
            listen = false;
            // set field values
            projectPathField.text = project.projectPath;
            zoneFolderField.text = project.zoneFolder;
            spriteFolderField.text = project.spriteFolder;
            spriteJsonField.text = project.spriteJson;
            // reenable listeners
            listen = true;
            // set OK button status
            okButton.interactable = project.IsValid();
        }
    }
}