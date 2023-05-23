using System.Collections.Generic;
using UnityEngine;

namespace Substances
{
    [CreateAssetMenu(fileName = "SubstanceProperty", menuName = "Substance Properties/SubstancePropertyComponents", order = 1)]

    public class SubstancePropertyComponents : SubstancePropertyBase
    {
        public List<SubstancePropertyBase> SubstancePropertyBases;
    }
}