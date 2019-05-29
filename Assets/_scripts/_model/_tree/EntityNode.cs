using System.Collections.Generic;

namespace Model.Tree
{
    public class EntityNode
    {
        public string Name { get; private set; }
        public ID Id { get; set; }
        public MetaData MetaData { get; set; }
        public Dictionary<string, EntityNode> Descandents { get; private set; }
        public EntityNode Ancestor { get; set; }

        public EntityNode()
        {
            this.Descandents = new Dictionary<string, EntityNode>();
        }

        public void AddDescandent(string name, EntityNode node)
        {
            node.Ancestor = this;
            node.Name = name;
            this.Descandents.Add(name, node);
        }

        public bool IsLeaf()
        {
            return this.Descandents.Count == 0;
        }

        public bool IsRoot()
        {
            return this.Ancestor == null;
        }
    }
}
