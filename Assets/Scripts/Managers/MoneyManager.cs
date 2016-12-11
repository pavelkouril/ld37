using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneRoomFactory.Managers
{
    public class MoneyManager : MonoBehaviour
    {
        public int StartingCash = 100;
        public int Balance { get; private set; }

        public bool CanPay(int cost)
        {
            return Balance >= cost;
        }

        public bool Pay(int cost)
        {
            if (CanPay(cost))
            {
                Balance -= cost;
                return true;
            }
            return false;
        }

        public void Recieve(int cash)
        {
            Balance += cash;
        }
    }
}