using JetBrains.Annotations;

namespace Substances
{
    public class SubstanceSplit : SubstanceBase
    {
        public SubstanceSplit MembraneSubstance, MainSubstance, SedimentSubstance;
        public SubstanceSplit(SubstancePropertyBase substancePropertyBase, float weight)
            : base(substancePropertyBase, weight)
        {
            if (substancePropertyBase is not SubstancePropertySplit substanceProperty) return;
            if (substanceProperty.Sediment is not null)
            {
                SedimentSubstance = new SubstanceSplit(substanceProperty.Sediment,
                    substanceProperty.GetPartOfSedimentWeight() * Weight);
            }

            if (substanceProperty.Main is not null)
            {
                MainSubstance = new SubstanceSplit(substanceProperty.Main,
                    substanceProperty.GetPartOfMainWeight() * Weight);
            }

            if (substanceProperty.Membrane is not null)
            {
                MainSubstance = new SubstanceSplit(substanceProperty.Membrane,
                    substanceProperty.GetPartOfMembraneWeight() * Weight);
            }
        }

        [CanBeNull]
        public SubstanceSplit PopCurrentSubstance()
        {
            if (MembraneSubstance is not null)
            {
                MembraneSubstance = null;
                return MainSubstance ?? SedimentSubstance;
            }
            if (MainSubstance is not null)
            {
                MainSubstance = null;
                return SedimentSubstance;
            }
            if (SedimentSubstance is not null)
            {
                SedimentSubstance = null;
                return null;
            }

            return null;
        }
        
        [CanBeNull]
        public SubstanceBase GetCurrentSubstance()
        {
            if (MembraneSubstance is not null)
            {
                return MembraneSubstance;
            }
            if (MainSubstance is not null)
            {
                return MainSubstance;
            }
            if (SedimentSubstance is not null)
            {
                return SedimentSubstance;
            }

            return null;
        }
    }
}