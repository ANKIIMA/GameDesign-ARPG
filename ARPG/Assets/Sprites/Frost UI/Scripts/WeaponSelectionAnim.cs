// THIS SCRIPT IS MADE FOR DEMO SCENE AND MAY NOT BE USEFUL IN YOUR PROJECT.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Michsky.UI.Frost
{
    public class WeaponSelectionAnim : MonoBehaviour
    {
        public Animator weaponListAnimator;
        public List<GameObject> weaponButtons = new List<GameObject>();

        private GameObject currentWeapon;
        private GameObject selectedWeapon;

        private GameObject currentButton;
        private GameObject nextButton;

        public int currentButtonIndex = 0;
        private int nextButtonIndex = 0;

        private Animator currentWeaponAnimator;

        private Animator currentButtonAnimator;
        private Animator nextButtonAnimator;

        void Start()
        {
            nextButton = weaponButtons[nextButtonIndex];
            nextButtonAnimator = nextButton.GetComponent<Animator>();
            nextButtonAnimator.Play("Equip On");
        }

        void Update()
        {
            if (Input.GetKeyDown("1"))
            {
                currentButton = weaponButtons[currentButtonIndex];
                nextButtonIndex = 0;
                nextButton = weaponButtons[nextButtonIndex];

                currentButtonAnimator = currentButton.GetComponent<Animator>();
                currentButtonAnimator.Play("Equip Off");

                nextButtonAnimator = nextButton.GetComponent<Animator>();
                nextButtonAnimator.Play("Equip On");
                currentButtonIndex = 0;

                if (weaponListAnimator.GetCurrentAnimatorStateInfo(0).IsName("Weapon List Out"))
                {
                    weaponListAnimator.Play("Weapon List In");
                }
            }

            else if (Input.GetKeyDown("2"))
            {
                currentButton = weaponButtons[currentButtonIndex];
                nextButtonIndex = 1;
                nextButton = weaponButtons[nextButtonIndex];

                currentButtonAnimator = currentButton.GetComponent<Animator>();
                currentButtonAnimator.Play("Equip Off");

                nextButtonAnimator = nextButton.GetComponent<Animator>();
                nextButtonAnimator.Play("Equip On");
                currentButtonIndex = 1;

                if (weaponListAnimator.GetCurrentAnimatorStateInfo(0).IsName("Weapon List Out"))
                {
                    weaponListAnimator.Play("Weapon List In");
                }
            }

            else if (Input.GetKeyDown("3"))
            {
                currentButton = weaponButtons[currentButtonIndex];
                nextButtonIndex = 2;
                nextButton = weaponButtons[nextButtonIndex];

                currentButtonAnimator = currentButton.GetComponent<Animator>();
                currentButtonAnimator.Play("Equip Off");

                nextButtonAnimator = nextButton.GetComponent<Animator>();
                nextButtonAnimator.Play("Equip On");
                currentButtonIndex = 2;

                if (weaponListAnimator.GetCurrentAnimatorStateInfo(0).IsName("Weapon List Out"))
                {
                    weaponListAnimator.Play("Weapon List In");
                }
            }

            else if (Input.GetKeyDown("4"))
            {
                currentButton = weaponButtons[currentButtonIndex];
                nextButtonIndex = 3;
                nextButton = weaponButtons[nextButtonIndex];

                currentButtonAnimator = currentButton.GetComponent<Animator>();
                currentButtonAnimator.Play("Equip Off");

                nextButtonAnimator = nextButton.GetComponent<Animator>();
                nextButtonAnimator.Play("Equip On");
                currentButtonIndex = 3;

                if (weaponListAnimator.GetCurrentAnimatorStateInfo(0).IsName("Weapon List Out"))
                {
                    weaponListAnimator.Play("Weapon List In");
                }
            }
        }
    }
}