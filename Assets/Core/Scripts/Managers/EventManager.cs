using Core.Systems;
using System;

namespace Core.Managers
{
    public static class EventManager
    {
        //Put your events here.

        public static readonly CustomEvent<int> IntegerEvent = new();
    }
}
