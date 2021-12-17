using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Store : MonoBehaviour
{
    
    public static Store instance;
    private void Awake() { instance = this; }

    #region Coins
    int coins;
    public TextMeshProUGUI coins_value;
    public AudioClip clip_take_coin;
    public AudioClip clip_buy;
    public GameObject coin_model;
    public static string CoinsQuantString => instance.coins.ToString();
    public static int CoinsQuant => instance.coins;
    public static bool CanSpendCoins(int quant)
    {
        if (instance.coins >= quant) return true;
        else return false;
    }
    public static void SpendCoin(int quant)
    {
        instance.coins -= quant;
        if (instance.coins < 0) instance.coins = 0;
        instance.coins_value.text = instance.coins.ToString();
        instance.ui.Refresh();
        instance.PlayClip_BuySound();
    }
    public static void AddCoin(int quant)
    {
        instance.coins += quant;
        instance.coins_value.text = instance.coins.ToString();
        instance.ui.Refresh();
        instance.PlayClip_CoinTake();
    }
    public static void SpawnCoin(Vector3 position)
    {
        GameObject coin = Instantiate(instance.coin_model);
        coin.transform.position = position;
    }
    #endregion


    bool isOpen = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isOpen = !isOpen;
            ui.OpenStore(isOpen);
        }
    }

    public UI_Store ui;

    public BuyItem[] database;
    public Dictionary<int, ItemBuyBehaviour> items_registry = new Dictionary<int, ItemBuyBehaviour>();

    private void Start()
    {
        for (int i = 0; i < database.Length; i++)
        {
            var currentscriptable = database[i];
            var item = GameObject.Instantiate(currentscriptable.model_behaviour, this.transform);
            item.SetDataReference(currentscriptable);

            if (!items_registry.ContainsKey(i))
            {
                items_registry.Add(i,item);
            }
        }

        ui.CreateStore(items_registry, OnBuy);

        AudioManager.instance.GetSoundPool(clip_take_coin.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, clip_take_coin);
        AudioManager.instance.GetSoundPool(clip_buy.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, clip_buy);
    }
    void PlayClip_CoinTake() => AudioManager.instance.PlaySound(clip_take_coin.name, transform);
    void PlayClip_BuySound() => AudioManager.instance.PlaySound(clip_buy.name, transform);

    public void OnBuy(int id)
    {
        items_registry[id].Buy();
    }
}
