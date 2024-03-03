namespace Game.SerializedData
{
    [System.Serializable]
    public class FactionData
    {
        // [SerializeField]
        // public FactionId factionId;
        // [SerializeField]
        // public string Name;
        // [SerializeField]
        // public List<PartyData> Parties;
        //
        // public FactionData(WM_Faction faction)
        // {
        //     factionId=faction.Id;
        //     Name = faction.Name;
        //     Parties = new List<PartyData>();
        //     foreach (var party in faction.Parties)
        //     {
        //         Parties.Add(new PartyData(party));
        //     }
        // }
        //
        // public void Load(WM_Faction faction)
        // {
        //     faction.Id = factionId;
        //     faction.Name = Name;
        //     faction.ClearParty();
        //     foreach (var partyData in Parties)
        //     {
        //         var party = ScriptableObject.CreateInstance<Party>();
        //         partyData.Load(party);
        //         party.Faction = faction;
        //         faction.Parties.Add(party);
        //     }
        // }
    }
}