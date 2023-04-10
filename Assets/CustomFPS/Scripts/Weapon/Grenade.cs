﻿/// DarkTreeDevelopment (2019) DarkTree FPS v1.21
/// If you have any questions feel free to write me at email --- darktreedevelopment@gmail.com ---
/// Thanks for purchasing my asset!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkTreeFPS {

    public class Grenade : MonoBehaviour
    {
        public float explosionTimer;
        public float explosionForce;
        public float damageRadius;
        public float damage;

        public GameObject explosionEffects;

        Collider[] colliders;
        GameObject effects_temp;

        void OnEnable()
        {
            if (explosionEffects != null)
            {
                effects_temp = Instantiate(explosionEffects);
                effects_temp.SetActive(false);
            }

            StartCoroutine(Timer(explosionTimer));
        }
        
        IEnumerator Timer(float explosionTimer)
        {
                yield return new WaitForSeconds(explosionTimer);
                print("Coroutine ended");
                Explosion();
        }

        void Explosion()
        {
            print("Explosion");

            colliders = Physics.OverlapSphere(transform.position, damageRadius);

            foreach (Collider collider in colliders)
            {
                if(collider.GetComponent<ObjectHealth>())
                {
                    collider.GetComponent<ObjectHealth>().health -= damage;
                }
               
                if(collider.GetComponent<PlayerStats>())
                {
                    collider.GetComponent<PlayerStats>().health -= (int)damage;
                }


                if (collider.GetComponent<Rigidbody>())
                {
                    collider.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, damageRadius);
                }
            }

            if (explosionEffects != null)
            {
                effects_temp.transform.position = transform.position;
                effects_temp.transform.rotation = transform.rotation;

                effects_temp.SetActive(true);
            }

            Destroy(gameObject);
        }
    }
}
