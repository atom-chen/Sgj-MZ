using System;
using UnityEngine;

namespace App.Controller.Common
{
    public interface IController
    {

        App.View.Common.Dispatcher Dispatcher { get; }

        void Reload(Request request = null, bool pushHistory = true);
        YieldInstruction Load(Request request = null, bool pushHistory = true);
        YieldInstruction LoadAnimation();
        YieldInstruction Unload();
        YieldInstruction UnloadAnimation();
    }
}
