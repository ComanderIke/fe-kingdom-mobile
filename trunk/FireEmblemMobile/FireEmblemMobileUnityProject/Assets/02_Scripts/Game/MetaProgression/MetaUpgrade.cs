public class MetaUpgrade
{
    public MetaUpgradeBP blueprint;
    public int level;
    public bool locked = false;
   

    public MetaUpgrade(MetaUpgradeBP bluePrint)
    {
        this.blueprint = bluePrint;
        level = 0;
        
    }

    public bool IsMaxed()
    {
        return level == blueprint.maxLevel;
    }

}