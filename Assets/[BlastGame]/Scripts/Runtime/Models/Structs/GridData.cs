using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Runtime.Models
{
    [Serializable]
    public struct GridData
    {
        [Range(1, 10)]
        public int Width;
        [Range(1, 10)]
        public int Height;

        public GridTile GridTilePrefab;
    }
}