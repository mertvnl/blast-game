using BlastGame.Runtime.Models;
using Core.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Interface
{
    public interface IBlastable
    {
        /// <summary>
        /// Event that is invoked after a blasting occured.
        /// </summary>
        public CustomEvent OnBlasted { get; }

        /// <summary>
        /// Property that checks if blastable can blast.
        /// </summary>
        bool CanBlast { get; }

        void Blast();

        /// <summary>
        /// Sets sprite renderer's sprite by given blastable visual type.
        /// </summary>
        /// <param name="visualType"></param>
        void SetBlastableGroup(BlastableVisualType visualType);
    }
}