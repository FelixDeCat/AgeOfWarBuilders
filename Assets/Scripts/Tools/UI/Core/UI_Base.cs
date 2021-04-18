using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(UI_Anim_Code))] 
public abstract class UI_Base : MonoBehaviour
{
    [Header("UI_Base")]
    public GameObject firstToOpenMenu;
    [System.NonSerialized] public int idfinder;
    [System.NonSerialized] bool isActive;
    public bool IsActive { get { return isActive; } }

    UI_Anim_Code anim;
    [SerializeField] protected GameObject parent;
    void Awake()
    {
        OnAwake();
    }
    void Start() 
    {
        anim = GetComponent<UI_Anim_Code>();
        if (anim == null) throw new System.Exception("No contiene un UI_AnimBase");
        else anim.AddCallbacks(OnEndOpenAnimation, EndCloseAnimation);

        OnStart(); parent.SetActive(false); }
    void EndCloseAnimation() => OnEndCloseAnimation();
    void Update() { OnUpdate(); }
    protected abstract void OnAwake();
    protected abstract void OnStart();
    protected abstract void OnEndOpenAnimation();
    protected abstract void OnEndCloseAnimation();
    protected abstract void OnUpdate();
    public abstract void Refresh();
    public void ConfigurateFirst(GameObject go) => firstToOpenMenu = go;
    public void ForceDirectConfigurateFirst(GameObject go) { Main.instance.GetMyEventSystem().DeselectGameObject(); StartCoroutine(SelectButtonCoroutine(go)); }

    IEnumerator SelectButtonCoroutine(GameObject button)
    {
        yield return new WaitForEndOfFrame();
        Main.instance.GetMyEventSystem().Set_First(button);
    }

    public virtual void Open()
    {
        if(!isActive) anim.Open();
        parent.SetActive(true);
        Refresh();
        isActive = true;
        if(firstToOpenMenu) Main.instance.GetMyEventSystem().Set_First(firstToOpenMenu.gameObject);
    }
    public virtual void Close(bool buttonDeselect = false)
    {
        if (isActive)
        {
            anim.Close();
            parent.SetActive(false);
            isActive = false;
            if (buttonDeselect) Main.instance.GetMyEventSystem().DeselectGameObject();
        }
    }
}
