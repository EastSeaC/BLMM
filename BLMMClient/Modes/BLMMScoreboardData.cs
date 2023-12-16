using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges;
using static TaleWorlds.MountAndBlade.MissionScoreboardComponent;
namespace BLMMClient.Modes
{
    internal class BLMMScoreboardData : IScoreboardData
    {
        public ScoreboardHeader[] GetScoreboardHeaders()
        {
            ScoreboardHeader[] array = new ScoreboardHeader[8];
            array[0] = new ScoreboardHeader("ping", (MissionPeer missionPeer) => Math.Round(missionPeer.GetNetworkPeer().AveragePingInMilliseconds).ToString(), (BotData bot) => "");
            array[1] = new ScoreboardHeader("avatar", (MissionPeer missionPeer) => "", (BotData bot) => "");
            array[2] = new ScoreboardHeader("badge", delegate (MissionPeer missionPeer)
                {
                    Badge byIndex = BadgeManager.GetByIndex(missionPeer.GetPeer().ChosenBadgeIndex);
                    if (byIndex != null)
                    {
                        return null;
                    }
                    return byIndex.StringId;
                }, (BotData bot) => "");
            array[3] = new ScoreboardHeader("name", (MissionPeer missionPeer) => missionPeer.DisplayedName, (BotData bot) => "Bot");
            array[4] = new ScoreboardHeader("kill", (MissionPeer missionPeer) => missionPeer.KillCount.ToString(), (BotData bot) => bot.KillCount.ToString());
            array[5] = new ScoreboardHeader("death", (MissionPeer peer) => peer.DeathCount.ToString(), (BotData bot) => bot.DeathCount.ToString());
            array[6] = new ScoreboardHeader("assist", (MissionPeer peer) => peer.AssistCount.ToString(), (BotData bot) => bot.AssistCount.ToString());
            //array[7] = new MissionScoreboardComponent.ScoreboardHeader("gold", (MissionPeer missionPeer) => missionPeer.GetComponent<CNMSiegeMissionRepresentative>().GetGoldAmountForVisual().ToString(), (BotData bot) => "");
            array[7] = new ScoreboardHeader("score", (MissionPeer peer)=>peer.Score.ToString(),(BotData bot)=>bot.Score.ToString());
            return array;
        }
    }
}