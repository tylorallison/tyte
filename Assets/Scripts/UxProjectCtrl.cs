using System;
using UnityEngine;
using UnityEngine.UI;

namespace TyTe {
    public class UxProjectCtrlCtx {
        public Action<string> chgProjectPathFcn;
        public Action<string> chgZoneFolderFcn;
        public Action<string> chgSpriteFolderFcn;
        public Action<string> chgSpriteJsonFcn;
        public Action<int> chgTileWidthFcn;
        public Action<int> chgTileHeightFcn;
    }

    public class UxProjectCtrl : UxPanel {
        public Button okButton;
        public InputField projectPathField;
        public InputField zoneFolderField;
        public InputField spriteFolderField;
        public InputField spriteJsonField;
        public InputField tileWidthField;
        public InputField tileHeightField;
        bool listen = true;
        Project project;

        UxProjectCtrlCtx ctx;

        public void AssignCtx(
            UxProjectCtrlCtx ctx
        ) {
            this.ctx = ctx;

            // wire listeners
            projectPathField.onValueChanged.AddListener((val) => { 
                if (listen) ctx.chgProjectPathFcn(projectPathField.text); 
                okButton.interactable = project.IsValid();
            });
            zoneFolderField.onValueChanged.AddListener((val) => { 
                if (listen) ctx.chgZoneFolderFcn(zoneFolderField.text); 
                okButton.interactable = project.IsValid();
            });
            spriteFolderField.onValueChanged.AddListener((val) => { 
                if (listen) ctx.chgSpriteFolderFcn(spriteFolderField.text); 
                okButton.interactable = project.IsValid();
            });
            spriteJsonField.onValueChanged.AddListener((val) => { 
                if (listen) ctx.chgSpriteJsonFcn(spriteJsonField.text); 
                okButton.interactable = project.IsValid();
            });
            tileWidthField.onValueChanged.AddListener((val) => { 
                if (listen) ctx.chgTileWidthFcn(Int32.Parse(tileWidthField.text)); 
                okButton.interactable = project.IsValid();
            });
            tileHeightField.onValueChanged.AddListener((val) => { 
                if (listen) ctx.chgTileHeightFcn(Int32.Parse(tileHeightField.text)); 
                okButton.interactable = project.IsValid();
            });
            okButton.onClick.AddListener(() => { Destroy(gameObject); });
            listen = true;
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
            tileWidthField.text = project.tileWidth.ToString();
            tileHeightField.text = project.tileHeight.ToString();
            // reenable listeners
            listen = true;
            // set OK button status
            okButton.interactable = project.IsValid();
        }
    }
}