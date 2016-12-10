using OneRoomFactory.Factory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneRoomFactory.Transporters
{
    public interface ITransporter : IBuildable
    {
        Tile Input { get; }
        Tile Output { get; }
        Movable ToMove { get; set; }

        void Transport();
    }
}