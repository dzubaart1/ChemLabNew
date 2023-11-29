using DefaultNamespace;
using LiquidVolumeFX;
using Substances;
using UnityEngine;

namespace Containers
{
    public class MixContainer : TransferSubstanceContainer
    {
        private AnchorCntrl _anchor;

        public AnchorCntrl AnchorCntrl => _anchor;
        public override bool AddSubstance(Substance substance)
        {
            Debug.Log($"---В {ContainerType}----");
            PrintAllSubstances();
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
            if (ContainerType == ContainersTypes.PetriContainer)
            {
                return true;
            }
            var _lv = _mainSubPrefab.GetComponentInChildren<LiquidVolume>();
            if (_lv != null)
            {
                var _w = GetNextSubstance().GetWeight() / MaxVolume;
                if (_w < 0.1f)
                    _w = 0.1f;
                _lv.level = _w;
                return true;
            }
            _mainSubPrefab.transform.localScale = new Vector3(1, GetNextSubstance().GetWeight() / MaxVolume, 1);
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
