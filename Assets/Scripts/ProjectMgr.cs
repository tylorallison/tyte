using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TyTe {

    public class ProjectMgr : UxPanel {
        [Header("Controllers")]
        public UxZoneMgrCtrl zoneCtrl;
        public UxToolMgrCtrl toolCtrl;
        public UxSpriteMgrCtrl spriteCtrl;
        public UxLayerMgrCtrl layerCtrl;
        public UxGridMgrCtrl gridCtrl;
        [Header("Other UI elements")]
        public Button projectButton;
        [Header("Prefabs")]
        public GameObject projectPrefab;
        [Header("Managers")]
        public SpriteMgr spriteMgr;
        public LayerMgr layerMgr;
        public ToolMgr toolMgr;
        public ZoneMgr zoneMgr;
        public bool setupStarted = false;
        public bool setupDone = false;
        public string prefsProjectKey = "project";
        Project project;

        void Start() {
            // wire listeners
            projectButton.onClick.AddListener(() => {
                PopupProjectPanel();
             });
        }

        void Load() {
            var json = PlayerPrefs.GetString(prefsProjectKey);
            if (json != "") {
                // load project from json
                project = Project.FromJson(json);
            }
        }

        void Save() {
            var json = Project.AsJson(project);
            PlayerPrefs.SetString(prefsProjectKey, json);
        }

        void PopupProjectPanel() {
            var panelGo = Instantiate(projectPrefab, GetComponentInParent<Canvas>().gameObject.transform);
            // setup a callback, so when the sub menu/panel is done, we display the current panel again
            var ctrl = panelGo.GetComponent<UxProjectCtrl>();
            ctrl.onDoneEvent.AddListener(Enable);
            // assign panel ctx and project
            ctrl.AssignCtx( new UxProjectCtrlCtx {
                chgProjectPathFcn = (val) => {
                    project.projectPath = val;
                    Save();
                },
                chgZoneFolderFcn = (val) => {
                    project.zoneFolder = val;
                    Save();
                },
                chgSpriteFolderFcn = (val) => {
                    project.spriteFolder = val;
                    Save();
                },
                chgSpriteJsonFcn = (val) => {
                    project.spriteJson = val;
                    Save();
                },
            });
            ctrl.AssignProject( project );
            // now hide the current panel
            Disable();
        }

        IEnumerator Setup() {
            // load project settings...
            Load();
            // initialize new project if one cannot be loaded
            if (project == null) {
                project = new Project();
                // popup project ctrl
                PopupProjectPanel();
            }
            // wait for project panel to be cloased
            while( !active ) {
                yield return null;      // wait for next update
            }

            // setup sprite manager
            spriteMgr.AssignCtx( new SpriteMgrCtx {
                project = project,
                ctrl = spriteCtrl,
            });
            // wait for manager to become ready
            while (!spriteMgr.ready) {
                yield return null;      // wait for next update
            }

            // setup tool manager
            toolMgr.AssignCtx( new ToolMgrCtx {
                ctrl = toolCtrl,
            });
            // wait for manager to become ready
            while (!toolMgr.ready) {
                yield return null;      // wait for next update
            }

            // setup grid manager
            gridCtrl.AssignCtx( new UxGridMgrCtrlCtx {
                gridCtrlCtx = new UxGridCtrlCtx {
                    getCurrentToolFcn = () => toolMgr.selected,
                    getCurrentSpriteFcn = () => spriteMgr.selected,
                    lookupSpriteFcn = spriteMgr.Lookup,
                    setSpriteFcn = spriteMgr.Select,
                },
            });

            // setup layer manager
            layerMgr.AssignCtx( new LayerMgrCtx {
                layerCtrl = layerCtrl,
                gridCtrl = gridCtrl,
            });
            // wait for manager to become ready
            while (!layerMgr.ready) {
                yield return null;      // wait for next update
            }

            // setup zone manager
            zoneMgr.AssignCtx( new ZoneMgrCtx {
                project = project,
                ctrl = zoneCtrl,
                assignZoneFcn = (zone) => {
                    layerMgr.AssignZone(zone);
                },
            });
            // wait for manager to become ready
            while (!zoneMgr.ready) {
                yield return null;      // wait for next update
            }
            setupDone = true;
        }

        public void Update() {
            if (!setupStarted) {
                StartCoroutine(Setup());
                setupStarted = true;
            }
        }
    }

}