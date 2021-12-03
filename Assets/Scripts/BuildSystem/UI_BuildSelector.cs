using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_BuildSelector : UI_Base
{
    [SerializeField] UI_ItemBuildeable model;
    [SerializeField] RectTransform parent_to_instanciate;
    List<UI_ItemBuildeable> items = new List<UI_ItemBuildeable>();
    Func<List<BuildData>> GetData = delegate { return null; };
    Action Callback_OnEndCloseAnimation;
    Action Callback_OnEndOpenAnimation;

    [SerializeField] ui_item_recipe ui_item_recipe_model;

    public void Configurate(Func<List<BuildData>> _data, Action _OnEndOpenAnimation, Action _OnEndCloseAnimation)
    {
        GetData = _data;
        Callback_OnEndCloseAnimation = _OnEndCloseAnimation;
        Callback_OnEndOpenAnimation = _OnEndOpenAnimation;
    }

    public override void Refresh()
    {
        var col = GetData();

        for (int i = 0; i < col.Count; i++)
        {
            int externalCount = i + 1;

            // ExternalCount quiere decir que si el count de lo que tengo que refrescar es mayor
            // al count de las cosas que ya tengo creadas, quiere decir que me esta
            // faltando 1, por lo tanto creo 1 nuevo... si no es asi... solo le actualizo los valores
            if (externalCount > items.Count)
            {
                //creo 1 nuevo
                var go = GameObject.Instantiate(model, parent_to_instanciate);
                go.SetImage(col[i].item_image);
                go.SetRequirements(col[i].requirements, ui_item_recipe_model);
                items.Add(go);
            }
            else
            {
                items[i].SetImage(col[i].item_image);
                items[i].Refresh();
                //solo actualizo
            }
        }
    }

    public override void InstantClose()
    {
        base.InstantClose();
        UI_Selector.Hide();
    }

    public void Select(int index)
    {
        if (items.Count > 0)
            items[index].Select();
    }

    protected override void OnEndCloseAnimation() => Callback_OnEndCloseAnimation.Invoke();
    protected override void OnEndOpenAnimation() => Callback_OnEndOpenAnimation.Invoke();

    protected override void OnAwake() { }
    protected override void OnStart() { }
    protected override void OnUpdate() { }

}
