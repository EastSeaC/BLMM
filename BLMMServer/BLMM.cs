using BLMMServer.Modes;
using BLMMServer.Modes.Skirmish;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BLMMServer
{
    internal class SubModule : MBSubModuleBase
    {
        public const string ServerName = "BLMM";
        public SubModule() { }

        protected override void OnApplicationTick(float dt)
        {
            base.OnApplicationTick(dt);
        }

        protected override void InitializeGameStarter(Game game, IGameStarter starterObject)
        {
            base.InitializeGameStarter(game, starterObject);

        }
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            Module.CurrentModule.AddMultiplayerGameMode(new BLMMSkirmish());
        }
    }
}
