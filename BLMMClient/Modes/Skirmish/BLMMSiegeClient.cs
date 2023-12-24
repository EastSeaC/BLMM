using NetworkMessages.FromServer;
using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace BLMMClient.Modes.Skirmish
{
    internal class BLMMSiegeClient : MissionMultiplayerGameModeBaseClient
    {
        public override bool IsGameModeUsingGold => true;

        public override bool IsGameModeTactical => true;

        public override bool IsGameModeUsingRoundCountdown => true;

        public override MultiplayerGameType GameType => MultiplayerGameType.FreeForAll;
        private BLMMSiegeMissionRepresentative _myRepresentative;
        public override int GetGoldAmount()
        {
            return 0;
        }


        private void OnMyClientSynchronized()
        {
            _myRepresentative = GameNetwork.MyPeer.GetComponent<BLMMSiegeMissionRepresentative>();
        }

        public override void OnRemoveBehavior()
        {
            MissionNetworkComponent.OnMyClientSynchronized -= OnMyClientSynchronized;
            base.OnRemoveBehavior();
        }

        public override void OnGoldAmountChangedForRepresentative(MissionRepresentativeBase representative, int goldAmount)
        {
            if (representative != null && MissionLobbyComponent.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Ending)
            {
                representative.UpdateGold(goldAmount);
                ScoreboardComponent.PlayerPropertiesChanged(representative.MissionPeer);
            }
        }

        protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
        {
            if (GameNetwork.IsClient)
            {
                registerer.RegisterBaseHandler<GoldGain>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(HandleServerEventTDMGoldGain));
            }
        }

        public override void OnBehaviorInitialize()
        {
            base.OnBehaviorInitialize();
            MissionNetworkComponent.OnMyClientSynchronized += OnMyClientSynchronized;

        }

        public override void AfterStart()
        {
            Mission.SetMissionMode(MissionMode.Battle, true);

        }



        private void HandleServerEventTDMGoldGain(GameNetworkMessage baseMessage)
        {
            if (OnGoldGainEvent == null) return;

            OnGoldGainEvent((GoldGain)baseMessage);
        }


        public event Action<GoldGain> OnGoldGainEvent;
    }
}