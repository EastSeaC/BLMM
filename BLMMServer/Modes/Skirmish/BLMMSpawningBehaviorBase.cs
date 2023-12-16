using TaleWorlds.MountAndBlade;

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

        protected override void SpawnAgents()
        {

        }

        public override void Initialize(SpawnComponent spawnComponent)
        {
            base.Initialize(spawnComponent);
            SpawnComponent = spawnComponent;

        }
    }
}