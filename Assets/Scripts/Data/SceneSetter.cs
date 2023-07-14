using System.Collections.Generic;
using System.Linq;
using BNG;
using Containers;
using Installers;
using JetBrains.Annotations;
using Machines.DozatorMachine;
using Substances;
using UnityEngine;
using Zenject;

namespace Data
{
    public class SceneSetter : MonoBehaviour
    {
        [SerializeField] private DozatorMachineCanvas _dozatorObj;
        [SerializeField] private SnapZone _dozatorSnapZone;
        
        private List<GameObject> _objectsWithTransformAndSubstanceState;
        private List<GameObject> _objectsWithTransformState;
        private List<GameObject> _objectsWithSubstanceState;
        private List<GameObject> _animatorObjects;
        private List<GameObject> _snapZoneObjects;

        private SceneState _savedSceneState;
        
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<LoadSignal>(LoadState);
            _signalBus.Subscribe<SaveSignal>(SaveState);
        }
        private void Start()
        {
            _savedSceneState = new SceneState();
            
            _objectsWithTransformAndSubstanceState = GameObject.FindGameObjectsWithTag("SerializeTransferAndSubstance").ToList();
            _objectsWithTransformState = GameObject.FindGameObjectsWithTag("SerializeTransfer").ToList();
            _objectsWithSubstanceState = GameObject.FindGameObjectsWithTag("SerializeSubstance").ToList();
            _animatorObjects = GameObject.FindGameObjectsWithTag("Animator").ToList();
            _snapZoneObjects = GameObject.FindGameObjectsWithTag("SnapZone").ToList();
        }
        
        private void LoadState()
        {
            if (_savedSceneState is null)
            {
                return;
            }

            LoadObjectsWithTransferAndSubstanceState();
            LoadObjectsWithTransformState();
            LoadObjectsWithSubstanceState();
            
            LoadSnapZones();
            LoadAnimators();
            LoadDozator();

            _signalBus.Fire(new RevertTaskSignal(){TaskId = _savedSceneState.TaskId});
        }

        private void LoadTransformForObj(GameObject obj, TransformState transformState)
        {
            var grabComp = obj.GetComponent<Grabbable>();
            
            if (grabComp is not null)
            {
                if (grabComp.HeldByGrabbers is not null && grabComp.HeldByGrabbers.Count > 0)
                {
                    grabComp.DropItem(grabComp.HeldByGrabbers[0]);
                }
                grabComp.enabled = true;
            }
            
            obj.transform.rotation = transformState.Rotation;
            obj.transform.position = transformState.Position;
            obj.SetActive(transformState.IsActive);
        }

        private void LoadSubstanceForObj(GameObject obj, SubstanceState substanceState)
        {
            var subContOfObj = obj.GetComponent<SubstanceContainer>();
            if (subContOfObj is null)
            {
                return;
            }
                
            subContOfObj.ClearSubstances();
            foreach (var substance in substanceState.Substances)
            {
                if (substance is not null)
                {
                    subContOfObj.AddSubstanceToArray(new Substance(substance));
                }
            }
        }

        private void LoadObjectsWithTransferAndSubstanceState()
        {
            foreach (var saveObj in _savedSceneState.ObjectsWithTransformAndSubstanceState)
            {
                var obj = saveObj.Key;
                var state = saveObj.Value;

                LoadTransformForObj(obj, state.TransformState);
                LoadSubstanceForObj(obj, state.SubstanceState);
            }
        }

        private void LoadObjectsWithTransformState()
        {
            foreach (var saveObj in _savedSceneState.ObjectsWithTransformState)
            {
                var obj = saveObj.Key;
                var state = saveObj.Value;

                LoadTransformForObj(obj, state);
            }
        }

        private void LoadObjectsWithSubstanceState()
        {
            foreach (var saveObj in _savedSceneState.ObjectsWithSubstanceState)
            {
                var obj = saveObj.Key;
                var state = saveObj.Value;
                
                LoadSubstanceForObj(obj, state);
            }
        }
        
        private void LoadSnapZones()
        {
            foreach (var t in _snapZoneObjects)
            {
                t.GetComponent<SnapZone>().ReleaseAll();
                if (_savedSceneState.SnapZonesDictionary.TryGetValue(t, out GameObject value))
                {
                    t.GetComponent<SnapZone>().GrabGrabbable(value.GetComponent<Grabbable>());
                }
            }
        }

        private void LoadAnimators()
        {
            foreach (var t in _animatorObjects)
            {
                t.GetComponent<Animator>().enabled = false;
            }
        }

        private void LoadDozator()
        {
            _dozatorSnapZone.GrabGrabbable(_dozatorObj.GetComponent<Grabbable>());
            _dozatorObj.SetDoze(_savedSceneState.DozatorDoze);
        }

        private void SaveState(SaveSignal saveSignal)
        {
            _savedSceneState.TaskId = saveSignal.TaskId;
            _savedSceneState.DozatorDoze = _dozatorObj.GetDoze();

            SaveObjectsWithTransformAndSubstanceState();
            SaveObjectsWithTransformState();
            SaveObjectsWithSubstanceState();
            
            SaveSnapZones();
        }

        private void SaveObjectsWithTransformAndSubstanceState()
        {
            _savedSceneState.ObjectsWithTransformAndSubstanceState.Clear();

            foreach (var obj in _objectsWithTransformAndSubstanceState)
            {
                _savedSceneState.ObjectsWithTransformAndSubstanceState.Add(obj,GetTransformAndSubstanceStateByObj(obj));
            }
        }

        private void SaveObjectsWithTransformState()
        {
            _savedSceneState.ObjectsWithTransformState.Clear();

            foreach (var obj in _objectsWithTransformState)
            {
                _savedSceneState.ObjectsWithTransformState.Add(obj,GetTransformStateByObj(obj));
            }
        }
        
        private void SaveObjectsWithSubstanceState()
        {
            _savedSceneState.ObjectsWithSubstanceState.Clear();

            foreach (var obj in _objectsWithSubstanceState)
            {
                _savedSceneState.ObjectsWithSubstanceState.Add(obj,GetSubstanceState(obj));
            }
        }
        
        private void SaveSnapZones()
        {
            _savedSceneState.SnapZonesDictionary.Clear();
            foreach (var t in _snapZoneObjects)
            {
                if (t.GetComponent<SnapZone>().HeldItem is not null)
                {
                    _savedSceneState.SnapZonesDictionary.Add(t, t.GetComponent<SnapZone>().HeldItem?.gameObject);
                }
            }
        }

        private TransferAndSubstanceState GetTransformAndSubstanceStateByObj(GameObject obj)
        {
            return new TransferAndSubstanceState()
            {
                TransformState = GetTransformStateByObj(obj),
                SubstanceState = GetSubstanceState(obj)
            };
        }

        private TransformState GetTransformStateByObj(GameObject obj)
        {
            return new TransformState()
            {
                IsActive = obj.activeSelf,
                Position = obj.transform.position,
                Rotation = obj.transform.rotation
            };
        }

        private SubstanceState GetSubstanceState(GameObject obj)
        {
            var substanceContainerOfObj = obj.GetComponent<SubstanceContainer>();
            var substanceState = new SubstanceState();
            substanceState.Substances = new Substance[substanceContainerOfObj.CurrentSubstances.Length];
            
            for (var j = 0; j < substanceContainerOfObj.CurrentSubstances.Length; j++)
            {
                Substance substance = null;
                if (substanceContainerOfObj.CurrentSubstances[j] is not null)
                {
                    substance = new Substance(substanceContainerOfObj.CurrentSubstances[j]);
                }
                substanceState.Substances[j] = substance;
            }

            return substanceState;
        }
    }
    
    public class SceneState
    {
        public int TaskId;

        public float DozatorDoze;

        public SceneState()
        {
            ObjectsWithSubstanceState = new Dictionary<GameObject, SubstanceState>();
            ObjectsWithTransformState = new Dictionary<GameObject, TransformState>();
            ObjectsWithTransformAndSubstanceState = new Dictionary<GameObject, TransferAndSubstanceState>();
            SnapZonesDictionary = new Dictionary<GameObject, GameObject>();
        }
        
        public Dictionary<GameObject, TransferAndSubstanceState> ObjectsWithTransformAndSubstanceState { get; }
        public Dictionary<GameObject, SubstanceState> ObjectsWithSubstanceState{ get; }
        public Dictionary<GameObject, TransformState> ObjectsWithTransformState{ get; }
        public Dictionary<GameObject, GameObject> SnapZonesDictionary{ get; }
    }

    public struct TransferAndSubstanceState
    {
        public TransformState TransformState;
        public SubstanceState SubstanceState;
    }
    
    public struct SubstanceState
    {
        [ItemCanBeNull] public Substance[] Substances;
    }

    public struct TransformState
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public bool IsActive;
    }
}