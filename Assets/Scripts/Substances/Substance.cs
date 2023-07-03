namespace Substances
{
    public class Substance
    {
        public SubstancePropertyBase SubstanceProperty { get; private set; }
        protected float Weight;
        public Substance(SubstancePropertyBase substanceProperty, float weight)
        {
            Weight = weight;
            SubstanceProperty = substanceProperty;
        }

        public Substance(Substance substance)
        {
            Weight = substance.GetWeight();
            SubstanceProperty = substance.SubstanceProperty;
        }
        
        public float GetWeight()
        {
            return Weight;
        }

        public void RemoveWeight(float weight)
        {
            Weight -= weight;
        }

        public void AddWeight(float weight)
        {
            Weight += weight;
        }
    }
}