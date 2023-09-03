using BlastGame.Runtime.Models;
using Core.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Interface
{
    public interface IBlastable
    {
        public CustomEvent OnBlasted { get; }
        bool CanBlast { get; }
        void Blast();
        void SetBlastableGroup(BlastableVisualType visualType);
    }
}