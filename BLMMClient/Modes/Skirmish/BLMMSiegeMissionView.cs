using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;

namespace BLMMClient.Modes.Skirmish
{
    [ViewCreatorModule]
    internal class BLMMSkirmishMissionView
    {
        [ViewMethod("BLMM")]
        public static MissionView[] OpenBLMMSiege(Mission mission)
        {
            return new MissionView[]
            {
                MultiplayerViewCreator.CreateMissionServerStatusUIHandler(),
                MultiplayerViewCreator.CreateMissionMultiplayerPreloadView(mission),
                new BLMMKillNotificationUIHandler(), //击杀反馈、金币反馈
                ViewCreator.CreateMissionAgentStatusUIHandler(mission),
                ViewCreator.CreateMissionMainAgentEquipmentController(mission),
                ViewCreator.CreateMissionMainAgentCheerBarkControllerView(mission),
                MultiplayerViewCreator.CreateMissionMultiplayerEscapeMenu("BLMM"),
                MultiplayerViewCreator.CreateMultiplayerEndOfBattleUIHandler(),
                ViewCreator.CreateMissionAgentLabelUIHandler(mission),
                MultiplayerViewCreator.CreateMultiplayerTeamSelectUIHandler(),
                MultiplayerViewCreator.CreateMissionScoreBoardUIHandler(mission, false),
                MultiplayerViewCreator.CreateMultiplayerEndOfRoundUIHandler(),
                MultiplayerViewCreator.CreateLobbyEquipmentUIHandler(),
                MultiplayerViewCreator.CreatePollProgressUIHandler(),
                MultiplayerViewCreator.CreateMultiplayerMissionDeathCardUIHandler(null),
                new MissionItemContourControllerView(),
                new MissionAgentContourControllerView(),
                MultiplayerViewCreator.CreateMissionFlagMarkerUIHandler(),
                ViewCreator.CreateOptionsUIHandler(),
                ViewCreator.CreateMissionMainAgentEquipDropView(mission),
                MultiplayerViewCreator.CreateMultiplayerAdminPanelUIHandler(), //管理UI
                ViewCreator.CreateMissionBoundaryCrossingView(),
                new MissionBoundaryWallView(),
                MultiplayerViewCreator.CreateMultiplayerMissionVoiceChatUIHandler(), //语音UI
            };
        }
    }
}
