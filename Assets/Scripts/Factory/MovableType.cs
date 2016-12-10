using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneRoomFactory.Factory
{
    public enum MovableType
    {
        // supplies
        Cuprexit,
        Acid,
        Electronics,

        // intermediates
        CircuitBoardCopper,
        CircuitBoardPrinted,
        CircuitBoardCleaned,
        CircuitBoardDrilled,

        CompletedPCB,
    }
}
