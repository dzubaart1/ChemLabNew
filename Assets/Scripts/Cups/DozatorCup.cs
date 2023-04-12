using Interfaces;

namespace Cups
{
    public class DozatorCup : BaseCup, IAbleThrown
    {
        public bool IsDirty { set; get; }
    }
}