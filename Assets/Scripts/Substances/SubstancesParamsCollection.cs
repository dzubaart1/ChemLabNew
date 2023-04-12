using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Substances
{
    public class SubstancesParamsCollection
    {
        private List<SubstanceParams> _substanceParamsList;

        public SubstancesParamsCollection()
        {
            _substanceParamsList = new List<SubstanceParams>();
            _substanceParamsList = Resources.LoadAll<SubstanceParams>("Substances/").ToList();
        }

        private SubstanceParams GetBadSubstance() 
        {
            return _substanceParamsList[0];
        }
        public SubstanceParams GetMixSubstance(SubstanceParams oldParam, SubstanceParams addParam)
        {
            var res = _substanceParamsList.Find(substance =>
                substance.Components.Contains(oldParam) && substance.Components.Contains(addParam));

            return res ? res : GetBadSubstance();
        }
        
        public SubstanceParams GetStirringSubstance(SubstanceParams substance)
        {
            Debug.Log(substance.SubName);
            var res = _substanceParamsList.Find(temp =>
                temp.Components.Contains(substance) && temp.Components.Count == 1);
            
            return res ? res : GetBadSubstance();
        }
    }
}
