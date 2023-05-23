using UnityEngine;

namespace Substances
{
    [CreateAssetMenu(fileName = "SubstanceProperty", menuName = "Substance Properties/SubstancePropertySplit", order = 1)]
    public class SubstancePropertySplit : SubstancePropertyBase
    {
        private int _membranWeightPart = 1;
        private int _mainWeightPart = 8; 
        private int _sedimentWeightPart = 1; 
        
        public SubstancePropertyBase Sediment;
        public SubstancePropertyBase Main;
        public SubstancePropertyBase Membrane;

        public float GetPartOfMembraneWeight()
        {
            return _membranWeightPart / (_membranWeightPart + _mainWeightPart + _sedimentWeightPart);
        }
        
        public float GetPartOfMainWeight()
        {
            return _mainWeightPart / (_membranWeightPart + _mainWeightPart + _sedimentWeightPart);
        }
        
        public float GetPartOfSedimentWeight()
        {
            return _sedimentWeightPart / (_membranWeightPart + _mainWeightPart + _sedimentWeightPart);
        }
    }
}