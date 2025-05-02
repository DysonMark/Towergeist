using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dyson.Towergeist
{
    public class WhichAgent : MonoBehaviour
    {
        public bool isCementAgent;
        public bool isStoneAgent;
        public bool isWoodAgent;

        private void Awake()
        {
            if (gameObject.name == "CementAgent")
            {
                isCementAgent = true;
            }
            else if (gameObject.name == "StoneAgent")
            {
                isStoneAgent = true;
            }
            else if (gameObject.name == "WoodAgent")
            {
                isWoodAgent = true;
            }
        }
    }
}
