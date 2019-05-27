using Logging;
using Model;
using Model.Tree;
using System.Collections.Generic;

namespace Import
{
    public class TreeModelProvider
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string FQDN_SEPARATOR = ".";

        private EntityTree tree;

        public TreeModelProvider(FlatModelProvider flatModelProvider)
        {
            this.tree = new EntityTree();
            this.BuildTree(flatModelProvider.ProvideMetaData());
        }

        public EntityTree ProvideTree()
        {
            return this.tree;
        }

        private void BuildTree(Dictionary<ID, MetaData> metaData)
        {
            log.Debug("Building tree ...");
            foreach (KeyValuePair<ID, MetaData> entry in metaData)
            {
                string fqdn = entry.Value.qualifiedName;

                EntityNode node = this.CreateByFQDN(fqdn);
                node.Id = entry.Key;
                node.MetaData = entry.Value;
            }
            log.Debug("Finished building tree ...");
        }

        private EntityNode CreateByFQDN(string fqdn)
        {
            EntityNode ancestor = this.tree.Root;
            int separatorIndex = fqdn.IndexOf(FQDN_SEPARATOR);

            while (separatorIndex != -1)
            {
                string name = fqdn.Substring(0, separatorIndex);

                if (!ancestor.Descandents.ContainsKey(name))
                {
                    ancestor.AddDescandent(name, new EntityNode());
                }

                ancestor = ancestor.Descandents[name];


                fqdn = fqdn.Substring(separatorIndex + 1);
                separatorIndex = fqdn.IndexOf(FQDN_SEPARATOR);
            }

            if (ancestor.Descandents.ContainsKey(fqdn))
            {
                return ancestor.Descandents[fqdn];
            }
            else
            {
                EntityNode node = new EntityNode();
                ancestor.AddDescandent(fqdn, node);
                return node;
            }
        }
    }
}