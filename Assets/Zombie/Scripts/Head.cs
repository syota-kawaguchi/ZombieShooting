using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    [SerializeField]
    private Zombie zombie;
    // Start is called before the first frame update
    public void OnHeadShot(int Damage) {
        Debug.Log("Head Shot !!!");
        zombie.HeadShot(Damage);
    }
}
