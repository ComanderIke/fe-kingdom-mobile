using System;

namespace __2___Scripts.External.Editor.Data.Save
{
    [Serializable]
    public class NodeData
    {
        public string ID;
        public string Name;
        public object userData;

        public NodeData(string nextNodeID, string nextNodeName, int parse)
        {
            this.ID = nextNodeID;
            this.Name = nextNodeName;
            this.userData = parse;
        }
    }
}