using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Interface
{
    public interface IBlastable
    {
        bool CanBlast { get; }
        void Blast();
    }
}