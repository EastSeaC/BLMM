using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;
using Timer = TaleWorlds.Core.Timer;
namespace BLMMServer.Modes.Skirmish
{
    internal class BLMMSpawningBehaviorBase : SpawningBehaviorBase
    {
        public override bool AllowEarlyAgentVisualsDespawning(MissionPeer missionPeer)
        {
            return true;
        }

        protected override bool IsRoundInProgress()
        {
            return true;
        }
        public override void OnTick(float dt)
        {
            base.OnTick(dt);
        }
        protected override void SpawnAgents()
        {
            BasicCultureObject cultureTeam1 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue());
            BasicCultureObject cultureTeam2 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue());
            foreach (NetworkCommunicator networkPeer in GameNetwork.NetworkPeers)
            {
                if (!networkPeer.IsSynchronized)
                {
                    continue;
                }

                MissionPeer component = networkPeer.GetComponent<MissionPeer>();
                if (component == null || component.ControlledAgent != null || component.HasSpawnedAgentVisuals || component.Team == null || component.Team == Mission.SpectatorTeam || !component.TeamInitialPerkInfoReady || !component.SpawnTimer.Check(Mission.CurrentTime))
                {
                    continue;
                }

                BasicCultureObject basicCultureObject = component.Team.Side == BattleSideEnum.Attacker ? cultureTeam1 : cultureTeam2;
                MultiplayerClassDivisions.MPHeroClass mPHeroClassForPeer = MultiplayerClassDivisions.GetMPHeroClassForPeer(component);
                if (mPHeroClassForPeer == null || mPHeroClassForPeer.TroopCasualCost > GameMode.GetCurrentGoldForPeer(component) || LockTroop(component, mPHeroClassForPeer))
                {
                    if (component.SelectedTroopIndex != 0)
                    {
                        component.SelectedTroopIndex = 0;
                        GameNetwork.BeginBroadcastModuleEvent();
                        GameNetwork.WriteMessage(new UpdateSelectedTroopIndex(networkPeer, 0));
                        GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.ExcludeOtherTeamPlayers, networkPeer);
                    }
                    continue;
                }

                BasicCharacterObject heroCharacter = mPHeroClassForPeer.HeroCharacter;
                Equipment equipment = heroCharacter.Equipment.Clone();
                IEnumerable<(EquipmentIndex, EquipmentElement)> enumerable = MPPerkObject.GetOnSpawnPerkHandler(component)?.GetAlternativeEquipments(isPlayer: true);
                if (enumerable != null)
                {
                    foreach (var item in enumerable)
                    {
                        equipment[item.Item1] = item.Item2;
                    }
                }

                AgentBuildData agentBuildData = new AgentBuildData(heroCharacter).MissionPeer(component).Equipment(equipment).Team(component.Team)
                    .TroopOrigin(new BasicBattleAgentOrigin(heroCharacter))
                    .IsFemale(component.Peer.IsFemale)
                    .BodyProperties(GetBodyProperties(component, component.Team == Mission.AttackerTeam ? cultureTeam1 : cultureTeam2))
                    .VisualsIndex(0)
                    .ClothingColor1(component.Team == Mission.AttackerTeam ? basicCultureObject.Color : basicCultureObject.ClothAlternativeColor)
                    .ClothingColor2(component.Team == Mission.AttackerTeam ? basicCultureObject.Color2 : basicCultureObject.ClothAlternativeColor2);
                if (GameMode.ShouldSpawnVisualsForServer(networkPeer) && agentBuildData.AgentVisualsIndex == 0)
                {
                    component.HasSpawnedAgentVisuals = true;
                    component.EquipmentUpdatingExpired = false;
                }

                GameMode.HandleAgentVisualSpawning(networkPeer, agentBuildData);
            }
        }

        public static bool LockTroop(MissionPeer component, MultiplayerClassDivisions.MPHeroClass mpheroClassForPeer) //锁定兵种比例上限
        {
            double archerPer = 0.25; //射狗比例
            double cavalryPer = 0.25; //骑兵比例
            double horseArcherPer = 0.25; //骑射比例
            bool flag = false;
            int Sum = GetTroopTypeCountForTeam(component.Team)[0];
            //int Infantry = GetTroopTypeCountForTeam(component.Team)[1];
            int Ranged = GetTroopTypeCountForTeam(component.Team)[2];
            int Cavalry = GetTroopTypeCountForTeam(component.Team)[3];
            int HorseArcher = GetTroopTypeCountForTeam(component.Team)[4];
            BasicCharacterObject Character = mpheroClassForPeer.TroopCharacter;
            if (Character.IsRanged && !Character.IsMounted && Ranged > Sum * archerPer || Character.IsMounted && !Character.IsRanged && Cavalry > Sum * cavalryPer || Character.IsMounted && Character.IsRanged && HorseArcher > Sum * horseArcherPer)
                flag = true;
            return flag;
        }


        public static int[] GetTroopTypeCountForTeam(Team team) //统计某方战场存活兵种数，0总数；1步兵；2射手；3骑兵；4骑射
        {
            int[] num = new int[5];
            foreach (NetworkCommunicator networkPeer in GameNetwork.NetworkPeers)
            {
                MissionPeer component = networkPeer.GetComponent<MissionPeer>();
                if (component?.Team != null && component.Team == team && component.IsControlledAgentActive)
                {
                    BasicCharacterObject Character = MultiplayerClassDivisions.GetMPHeroClassForPeer(component).HeroCharacter;
                    num[0]++;
                    if (Character.IsInfantry)
                    {
                        num[1]++;
                        continue;
                    }
                    if (Character.IsRanged && !Character.IsMounted)
                        num[2]++;
                    if (Character.IsMounted && !Character.IsRanged)
                        num[3]++;
                    if (Character.IsRanged && Character.IsMounted)
                        num[4]++;
                }
            }
            return num;
        }

        public override void Initialize(SpawnComponent spawnComponent)
        {
            base.Initialize(spawnComponent);
            SpawnComponent = spawnComponent;

            _spawnCheckTimer = new Timer(Mission.Current.CurrentTime, 0.2f, true);

        }
    }
}