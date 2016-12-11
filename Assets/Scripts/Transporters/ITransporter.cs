using OneRoomFactory.Factory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneRoomFactory.Transporters
{
    abstract public class ITransporter : Buildable
    {
        public Tile Output { get; set; }
        public Movable ToMove { get; set; }
        abstract public TransporterType Type { get; }
    }
}