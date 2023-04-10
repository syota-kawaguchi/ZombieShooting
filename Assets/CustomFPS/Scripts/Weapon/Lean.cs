﻿/// DarkTreeDevelopment (2019) DarkTree FPS v1.21
/// If you have any questions feel free to write me at email --- darktreedevelopment@gmail.com ---
/// Thanks for purchasing my asset!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkTreeFPS
{

    public class Lean : MonoBehaviour
    {
        [Header("Lean Settings")]
        public float leanRotationSpeed = 80f;
        public float leanPositionSpeed = 3f;
        public float maxAngle = 30f;
        public float leanPositionShift = 0.1f;

        private float leanCurrentAngle = 0f;
        private float leanCurrentPosition;
        Quaternion leanRotation;

        Vector3 velocity = Vector3.zero;

        public float checkCollisionDistance = 0.1f;

        public Animator additionalWeaponsAnimator;

        private WeaponManager weaponManager;

        private void Start()
        {
            weaponManager = FindObjectOfType<WeaponManager>();
        }

        void Update()
        {
            if (Input.GetKey(InputManager.Instance.LeanLeft))
            {
                if (weaponManager.activeWeapon != null && !weaponManager.activeWeapon.setAim)
                {
                    additionalWeaponsAnimator.SetBool("LeanLeft", true);
                    additionalWeaponsAnimator.SetBool("LeanRight", false);
                }
                else
                {
                    additionalWeaponsAnimator.SetBool("LeanLeft", false);
                    additionalWeaponsAnimator.SetBool("LeanRight", false);
                }

                RaycastHit hit;

                if (!Physics.Raycast(transform.position, -transform.right, out hit, checkCollisionDistance)){
                    var temp_leanPositionShift = leanPositionShift;
                    leanCurrentAngle = Mathf.MoveTowardsAngle(leanCurrentAngle, maxAngle, leanRotationSpeed * Time.smoothDeltaTime);
                    transform.localPosition = Vector3.SmoothDamp(transform.localPosition, new Vector3(-temp_leanPositionShift, 0, 0), ref velocity, leanPositionSpeed * Time.smoothDeltaTime);
                }else
                {
                    var temp_leanPositionShift = Vector3.Distance(transform.position, hit.point)/1.5f;
                    leanCurrentAngle = Mathf.MoveTowardsAngle(leanCurrentAngle, maxAngle/3, leanRotationSpeed * Time.smoothDeltaTime);
                    transform.localPosition = Vector3.SmoothDamp(transform.localPosition, new Vector3(-temp_leanPositionShift, 0, 0), ref velocity, leanPositionSpeed * Time.smoothDeltaTime);
                }

            }
            else if (Input.GetKey(InputManager.Instance.LeanRight))
            {
                if (weaponManager.activeWeapon != null && !weaponManager.activeWeapon.setAim)
                {
                    additionalWeaponsAnimator.SetBool("LeanLeft", false);
                    additionalWeaponsAnimator.SetBool("LeanRight", true);
                }
                else 
                {
                    additionalWeaponsAnimator.SetBool("LeanLeft", false);
                    additionalWeaponsAnimator.SetBool("LeanRight", false);
                }

                RaycastHit hit;

                if (!Physics.Raycast(transform.position, transform.right, out hit, checkCollisionDistance))
                {
                    var temp_leanPositionShift = leanPositionShift;
                    leanCurrentAngle = Mathf.MoveTowardsAngle(leanCurrentAngle, -maxAngle, leanRotationSpeed * Time.smoothDeltaTime);
                    transform.localPosition = Vector3.SmoothDamp(transform.localPosition, new Vector3(temp_leanPositionShift, 0, 0), ref velocity, leanPositionSpeed * Time.smoothDeltaTime);
                }
                else
                {
                    var temp_leanPositionShift = Vector3.Distance(transform.position, hit.point) / 1.5f;
                    leanCurrentAngle = Mathf.MoveTowardsAngle(leanCurrentAngle, -maxAngle / 3, leanRotationSpeed * Time.smoothDeltaTime);
                    transform.localPosition = Vector3.SmoothDamp(transform.localPosition, new Vector3(temp_leanPositionShift, 0, 0), ref velocity, leanPositionSpeed * Time.smoothDeltaTime);
                }
            }
            else
            {
                additionalWeaponsAnimator.SetBool("LeanLeft", false);
                additionalWeaponsAnimator.SetBool("LeanRight", false);
                leanCurrentAngle = Mathf.MoveTowardsAngle(leanCurrentAngle, 0f, leanRotationSpeed * Time.deltaTime);
                transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref velocity, leanPositionSpeed * Time.smoothDeltaTime);
            }

            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, leanCurrentAngle));
            
        }
    }
}
