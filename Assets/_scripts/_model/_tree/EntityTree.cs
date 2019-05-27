namespace Model.Tree
{
    public class EntityTree
    {
        public EntityNode Root { get; private set; }

        public EntityTree()
        {
            this.Root = new EntityNode();
        }
    }
}
