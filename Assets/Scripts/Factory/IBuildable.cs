using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneRoomFactory.Factory
{
    public interface IBuildable
    {
        Tile Tile { get; set; }
        BuildRotation Rotation { get; set; }
    }
}