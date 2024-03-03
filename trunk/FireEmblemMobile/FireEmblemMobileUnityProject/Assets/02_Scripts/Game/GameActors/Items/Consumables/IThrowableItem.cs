namespace Game.GameActors.Items.Consumables
{
    interface IThrowableItem
    {
        public int Range { get; set; }
        public EffectType EffectType { get; set; } 
    }
}