﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    private void OnEnable()
    {
        //gameObject.layer = LayerMask.NameToLayer("Weapon");
        //Invoke("ChangeLayer", 0.13f);
    }

    void ChangeLayer()
    {
        //gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
