namespace Substances
{
    public class Substance
    {
        public SubstanceParams SubParams { get; }
        public float Weight { get; private set; }
        public Substance(SubstanceParams substanceParams, float weight)
        {
            SubParams = substanceParams;
            Weight = weight;
        }
        
        public void RemoveSubstanceWeight(float removeWeight)
        {
            Weight -= removeWeight;
        }
        
        public void AddSubstanceWeight(float addWeight)
        {
            Weight += addWeight;
        }
    }
}