﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneRoomFactory.Graphics
{
    public class BeltRollingAnimator : MonoBehaviour
    {
        private Renderer rend;

        void Start()
        {
            rend = GetComponent<Renderer>();
        }

        void FixedUpdate()
        {
            rend.material.SetTextureOffset("_MainTex", new Vector2(BeltRollingGlobalOffsetManager.offset, 0.0f));
        }
    }
}