using UnityEngine;

namespace Substances
{
    public class SubstancePropertyBase : ScriptableObject
    {
        public string SubName;
        public Color Color;
        public SubstanceLayer SubstanceLayer;
        public string HintName;
    }
}