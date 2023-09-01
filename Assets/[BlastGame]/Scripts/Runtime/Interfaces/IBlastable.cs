using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Interface
{
    public interface IBlastable
    {
        void Blast();
        void Notify(IItem item);
    }
}