using UnityEngine;
using UnityEngine.InputSystem;

namespace BNG {
    public class ToggleActiveOnInputAction : MonoBehaviour {

        public InputActionReference InputAction;
        public GameObject ToggleObject;

        private void OnEnable() {
            InputAction.action.performed += ToggleActive;
        }

        private void OnDisable() {
            InputAction.action.performed -= ToggleActive;
        }

        private void ToggleActive(InputAction.CallbackContext context)
        {
            if(!ToggleObject)
            {
                return;
            }
            ToggleObject.SetActive(!ToggleObject.activeSelf);
        }
    }
}

