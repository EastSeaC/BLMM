using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLMMServer.Modes.Warmup;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Multiplayer;
using TaleWorlds.MountAndBlade.Source.Missions;

namespace BLMMServer.Modes.Skirmish
{
    [ViewCreatorModule]
    internal class BLMMSkirmish : MissionBasedMultiplayerGameMode
    {
        private const string GameName = "BLMM";
        public BLMMSkirmish() : base(GameName)
        {
        }

        public override void StartMultiplayerGame(string scene)
        {
            BLMMWarmupComponent warmupComponent = new(() => (new SiegeSpawnFrameBehavior(), new BLMMSiegeSpawningBehavior()));
            MissionState.OpenNew(GameName, new MissionInitializerRecord(scene), _ => new MissionBehavior[]
            {
                MissionLobbyComponent.CreateBehavior(),
                new BLMMSkirmishServer(),
                //warmupComponent,
                new BLMMSiegeClient(),
                new MultiplayerTimerComponent(),
                new SpawnComponent(new SiegeSpawnFrameBehavior(), new BLMMSiegeSpawningBehavior()),
                new MissionLobbyEquipmentNetworkComponent(),
                new MultiplayerTeamSelectComponent(),
                new MissionHardBorderPlacer(),
                new MissionBoundaryPlacer(),
                new MissionBoundaryCrossingHandler(),
                new MultiplayerPollComponent(),
                new MultiplayerAdminComponent(),
                new MultiplayerGameNotificationsComponent(),
                new MissionOptionsComponent(),
                new MissionScoreboardComponent(new BLMMScoreboardData()),
                new MissionAgentPanicHandler(),
                new AgentHumanAILogic(),
                new EquipmentControllerLeaveLogic(),
                new VoiceChatHandler(),
                new MultiplayerPreloadHelper()
            });
        }
    }
}
