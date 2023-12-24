using BLMMClient.Modes.Warmup;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Multiplayer;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Source.Missions;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace BLMMClient.Modes.Skirmish;

[ViewCreatorModule]
internal class BLMMSkirmish : MissionBasedMultiplayerGameMode
{
    private const string GameName = "BLMM";
    public BLMMSkirmish() : base(GameName)
    {
    }

    [MissionMethod]
    public override void StartMultiplayerGame(string scene)
    {
        MissionState.OpenNew(GameName, new MissionInitializerRecord(scene),
            _ => new MissionBehavior[]
            {
                MissionLobbyComponent.CreateBehavior(),
                new BLMMSiegeClient(),
                new MultiplayerAchievementComponent(),
                new MultiplayerTimerComponent(),
                new MultiplayerMissionAgentVisualSpawnComponent(),
                new ConsoleMatchStartEndHandler(),
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
                MissionMatchHistoryComponent.CreateIfConditionsAreMet(),
                new EquipmentControllerLeaveLogic(),
                new MissionRecentPlayersComponent(),
                new VoiceChatHandler(),
                new MultiplayerPreloadHelper()
            });
    }


}
