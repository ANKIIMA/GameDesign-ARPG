using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ACT.Move;

    public class AIMovement : BasicMovementModel
    {
        protected override void Update()
        {
            base.Update();

            UpdateGrvity();
        }


        private void UpdateGrvity()
        {
            verticalDirection.Set(0f,verticalSpeed,0f);
            characterController.Move(Time.deltaTime * verticalDirection);
        }
    }
