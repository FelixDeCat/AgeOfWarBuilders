using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfWarBuilders.Entities;
using Tools.UI;
using Tools.Extensions;
using UnityEngine.Events;
using Tools.Structs;
using System;


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

        public int MaxCant_Towers = 20;
        public int currentCant_Towers = 0;
        public GenericBar_Sprites bar;

        public bool i_have_space_to_build => currentCant_Towers < MaxCant_Towers;

        [SerializeField] AudioClip BuildSucessful;
        [SerializeField] AudioClip Click_Tower_Switch;
        [SerializeField] AudioClip BuildNegate;
        [SerializeField] AudioClip Enter_Build_Mode;
        [SerializeField] AudioClip Exit_Build_Mode;


        void PlayClip_BuildSucessful() => AudioManager.instance.PlaySound(BuildSucessful.name, transform);
        void PlayClip_Click_Tower_Switch() => AudioManager.instance.PlaySound(Click_Tower_Switch.name, transform);
        void PlayClip_BuildNegate() => AudioManager.instance.PlaySound(BuildNegate.name, transform);
        void PlayClip_Enter() => AudioManager.instance.PlaySound(Enter_Build_Mode.name, transform);
        void PlayClip_Exit() => AudioManager.instance.PlaySound(Exit_Build_Mode.name, transform);

        private void Start()
        {
            if (list_of_build_data == null || list_of_build_data.Count < 1) throw new System.Exception("No hay data cargada");
            ui.Configurate(() => list_of_build_data, OnUIAnimationEnd_OPEN, OnUIAnimationEnd_CLOSE);

            bar.Configure(MaxCant_Towers, 0.01f,0);

            AudioManager.instance.GetSoundPool(BuildSucessful.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, BuildSucessful);//
            AudioManager.instance.GetSoundPool(Click_Tower_Switch.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, Click_Tower_Switch);//
            AudioManager.instance.GetSoundPool(BuildNegate.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, BuildNegate);//
            AudioManager.instance.GetSoundPool(Enter_Build_Mode.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, Enter_Build_Mode);//
            AudioManager.instance.GetSoundPool(Exit_Build_Mode.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, Exit_Build_Mode);//
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
            PlayClip_Enter();

        }
        void ExitToBuildMode()
        {
            OnCloseBuildMode.Invoke();
            ui.InstantClose();
            Turn_OFF_Canvas();
            Hide();
            PlayClip_Exit();
        }
        #endregion

        #region [LOGIC] Destroy Tower
        void OnDeathTower(TowerEntity tower, Vector3 lastPosition)
        {
            currentCant_Towers--;
            tower.DeInitialize();
            MyBuildings.Remove(tower);


            ParticlesPoolManager.Play_DestroyTower(lastPosition + Vector3.up * 2);

            RefreshTowerQuantity(currentCant_Towers, MaxCant_Towers);
        }
        #endregion

        #region [FRONTEND] TOWERS QUANTITY
        public void RefreshTowerQuantity(int current, int max)
        {
            bar.Configure(max, 0.01f);
            bar.SetValue(current);
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
                    PlayClip_Click_Tower_Switch();
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

                var resource = ConvertRecipeToTupleArray(indexHorizontalCursor);

                if (currentObject_BuildChecker == null) return;
                if (currentObject_BuildChecker.CanBuild && 
                    hit.collider.tag != "NotBuild" &&
                    i_have_space_to_build /*&& 
                    ResourceManager.instance.IHaveThisResourcePackage(resource)*/)
                {
                    //Arreglar esto del Pool
                    /*
                    PlayObject_PoolManager.instance.Feed(list_of_build_data[indexHorizontalCursor].model, LocalSceneTransforms.parent_MyBuildings);
                    var go = PlayObject_PoolManager.instance.Get(list_of_build_data[indexHorizontalCursor].model.type, GetPosRot().pos, GetPosRot().rot);
                    */

                    /*
                    ResourceManager.instance.SpendResourcePackage(resource);
                    */

                    TowerEntity go = GameObject.Instantiate(list_of_build_data[indexHorizontalCursor].model, this.transform);
                    go.transform.eulerAngles = GetPosRot().rot;
                    go.transform.position = GetPosRot().pos;
                    go.Initialize();
                    go.CallbackOnDeath(OnDeathTower);
                    MyBuildings.Add(go);
                    PlayClip_BuildSucessful();
                    currentCant_Towers++;
                    RefreshTowerQuantity(currentCant_Towers, MaxCant_Towers);
                }
                else
                {
                    PlayClip_BuildNegate();
                }
            }
        }
        Tuple<ResourceType, int>[] ConvertRecipeToTupleArray(int indexCursor)
        {
            var data = list_of_build_data[indexHorizontalCursor].requirements;
            var packdata = new Tuple<ResourceType, int>[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                packdata[i] = Tuple.Create(data[i].resource, data[i].Cant);
            }

            return packdata;
        }
        #endregion

        #region [LOGIC] Object Preview
        public Transform InstancePosition;
        public GameObject currentObject;
        ObjectBuildChecker currentObject_BuildChecker;
        Vector3 posToInstanciate;
        public LayerMask leyer_to_interact;
        Vector3 euler_rot;
        public Vector3 frontoffset;
        public float max_Object_Forward_offset = 5f;
        bool can_refresh_build_transform;
        RaycastHit hit;
        void Update_PosToBuild()
        {
            if (currentObject)
            {

                var resource = ConvertRecipeToTupleArray(indexHorizontalCursor);

                Vector3 posToRaycast = InstancePosition.transform.position + InstancePosition.forward * scrollvalue;

                can_refresh_build_transform = Physics.Raycast(posToRaycast + Vector3.up * 5, Vector3.up * -1, out hit, 10, leyer_to_interact);

                if (can_refresh_build_transform)
                {
                    posToInstanciate = hit.point;
                    currentObject.transform.position = posToInstanciate;
                    currentObject.transform.eulerAngles = euler_rot;
                }

                currentObject_BuildChecker.SetAuxiliarCanBuild(hit.collider.tag != "NotBuild" /*&& ResourceManager.instance.IHaveThisResourcePackage(resource)*/);
                
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
            if (scrollvalue < 0) scrollvalue = 0;
            if (scrollvalue > max_Object_Forward_offset) scrollvalue = max_Object_Forward_offset;

        }
        #endregion
    }
}

