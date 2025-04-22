using System;
using UnityEngine;

namespace Tower
{
    public class TowerManager : MonoBehaviour
    {
        int towerLevel = 0;
        [SerializeField] private GameObject[] towerLevels;

        public int Resource1;
        public int Resource2;
        public int Resource3;
        [SerializeField] private int[] resource1Required;
        [SerializeField] private int[] resource2Required;
        [SerializeField] private int[] resource3Required;

        private void Update()
        {
            if (towerLevel == towerLevels.Length) return;

            if (Resource1 < resource1Required[towerLevel + 1]) return;
            if  (Resource2 < resource2Required[towerLevel + 1]) return;
            if (Resource3 < resource3Required[towerLevel + 1]) return;
            
            towerLevel++;
            towerLevels[towerLevel].SetActive(true);
        }
    }
}