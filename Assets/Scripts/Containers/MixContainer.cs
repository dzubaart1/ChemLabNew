using BNG;
using DefaultNamespace;
using Installers;
using LiquidVolumeFX;
using Substances;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Containers
{
    public class MixContainer : TransferSubstanceContainer
    {
        private AnchorCntrl _anchor;

        public AnchorCntrl AnchorCntrl => _anchor;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(List<Grabber> grabbers, SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        public override bool AddSubstance(Substance substance)
        {
            if (!IsEnable())
            {
                return false;
            }

            if (CurrentCountSubstances == 0)
            {
                _substancesCntrl.AddSubstance(this,substance);
            }
            else
            {
                _substancesCntrl.MixSubstances(this, substance);
            }

            var temp = GetWeight() / MaxVolume;

            if (temp < 0.1f)
            {
                temp = 0.3f;
            }

            if (ContainerType == ContainersTypes.PetriContainer) 
            {
                return true;
            }
            if (GetNextSubstance().SubstanceProperty.SubName == "Bad Substance")
            {
                _signalBus.Fire<EndGameSignal>();
                return true;
            }
            _liquidVolume.Redraw();
            _liquidVolume.level = temp;
            _mainSubPrefab.transform.localScale = new Vector3(1, temp, 1);

            return true;
        }

        public bool AddAnchor(AnchorCntrl anchorCntrl)
        {
            if (_anchor is not null)
            {
                return false;
            }
            
            _anchor = anchorCntrl;
            return true;
        }

        public override bool IsEnable()
        {
            if (_cupsList.Count > 0)
            {
                return _snapZone.HeldItem is null;
            }
            return true;
        }
    }
}
