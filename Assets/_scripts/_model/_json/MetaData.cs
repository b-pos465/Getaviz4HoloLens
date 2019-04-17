using System;

namespace Model
{
    [Serializable]
    public class MetaData
    {
        public string id;
        public string qualifiedName;
        public string name;
        public string type;
        public string modifiers;
        public string signature;
        public string calls;
        public string calledBy;
        public string accesses;
        public string belongsTo;
        public string declaredType;
        public string accessedBy;
        public string subClassOf;
        public string superClassOf;
    }
}
