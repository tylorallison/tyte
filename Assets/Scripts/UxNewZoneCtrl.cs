using System;
using UnityEngine;
using UnityEngine.UI;

namespace TyTe {

    public class UxNewZoneCtrlCtx {
        public Action<string> chgNameFcn;
        public Action<int> chgWidthFcn;
        public Action<int> chgHeightFcn;
        public Action okFcn;
    }

    public class UxNewZoneCtrl : UxPanel {

        public InputField nameField;
        public InputField widthField;
        public InputField heightField;
        public Button okButton;
        public Button cancelButton;

        UxNewZoneCtrlCtx ctx;
        Zone zone;

        public void AssignCtx(
            UxNewZoneCtrlCtx ctx
        ) {
            this.ctx = ctx;
            // wire handlers
            nameField.onValueChanged.AddListener((val) => { 
                ctx.chgNameFcn(val);
                okButton.interactable = zone.IsValid();
            });
            widthField.onValueChanged.AddListener((val) => { 
                var intVal = Int32.Parse(val);
                ctx.chgWidthFcn(intVal) ;
                okButton.interactable = zone.IsValid();
            });
            heightField.onValueChanged.AddListener((val) => { 
                var intVal = Int32.Parse(val);
                ctx.chgHeightFcn(intVal) ;
                okButton.interactable = zone.IsValid();
            });
            okButton.onClick.AddListener(() => { 
                ctx.okFcn() ;
                Destroy(gameObject); 
            });
            cancelButton.onClick.AddListener(() => { 
                Destroy(gameObject); 
            });
        }

        public void AssignZone(
            Zone zone
        ) {
            this.zone = zone;
            this.nameField.text = zone.name;
            this.widthField.text = zone.width.ToString();
            this.heightField.text = zone.height.ToString();
        }

    }
}