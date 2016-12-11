using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneRoomFactory.Graphics
{
    public class BeltRollingGlobalOffsetManager : MonoBehaviour
    {
        public static float offset = 0.0f;
        private static float offsetSpeed = 0.2f;

        void FixedUpdate()
        {
            offset -= offsetSpeed * Time.fixedDeltaTime;
        }
    }
}