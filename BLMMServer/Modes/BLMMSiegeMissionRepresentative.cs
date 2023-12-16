using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace BLMMServer.Modes
{
    internal class BLMMSiegeMissionRepresentative : MissionRepresentativeBase
    {
        private GoldGainFlags _currentGoldGains;
        private int _killCountOnSpawn;

        public int GetGoldAmountForVisual()
        {
            if (Gold < 0)
            {
                return 80;
            }
            return Gold;
        }

        public override void OnAgentSpawned()
        {
            _currentGoldGains = 0;
            _killCountOnSpawn = MissionPeer.KillCount;
        }
    }
}
