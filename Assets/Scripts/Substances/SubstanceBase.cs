namespace Substances
{
    public class SubstanceBase
    {
        public SubstancePropertyBase SubstanceProperty { get; private set; }
        protected float Weight;
        public SubstanceBase(SubstancePropertyBase substanceProperty, float weight)
        {
            Weight = weight;
            SubstanceProperty = substanceProperty;
        }
        
        public float GetWeight()
        {
            return Weight;
        }

        public void RemoveWeight(float weight)
        {
            Weight -= weight;
        }
    }
}