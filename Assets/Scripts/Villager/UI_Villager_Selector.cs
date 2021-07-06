using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Villager_Selector : MonoBehaviour
{
    public static UI_Villager_Selector instance;
    public TextMeshProUGUI current_profession;
    public TMP_InputField inputname;
    private void Awake()
    {
        instance = this;

        group.alpha = 0;
        group.blocksRaycasts = false;
        group.interactable = false;
    }

    public CanvasGroup group;

    Villager currentVillager;

    public void OpenVillagerOptions(Villager current)
    {
        currentVillager = current;
        current_profession.text = current.GetProfession().ToString();
        inputname.text = current.VillagerName;
        group.alpha = 1;
        group.blocksRaycasts = true;
        group.interactable = true;
    }

    public void CloseVillagerOptions()
    {
        currentVillager = null;
        group.alpha = 0;
        group.blocksRaycasts = false;
        group.interactable = false;
    }

    public void BTN_Select_Be_Farmer() { if (currentVillager == null) return; currentVillager.ConfigureProfession(VillagerProfesion.farmer); UpdateProfesion(); }
    public void BTN_Select_Be_LumberJack() { if (currentVillager == null) return; currentVillager.ConfigureProfession(VillagerProfesion.lumberjack); UpdateProfesion(); }
    public void BTN_Select_Be_Miner() { if (currentVillager == null) return; currentVillager.ConfigureProfession(VillagerProfesion.miner);  UpdateProfesion(); }
    public void BTN_Select_Be_Warrior() { if (currentVillager == null) return; currentVillager.ConfigureProfession(VillagerProfesion.warrior);  UpdateProfesion(); }
    public void SetName(string name) { if (currentVillager == null) return; currentVillager.VillagerName = name; inputname.text = name; }

    void UpdateProfesion()
    {
        current_profession.text = currentVillager.GetProfession().ToString();
    }
}
