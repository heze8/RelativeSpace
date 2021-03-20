using UnityEngine;

namespace BattleCore
{
    public class Team
    {
        public int teamIndex;

        public Team(int team)
        {
            this.teamIndex = team;
        }

        public bool IsEnemy(IBattleOccupant battleOccupant)
        {
            BattleUnit attempt = (BattleUnit) battleOccupant;
            
            if (attempt != null)
            {
                //Debug.Log("comparing team " + teamIndex + " " + attempt.team.teamIndex);
                return teamIndex != attempt.team.teamIndex;
            }

            return false;
        }
    }
}