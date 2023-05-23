using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Substances
{
    public class SubstancesParamsCollection
    {
        private List<SubstancePropertyComponents> _substancePropertyComponents;
        private List<SubstancePropertyDry> _substancePropertyDries;
        private List<SubstancePropertySplit> _substancePropertySplits;
        private List<SubstancePropertyStirring> _substancePropertyStirrings;
        private List<SubstancePropertyBase> _substancePropertyBases;
        public SubstancesParamsCollection()
        {
            _substancePropertyComponents = new List<SubstancePropertyComponents>();
            _substancePropertyDries = new List<SubstancePropertyDry>();
            _substancePropertySplits = new List<SubstancePropertySplit>();
            _substancePropertyStirrings = new List<SubstancePropertyStirring>();
            _substancePropertyBases = new List<SubstancePropertyBase>();

            Upload();
        }

        private void Upload()
        {
            _substancePropertyComponents = Resources.LoadAll<SubstancePropertyComponents>("SubstancesProperties/SubstancePropertyComponents").ToList();
            _substancePropertyDries = Resources.LoadAll<SubstancePropertyDry>("SubstancesProperties/SubstancePropertyDry").ToList();
            _substancePropertySplits = Resources.LoadAll<SubstancePropertySplit>("SubstancesProperties/SubstancePropertySplit").ToList();
            _substancePropertyStirrings = Resources.LoadAll<SubstancePropertyStirring>("SubstancesProperties/SubstancePropertyStirring").ToList();
            _substancePropertyBases = Resources.LoadAll<SubstancePropertyBase>("SubstancesProperties/SubstancePropertyBase").ToList();
        }
        private SubstancePropertyBase getBadSubstanceParams() 
        {
            return _substancePropertyBases.Find(sub => sub.SubName.Equals("Bad Substance"));
        }
        public SubstancePropertyBase GetMixSubstanceParams(SubstancePropertyBase oldParam, SubstancePropertyBase addParam)
        {
            var res = _substancePropertyComponents.Find(substance =>
                substance.SubstancePropertyBases.Contains(oldParam) && substance.SubstancePropertyBases.Contains(addParam));

            return res ? res : getBadSubstanceParams();
        }
        public SubstancePropertyBase GetStirringSubstanceParams(SubstancePropertyBase substance)
        {
            var res = _substancePropertyStirrings.Find(temp =>
                temp.SubName.Equals(substance.SubName));
            
            return res ? res : getBadSubstanceParams();
        }
        public SubstancePropertyBase GetDrySubstanceParams(SubstancePropertyBase substance)
        {
            var res = _substancePropertyDries.Find(temp =>
                temp.SubName.Equals(substance.SubName));
            
            return res ? res : getBadSubstanceParams();
        }
        public SubstancePropertyBase GetSplitSubstanceParams(SubstancePropertyBase substance)
        {
            var res = _substancePropertySplits.Find(temp =>
                temp.SubName.Equals(substance.SubName));
            
            return res ? res : getBadSubstanceParams();
        }
    }
}
