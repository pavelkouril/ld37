using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneRoomFactory.Managers
{
    public class MoneyManager : MonoBehaviour
    {
        public int StartingCash = 25000;
        public int Balance { get; private set; }

        private UIManager uiManager;

        private void Awake()
        {
            uiManager = GetComponent<UIManager>();
        }

        private void Start()
        {
            Balance = StartingCash;
            uiManager.UpdateBalance(Balance);
        }

        public bool CanPay(int cost)
        {
            return Balance >= cost;
        }

        public bool Pay(int cost)
        {
            if (CanPay(cost))
            {
                Balance -= cost;
                uiManager.UpdateBalance(Balance);
                return true;
            }
            return false;
        }

        public void Recieve(int cash)
        {
            Balance += cash;
            uiManager.UpdateBalance(Balance);
        }
    }
}