using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class Entity: MonoBehaviour
    {
        public ID id;
        public string qualifiedName;
        public new string name;
        public string type;
        public List<string> modifiers;
        public string signature;
        public GameObject calls;
        public GameObject calledBy;
        public GameObject accesses;
        public GameObject belongsTo;

        public bool IsClass()
        {
            return this.type == "FAMIX.Class";
        }

        public bool IsPackage()
        {
            return this.type != "FAMIX.Class";
        }
    }
}
