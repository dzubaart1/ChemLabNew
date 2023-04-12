using Generators;
using UnityEngine;

namespace Cups
{
    public class BaseCup : MonoBehaviour
    {
        public int Id { get; private set; }
        public void Awake()
        {
            Id = IdGenerator.GetCupId();
            gameObject.name += Id;
        }
    }
}
