using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Tools.Structs;
using System;

public class UI_ItemBuildeable : UI_CustomSelectable
{
    public RectTransform parent_requirements;
    bool oneshot;
    ui_item_recipe[] uis;

    Tuple<ResourceType, int>[] package = new Tuple<ResourceType, int>[0];

    protected override void OnSelect()
    {

    }

    public void SetRequirements(Item_Recipe[] recipe, ui_item_recipe model)
    {
        if (!oneshot)
        {
            oneshot = true;

            package = new Tuple<ResourceType, int>[recipe.Length];
            uis = new ui_item_recipe[recipe.Length];

            for (int i = 0; i < recipe.Length; i++)
            {
                var ui = GameObject.Instantiate(model, parent_requirements);
                ui.SetValues(ResourceManager.instance.GetSprite(recipe[i].resource), recipe[i].Cant);

                package[i] = Tuple.Create(recipe[i].resource, recipe[i].Cant);
                uis[i] = ui;
            }


            ResourceManager.instance.SubscribeToChanges(Refresh);
        }
    }

    public void Refresh()
    {
        foreach (var r in uis)
        {
            r.IHave(ResourceManager.instance.IHaveThisResourcePackage(package));
        }
    }
}
