using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfWarBuilders.Entities;
using Tools.UI;
using Tools.Extensions;
using UnityEngine.Events;
using Tools.Structs;


namespace AgeOfWarBuilders.BuildSystem
{
    public class BuildManager : MonoBehaviour
    {
        [SerializeField] UnityEvent OnOpenBuildMode;
        [SerializeField] UnityEvent OnCloseBuildMode;

        public UI_BuildSelector ui;

        public List<BuildData> list_of_build_data;
        public int ObjectCount { get { return list_of_build_data.Count; } }
        [SerializeField] int indexHorizontalCursor;

        public List<PlayObject> MyBuildings = new List<PlayObject>(); 

        private void Start()
        {
            if (list_of_build_data == null || list_of_build_data.Count < 1) throw new System.Exception("No hay data cargada");
            ui.Configurate(() => list_of_build_data, OnUIAnimationEnd_OPEN, OnUIAnimationEnd_CLOSE);
        }

        #region [TICK]
        void Update()
        {
            Update_OpenCloseMode();
            if (buildModeActive)
            {
                Update_CursorMovement();
                Update_PosToBuild();
                Update_Instance_Object();
                Update_ObjectTransform();
            }
        }
        #endregion

        #region [EXPOSITION] END Animation Events
        void OnUIAnimationEnd_OPEN()
        {
            ui.Select(indexHorizontalCursor);
            InstanciateStruct(list_of_build_data[indexHorizontalCursor].model_BuildMode);
        }
        void OnUIAnimationEnd_CLOSE()
        {
            indexHorizontalCursor = 0;
        }
        #endregion

        #region [EXPOSITION] Enter and Close to Build Mode
        void EnterToBuildMode()
        {
            OnOpenBuildMode.Invoke();
            Turn_ON_Canvas();
            ui.Open();

        }
        void ExitToBuildMode()
        {
            OnCloseBuildMode.Invoke();
            ui.InstantClose();
            Turn_OFF_Canvas();
            Hide();
        }
        #endregion

        #region [LOGIC] Open close BUILD MODE
        [SerializeField] CanvasGroup BuildCanvas;
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
        void Turn_ON_Canvas() => BuildCanvas.alpha = 1;
        void Turn_OFF_Canvas() => BuildCanvas.alpha = 0;
        #endregion

        #region [LOGIC] Selection Index Cursor 
        public void NextHorizontalElement(float direction = 1) // 1:adelante  -1:atras
        {
            indexHorizontalCursor = direction > 0 ? indexHorizontalCursor.NextIndex(ObjectCount) : indexHorizontalCursor.BackIndex(ObjectCount);
            OnUIAnimationEnd_OPEN();
        }
        bool oneshot_axis;
        void Update_CursorMovement()
        {
            if (PlayerController.AXIS_Horizontal_ARROWS != 0)
            {
                if (!oneshot_axis)
                {
                    NextHorizontalElement(PlayerController.AXIS_Horizontal_ARROWS);
                    oneshot_axis = true;
                }
            }
            else
            {
                oneshot_axis = false;
            }

            //if (PlayerController.AXIS_Vertical_ARROWS != 0)
            //{

            //}
            //else
            //{

            //}
        }
        #endregion

        #region [LOGIC] Press to Build 
        void Update_Instance_Object()
        {
            if (PlayerController.PRESS_DOWN_Submit)
            {
                if (currentObject_BuildChecker == null) return;
                if (currentObject_BuildChecker.CanBuild && hit.collider.tag != "NotBuild")
                {
                    //Arreglar esto del Pool
                    /*
                    PlayObject_PoolManager.instance.Feed(list_of_build_data[indexHorizontalCursor].model, LocalSceneTransforms.parent_MyBuildings);
                    var go = PlayObject_PoolManager.instance.Get(list_of_build_data[indexHorizontalCursor].model.type, GetPosRot().pos, GetPosRot().rot);
                    */

                    TowerEntity go = GameObject.Instantiate( list_of_build_data[indexHorizontalCursor].model, LocalSceneTransforms.parent_MyBuildings);
                    go.transform.eulerAngles = GetPosRot().rot;
                    go.transform.position = GetPosRot().pos;
                    go.Initialize();

                    MyBuildings.Add(go);
                }
            }
        }
        #endregion

        #region [LOGIC] Object Preview
        public Transform InstancePosition;
        public GameObject currentObject;
        ObjectBuildChecker currentObject_BuildChecker;
        Vector3 posToInstanciate;
        public LayerMask layer_PlacesToBuild;
        Vector3 euler_rot;
        public Vector3 frontoffset;
        public float max_Object_Forward_offset = 5f;
        bool can_refresh_build_transform;
        RaycastHit hit;
        void Update_PosToBuild()
        {
            if (currentObject)
            {
                
                Vector3 posToRaycast = InstancePosition.transform.position + InstancePosition.forward * scrollvalue;

                can_refresh_build_transform = Physics.Raycast(posToRaycast + Vector3.up * 5, Vector3.up * -1, out hit, 10, layer_PlacesToBuild);

                if (can_refresh_build_transform)
                {
                    posToInstanciate = hit.point;
                    currentObject.transform.position = posToInstanciate;
                    currentObject.transform.eulerAngles = euler_rot;
                }

                currentObject_BuildChecker.SetAuxiliarCanBuild(hit.collider.tag != "NotBuild");
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
        void Hide()
        {
            if (currentObject) { Destroy(currentObject); }
            currentObject_BuildChecker = null;
        }

        public posrot GetPosRot()
        {
            return new posrot(posToInstanciate, euler_rot);
        }

        #endregion

        #region [LOGIC] Object Transform
        float scrollvalue;
        void Update_ObjectTransform()
        {
            var y_rot = euler_rot.y + PlayerController.AXIS_MouseBUTTONS * 100 * Time.deltaTime;
            euler_rot = new Vector3(0, y_rot, 0);

            scrollvalue += PlayerController.AXIS_MouseScrollWheel;
            if(scrollvalue < 0) scrollvalue = 0;
            if (scrollvalue > max_Object_Forward_offset) scrollvalue = max_Object_Forward_offset;

        }
        #endregion
    }
}

