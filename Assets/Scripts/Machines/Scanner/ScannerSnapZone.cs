using UnityEngine;
using BNG;
using Containers;

public class ScannerSnapZone : MonoBehaviour
{
    private SnapZone _snapZone;
    void Start()
    {
        _snapZone = GetComponent<SnapZone>();
    }

    public void OnAttach()
    {
        if (_snapZone.HeldItem.gameObject.GetComponent<MixContainer>() is null || _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().ContainerType == ContainersTypes.PetriContainer)
            return;
        _snapZone.HeldItem.gameObject.transform.Rotate(0,24.57f,90, Space.Self);
        _snapZone.HeldItem.gameObject.transform.localPosition = new Vector3(-0.016f, 0, 0);
    }
    /*public void OnDetach()
    {
        if (_snapZone.HeldItem.gameObject.GetComponent<MixContainer>() is null || _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().ContainerType == ContainersTypes.PetriContainer)
            return;
        _snapZone.HeldItem.gameObject.GetComponent<SnapZoneOffset>().LocalRotationOffset = new Vector3(0, 0, -90);
        _snapZone.HeldItem.gameObject.GetComponent<SnapZoneOffset>().LocalPositionOffset = new Vector3(-0.04f, 0, 0);
    }*/
}
