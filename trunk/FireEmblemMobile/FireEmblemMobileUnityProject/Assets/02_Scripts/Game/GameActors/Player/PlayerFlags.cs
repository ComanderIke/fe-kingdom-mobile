namespace Game.GameActors.Player
{
    public class PlayerFlags
    {
        public bool BoonBaneUnlocked { get; set; }
        public bool StrongestAttributeIncrease { get; set; }
        public bool GluttonyForceEat { get; set; }
        public bool SpecialUpgradeUnlocked { get; set; }
        public bool EventPreviewsUnlocked { get; set; }
        public bool StartingRelic { get; set; }
        public bool MoralityVisible { get; set; }
        public bool WeakestAttributeIncrease { get; set; }
        public bool RerollSkills { get; set; }
        public bool RerollLevelUps { get; set; }
        public bool RevivalStoneStart { get; set; }
        public bool PartyMemberAfterArea1 { get; set; }
        public bool PartyMemberAfterArea2 { get; set; }
        public bool PartyMemberAfterArea3 { get; set; }
        public bool PartyMemberAfterArea4 { get; set; }
        public bool PartyMemberAfterArea5 { get; set; }
        public bool PartyMemberAfterArea6 { get; set; }
        public bool StartingUpgrade { get; set; }
        public bool StartingSkill { get; set; }
        public bool SmithingBonds { get; set; }
        public bool EventBonds { get; set; }
        public bool InnBonds { get; set; }
        public bool InnSupplies { get; set; }
        public bool CombatSkills { get; set; }
        public bool LiquorBonds { get; set; }
        public bool KillBonds { get; set; }
        public bool BossKillBonds { get; set; }
        public bool Secret { get; set; }
        public bool LethalCritical { get; set; }

        public bool HasPartyMemberAfterArea(int partyAreaIndex)
        {
            switch (partyAreaIndex)
            {
                case 1:
                    return PartyMemberAfterArea1; break;
                case 2:
                    return PartyMemberAfterArea2; break;
                case 3:
                    return PartyMemberAfterArea3; break;
                case 4:
                    return PartyMemberAfterArea4; break;
                case 5:
                    return PartyMemberAfterArea5; break;
                case 6:
                    return PartyMemberAfterArea6; break;
                
            }

            return false;
        }
    }
}