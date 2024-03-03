using Game.SerializedData;

namespace Game.EncounterAreas.Serialization
{
    public abstract class SerializedAction
    {
        protected SerializedAction()
        {
            //Debug.Log("Constructor is called!");
           // NotifySystem();
        }

        // private void NotifySystem()
        // {
        //     SerializedActionSystem.Instance.Add(this);
        // }

        public abstract void PerformAction();
        public abstract void Save(SaveData current);
       
    }
}