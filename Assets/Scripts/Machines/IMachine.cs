using OneRoomFactory.Factory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneRoomFactory.Machines
{
    public interface IMachine
    {
        Tile Output { get; }
    }
}

