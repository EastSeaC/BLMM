using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Objects;
using TaleWorlds.ObjectSystem;

namespace BLMMServer.Modes.Skirmish
{
    internal class BLMMSkirmishServer : MissionMultiplayerGameModeBase, IAnalyticsFlagInfo, IMissionBehavior
    {
        public MBReadOnlyList<FlagCapturePoint> AllCapturePoints { get; private set; }

        public override bool IsGameModeHidingAllAgentVisuals => true;

        public override bool IsGameModeUsingOpposingTeams => true;

        private Team[] _capturePointOwners;
        private MultiplayerWarmupComponent _warmupComponent;


        public Team GetFlagOwnerTeam(FlagCapturePoint flag)
        {
            return _capturePointOwners[flag.FlagIndex];
        }

        public override MultiplayerGameType GetMissionType() => MultiplayerGameType.FreeForAll;

        public override void AfterStart()
        {
            base.AfterStart();
            AddTeams();
            Debug.Print("[SC]");
            //_warmupComponent.OnWarmupEnding += OnWarmupEnding;
        }

        private void AddTeams()
        {
            BasicCultureObject cultureTeam1 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue());
            BasicCultureObject cultureTeam2 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue());
            Banner bannerTeam1 = new(cultureTeam1.BannerKey, cultureTeam1.BackgroundColor1, cultureTeam1.ForegroundColor1);
            Banner bannerTeam2 = new(cultureTeam2.BannerKey, cultureTeam2.BackgroundColor2, cultureTeam2.ForegroundColor2);
            Mission.Teams.Add(BattleSideEnum.Attacker, cultureTeam1.BackgroundColor1, cultureTeam1.ForegroundColor1, bannerTeam1, false, true);
            Mission.Teams.Add(BattleSideEnum.Defender, cultureTeam2.BackgroundColor2, cultureTeam2.ForegroundColor2, bannerTeam2, false, true);
        }

        private void OnWarmupEnding()
        {
            NotificationsComponent.WarmupEnding();
        }

 

        //public override bool CheckForWarmupEnd() //修正了一个Native错误，现在服务器可以正确比较当前玩家数与 MinNumberOfPlayersForMatchStart
        //{
        //    int num = 0;
        //    foreach (NetworkCommunicator networkPeer in GameNetwork.NetworkPeers)
        //    {
        //        MissionPeer component = networkPeer.GetComponent<MissionPeer>();
        //        if (networkPeer.IsSynchronized && component?.Team != null && component.Team.Side != BattleSideEnum.None)
        //        {
        //            num++;
        //        }
        //    }
        //    return num >= MultiplayerOptions.OptionType.MinNumberOfPlayersForMatchStart.GetIntValue();
        //}
    }
}