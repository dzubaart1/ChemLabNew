using BNG;
using Containers;
using Installers;
using Substances;
using UnityEngine;
using Zenject;

namespace Machines.DryBoxMachine
{
    public class DryBoxCntrl : MonoBehaviour
    {
        [SerializeField] private SnapZone _snapZone;
        private bool _isEnter;
        private bool _isStart;

        private SubstancesCntrl _substancesCntrl;
        private SignalBus _signalBus;
        [Inject]
        public void Construct(SignalBus signalBus, SubstancesCntrl substancesCntrl)
        {
            _signalBus = signalBus;
            _substancesCntrl = substancesCntrl;
        }
        
        private void Update()
        {
            if (_snapZone.HeldItem is null ||
                _snapZone.HeldItem.gameObject.GetComponent<MixContainer>() is null ||
                _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().ContainerType != ContainersTypes.PetriContainer)
            {
                _isEnter = false;
                return;
            }
            
            if (!_isEnter)
            {
                OnEnterObject();
            }
        }
        private void OnEnterObject()
        {
            _isEnter = true;
            var temp = _snapZone.HeldItem.gameObject.GetComponent<SubstanceContainer>();
            _substancesCntrl.DrySubstance(temp);
            _signalBus.Fire(new EnterIntoMachineSignal(){ContainersType = temp.ContainerType, MachinesType = MachinesTypes.DryBoxMachine});
        }
    }
}


