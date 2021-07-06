using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Resources : MonoBehaviour
{
    [SerializeField] RectTransform parent;
    [SerializeField] UI_Res_Elemet model;

    Dictionary<ResourceType, UI_Res_Elemet> elements = new Dictionary<ResourceType, UI_Res_Elemet>();

    public void CreateUI(Sprite s, string quant, ResourceType type)
    {
        if (!elements.ContainsKey(type))
        {
            var e = Instantiate(model, parent);
            e.Configurate(s);
            e.SetQuant("0");
            elements.Add(type, e);
        }
    }

    public void UpdateResource(ResourceType type_key, string quant) => elements[type_key].SetQuant(quant);

    public void AnimateAddResource(ResourceType type_key) => elements[type_key].Animate_Add();
    public void AnimateRemoveResource(ResourceType type_key) => elements[type_key].Animate_Remove();
    public void AnimateCanNotAddResource(ResourceType type_key) => elements[type_key].Animate_CanNotAdd();
    public void AnimateCanNotRemoveResource(ResourceType type_key) => elements[type_key].Animate_CanNotRemove();
}
