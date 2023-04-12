using System.Collections.Generic;
using UnityEngine;

namespace Substances
{
    [CreateAssetMenu(fileName = "SubstanceParams", menuName = "Substance/Substance Params", order = 1)]
    public class SubstanceParams : ScriptableObject
    {
        public string SubName;
        public Color Color;
        public List<SubstanceParams> Components;
    }
}