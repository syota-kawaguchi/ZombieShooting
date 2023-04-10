using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkTreeFPS
{
    public class CameraBob : MonoBehaviour
    {
        FPSController controller;
        Animator animator;
        InputManager manager;

        void Start()
        {
            controller = FindObjectOfType<FPSController>();
            animator = GetComponent<Animator>();
            manager = FindObjectOfType<InputManager>();
        }

        // Update is called once per frame
        void Update()
        {
            animator.SetBool("isMoving", CheckMovement());
        }

        public bool CheckMovement()
        {
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                return true;
            }

            return false;
        }
    }
}
