using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;

    public int amount = 0;

    public TextMeshProUGUI textToUpdate;
    
    public event EventHandler<int> OnMoneyChanged;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        MoneyChanged(this, amount); // dummy call, to make sure the correct amount is reflected in the ui
        OnMoneyChanged += MoneyChanged;
    }

    private void MoneyChanged(object sender, int e)
    {
        textToUpdate.text = ""+amount;
    }

    public void AddMoney(int _amount)
    {
        amount += _amount;
        
        OnMoneyChanged?.Invoke(this, amount);
    }

    // Why two methods ? Maybe we want a different SFX 
    public void RemoveMoney(int _amount)
    {
      
        amount -= _amount;
        
        OnMoneyChanged?.Invoke(this, amount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
