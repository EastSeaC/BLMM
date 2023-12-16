using BLMMServer.Modes.Skirmish;
using TaleWorlds.MountAndBlade;

namespace BLMMServer.Modes
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
            
        }

        public BLMMSiegeSpawningBehavior()
        {

        }
    }
}