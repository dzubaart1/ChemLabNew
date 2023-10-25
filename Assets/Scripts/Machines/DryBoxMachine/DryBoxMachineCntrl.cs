using BNG;
using Containers;
using Installers;
using Substances;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Machines.DryBoxMachine
{
    public class DryBoxMachineCntrl : MonoBehaviour
    {
        [SerializeField] private SnapZone _snapZone;
        [SerializeField] private Image _startBtnImage;
        [SerializeField] private Sprite _onSprite, _offSprite;
        private bool _isEnter, _isStart;

        private SubstancesCntrl _substancesCntrl;
        private SignalBus _signalBus;
        [Inject]
        public void Construct(SignalBus signalBus, SubstancesCntrl substancesCntrl)
        {
            _signalBus = signalBus;
            _substancesCntrl = substancesCntrl;
            
            _signalBus.Subscribe<LoadSignal>(OnLoadScene);
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
            _signalBus.Fire(new MachineWorkSignal() { MachinesType = MachinesTypes.DryBoxMachine, SubstancePropertyBase = _snapZone.HeldItem?.gameObject.GetComponent<SubstanceContainer>().GetNextSubstance().SubstanceProperty});
        }
        
        public void OnToggleWork()
        {
            _isStart = !_isStart;
            
            if (_isStart)
            {
                _startBtnImage.sprite = _onSprite;
                _signalBus.Fire(new MachineWorkSignal() { MachinesType = MachinesTypes.DryBoxMachine, SubstancePropertyBase = _snapZone.HeldItem?.gameObject.GetComponent<SubstanceContainer>().GetNextSubstance().SubstanceProperty});
            }
            else
            {
                _startBtnImage.sprite = _offSprite;
                
                var temp = _snapZone.HeldItem.gameObject.GetComponent<SubstanceContainer>();
                var res = _substancesCntrl.DrySubstance(temp);
                if (!res)
                {
                    _substancesCntrl.SplitSubstances(temp);
                }
                
                _signalBus.Fire(new MachineWorkSignal() { MachinesType = MachinesTypes.DryBoxMachine, SubstancePropertyBase = temp.GetNextSubstance()?.SubstanceProperty});
            }
        }

        private void OnLoadScene()
        {
            if (_startBtnImage.sprite == _onSprite)
            {
                _isStart = true;
            }
            else
            {
                _isStart = false;
            }
        }
    }
}


