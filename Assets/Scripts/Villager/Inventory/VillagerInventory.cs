using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerInventory : MonoBehaviour
{
    bool hasTool = false;
    bool hasWeapon = false;
    bool hasFood = false;

    public bool HasTool => hasTool;
    public bool HasWeapon => hasWeapon;
    public bool HasFood => hasFood;

    public VillagerWeapon weapon_model;
    public VillagerTool tool_model;

    [SerializeField] VillagerTool tool_equiped;
    [SerializeField] Villager villager;

    GameObject go_weapon;
    GameObject go_tool;

    public void PickUpWeapon()
    {
        DropTool();

        go_weapon?.SetActive(true);
        hasWeapon = true;
    }
    public void PickUpTool()
    {
        DropWeapon();

        go_tool?.SetActive(true);
        tool_equiped?.ReconfigureTool(villager.GetProfession());
        hasTool = true;
    }

    #region Drop
    public void DropWeaponAndTool()
    {
        DropWeapon();
        DropTool();
    }
    void DropWeapon()
    {
        if (hasWeapon)
        {
            if (go_weapon)
            {
                go_weapon.SetActive(false);
                var go = Instantiate(weapon_model);
                go.transform.position = this.transform.position;
            }
            hasWeapon = false;
        }
    }
    void DropTool()
    {
        if (hasTool)
        {
            if (go_tool)
            {
                go_tool.SetActive(false);
                var go = Instantiate(tool_model);
                go.transform.position = this.transform.position;
            }
            
            hasTool = false;
        }
    }
    #endregion

    public void PickUpFood()
    {
        hasFood = true;
    }
    public void EatFood()
    {
        hasFood = false;
    }
}
