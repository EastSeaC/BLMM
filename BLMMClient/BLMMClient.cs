using BLMMClient.Modes.Skirmish;
using System;
using System.Collections.Generic;
using System.Text;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace BLMMClient
{
    internal class Submodule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            Module.CurrentModule.AddMultiplayerGameMode(new BLMMSkirmish());

        }

        protected override void OnApplicationTick(float dt)
        {
            base.OnApplicationTick(dt);
        }
    }
}
