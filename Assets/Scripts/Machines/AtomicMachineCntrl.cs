using UnityEngine;

namespace Machines
{
    public class AtomicMachineCntrl : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private GameObject _docPrefab;
        public void GenerateResults()
        {
            Instantiate(_docPrefab, _spawnPoint.position, Quaternion.identity);
        }
    }
}