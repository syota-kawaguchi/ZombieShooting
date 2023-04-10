using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DarkTreeFPS;
using DTInventory;

public class ActionWait : MonoBehaviour
{
    [HideInInspector]
    public float actionTimer;
    [HideInInspector]
    public string actionText;

    [HideInInspector]
    public AudioSource audioSource;

    private WeaponManager weaponManager;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        weaponManager = FindObjectOfType<WeaponManager>();
    }

    void Update()
    {
        actionTimer -= Time.deltaTime;

        if (actionTimer >= 0)
        {

            if (actionTimer > 0)
            {
                float val = (int)actionTimer+1;
                print(actionText + " :" + val.ToString());
            }

            weaponManager.HideWeapon();
            FPSController.canMove = false;
            InventoryManager.showInventory = false;
        }
        else
        {
            if (!FPSController.canMove == false)
            {
                FPSController.canMove = true;
            }
        }
    }
}
