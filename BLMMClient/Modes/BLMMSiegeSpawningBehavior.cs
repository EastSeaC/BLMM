﻿using BLMMClient.Modes.Skirmish;
using NetworkMessages.FromServer;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace BLMMClient.Modes
{
    internal class BLMMSiegeSpawningBehavior : BLMMSpawningBehaviorBase
    {
        public override void Initialize(SpawnComponent spawnComponent)
        {
            base.Initialize(spawnComponent);
            OnAllAgentsFromPeerSpawnedFromVisuals += BLMMSiegeSpawningBehavior_OnAllAgentsFromPeerSpawnedFromVisuals;
        }

        private void BLMMSiegeSpawningBehavior_OnAllAgentsFromPeerSpawnedFromVisuals(MissionPeer obj)
        {
            bool flag = obj.Team == Mission.AttackerTeam;
        }

        public override void Clear()
        {
            base.Clear();
            OnAllAgentsFromPeerSpawnedFromVisuals -= BLMMSiegeSpawningBehavior_OnAllAgentsFromPeerSpawnedFromVisuals;

        }
        public override void OnTick(float dt)
        {
            if (IsSpawningEnabled && _spawnCheckTimer.Check(Mission.CurrentTime))
            {
                SpawnAgents();
                //SpawnBotAgents();
            }
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


        public BLMMSiegeSpawningBehavior()
        {
        }
    }
}