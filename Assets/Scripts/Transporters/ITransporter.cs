﻿using OneRoomFactory.Factory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneRoomFactory.Transporters
{
    public interface ITransporter : IBuildable
    {
        Tile Output { get; }
        Movable ToMove { get; set; }
        TransporterType Type { get; }
    }
}