using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfWarBuilders.Entities;
using Tools.UI;
using Tools.Extensions;


namespace AgeOfWarBuilders.BuildSystem
{
    public class BuildManager : MonoBehaviour
    {
        public UI_BuildSelector ui;

        public List<BuildData> ObjectsToBuild;
        public int ObjectCount { get { return ObjectsToBuild.Count; } }
        int indexCursor;

        public List<GameObject> MyBuildings = new List<GameObject>(); 

        #region Apertura y cierre del modo de Contruccion
        [SerializeField] Canvas BuildCanvas;
        [SerializeField] UI_ImageFiller filler;
        float timer;
        float Time_To_TurnOn = 1f;
        bool build_button_pressed = false;
        bool buildModeActive = false;
        bool oneshot;
        void Update_OpenCloseMode()
        {
            if (PlayerController.PRESS_DOWN_BuildMode)
            {
                //no estoy dentro, inicio la cuenta y prendo el filler
                if (!buildModeActive)
                {
                    build_button_pressed = true;
                    filler.TurnOn();
                }
                //ya estoy dentro, con el mismo boton lo apago
                else
                {
                    filler.TurnOff();
                    buildModeActive = false;
                    ExitToBuildMode();
                }

            }
            if (PlayerController.PRESS_Up_BuildMode)
            {
                //no estoy dentro, pero me cancelaron antes, cancelo todo
                if (!buildModeActive)
                {
                    build_button_pressed = false;
                    filler.TurnOff();
                }
                else
                {
                    //nada porque si estoy dentro y suelto la tecla no quiero nada
                }

            }

            if (build_button_pressed)
            {
                if (timer < Time_To_TurnOn)
                {
                    timer = timer + 1 * Time.deltaTime;
                    filler.Fill(timer);
                }
                else
                {
                    filler.TurnOff();
                    timer = 0;
                    build_button_pressed = false;
                    buildModeActive = true;
                    EnterToBuildMode();
                }
            }
            else
            {
                timer = 0;
            }
        }
        void Turn_ON_Canvas() => BuildCanvas.gameObject.SetActive(true);
        void Turn_OFF_Canvas() => BuildCanvas.gameObject.SetActive(false);
        #endregion

        #region Index Cursor Logic
        public void NextElement(float direction = 1) // 1:adelante  -1:atras
        {
            indexCursor = direction > 0 ? indexCursor.NextIndex(ObjectCount) : indexCursor.BackIndex(ObjectCount);
            RefreshFrontEnd();
        }
        bool oneshot_axis;
        void Update_CursorMovement()
        {
            if (PlayerController.AXIS__Horizontal_ARROWS != 0)
            {
                if (!oneshot_axis)
                {
                    NextElement(PlayerController.AXIS__Horizontal_ARROWS);
                    oneshot_axis = true;
                }
            }
            else
            {
                oneshot_axis = false;
            }
        }
        #endregion

        private void Start()
        {
            ui.Configurate(() => ObjectsToBuild, RefreshFrontEnd, OnUIAnimationEndClose);
        }

        void RefreshFrontEnd()
        {
            ui.Select(indexCursor);
            InstanciateStruct(ObjectsToBuild[indexCursor].model_BuildMode);
        }
        void OnUIAnimationEndClose()
        {
            indexCursor = 0;
        }

        void Update()
        {
            Update_OpenCloseMode();
            Update_CursorMovement();
            Update_PosToBuild();
            Update_Instance_Object();
        }

        void EnterToBuildMode()
        {
            Turn_ON_Canvas();
            ui.Open();

        }
        void ExitToBuildMode()
        {
            ui.InstantClose();
            Turn_OFF_Canvas();
        }

        #region Place Real Object 
        void Update_Instance_Object()
        {
            if (PlayerController.PRESS_DOWN_Submit)
            {
                if (currentObject_BuildChecker == null) return;
                if (currentObject_BuildChecker.CanBuild)
                {
                    GameObject go = GameObject.Instantiate(ObjectsToBuild[indexCursor].model, LocalSceneTransforms.parent_MyBuildings);
                    var posrot = GetPosRot();
                    go.transform.position = posrot.pos;
                    go.transform.eulerAngles = posrot.rot;
                    MyBuildings.Add(go);
                }
            }
        }
        #endregion

        #region Object Preview
        public Transform InstancePosition;
        public GameObject currentObject;
        ObjectBuildChecker currentObject_BuildChecker;
        Vector3 posToInstanciate;
        public LayerMask layer_PlacesToBuild;
        Vector3 euler_rot;
        bool can_refresh_build_transform;

        void Update_PosToBuild()
        {
            if (currentObject)
            {
                RaycastHit hit;
                can_refresh_build_transform = Physics.Raycast(InstancePosition.transform.position + Vector3.up * 5, Vector3.up * -1, out hit, 10, layer_PlacesToBuild);

                if (can_refresh_build_transform)
                {

                    Debug.Log("Estoy updateando");
                    posToInstanciate = hit.point;
                    currentObject.transform.position = posToInstanciate;
                    currentObject.transform.eulerAngles = euler_rot;

                }
            }
        }

        void InstanciateStruct(GameObject go)
        {
            if (currentObject)
            {
                Destroy(currentObject);
                currentObject_BuildChecker = null;
            }
            currentObject = GameObject.Instantiate(go);
            currentObject.transform.position = posToInstanciate;
            currentObject.transform.eulerAngles = euler_rot;
            currentObject_BuildChecker = currentObject.GetComponent<ObjectBuildChecker>();
        }

        public posrot GetPosRot()
        {
            return new posrot(posToInstanciate, euler_rot);
        }
        public struct posrot
        {
            internal Vector3 pos;
            internal Vector3 rot;
            public posrot(Vector3 pos, Vector3 rot)
            {
                this.pos = pos;
                this.rot = rot;
            }
        }
        #endregion
    }
}

