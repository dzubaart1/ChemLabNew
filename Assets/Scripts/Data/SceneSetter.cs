using System.Collections.Generic;
using System.Linq;
using BNG;
using Containers;
using Installers;
using JetBrains.Annotations;
using Substances;
using UnityEngine;
using Zenject;

namespace Data
{
    public class SceneSetter : MonoBehaviour
    {
        private List<GameObject> _listOfObjects;
        private List<GameObject> _animatorObjects;
        private List<GameObject> _snapZoneObjects;
        private GameObject _expTablet;
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
            _listOfObjects = GameObject.FindGameObjectsWithTag("Serializable").ToList();
            _animatorObjects = GameObject.FindGameObjectsWithTag("Animator").ToList();
            _snapZoneObjects = GameObject.FindGameObjectsWithTag("SnapZone").ToList();
            _expTablet = GameObject.FindGameObjectWithTag("ExpTablet");
            
            _savedSceneState = new SceneState();
            _savedSceneState.SaveObjectsList = new List<SavedObjectState>();
        }
        
        public void LoadState()
        {
            _expTablet.transform.position = _savedSceneState.ExpTabletTransform.Position;
            _expTablet.transform.rotation = _savedSceneState.ExpTabletTransform.Rotation;
            foreach (var t in _snapZoneObjects)
            {
                t.GetComponent<SnapZone>().ReleaseAll();
                if (_savedSceneState.SaveSnapPoint.TryGetValue(t, out GameObject value))
                {
                    t.GetComponent<SnapZone>().GrabGrabbable(value.GetComponent<Grabbable>());
                }
            }
            foreach (var t in _animatorObjects)
            {
                t.GetComponent<Animator>().enabled = false;
            }
            foreach (var t in _savedSceneState.SaveObjectsList)
            {
                var objInList = _listOfObjects[t.IndexInList];
                var subCont = objInList.GetComponent<SubstanceContainer>();
                var grab = objInList.GetComponent<Grabbable>();
                
                if (grab is not null)
                {
                    if (grab.HeldByGrabbers is not null && grab.HeldByGrabbers.Count > 0)
                    {
                        grab.DropItem(grab.HeldByGrabbers[0]);
                    }
                    grab.enabled = true;
                }
                
                if (subCont is not null)
                {
                    subCont.ClearSubstances();
                    foreach (var substance in t.Substances)
                    {
                        if (substance is not null)
                        {
                            subCont.AddSubstanceToArray(new Substance(substance));
                        }
                    }
                }

                objInList.transform.position = t.Transform.Position;
                objInList.transform.rotation = t.Transform.Rotation;
                objInList.SetActive(t.IsActive);
            }
            
            _signalBus.Fire(new RevertTaskSignal(){TaskId = _savedSceneState.TaskId});
        }

        private void SaveState(SaveSignal saveSignal)
        {
            _savedSceneState.SaveObjectsList.Clear();
            _savedSceneState.TaskId = saveSignal.TaskId;
            _savedSceneState.ExpTabletTransform = new SavedTransform()
            {
                Position = _expTablet.transform.position,
                Rotation = _expTablet.transform.rotation
            };
            for(var i = 0; i < _listOfObjects.Count; i++)
            {
                var savedObjectState = new SavedObjectState()
                {
                    IndexInList = i,
                    Transform = new SavedTransform()
                    {
                        Position = _listOfObjects[i].transform.position,
                        Rotation = _listOfObjects[i].transform.rotation
                    }
                };
                savedObjectState.Substances = new Substance[_listOfObjects[i].GetComponent<SubstanceContainer>().CurrentSubstances.Length];
                savedObjectState.IsActive = _listOfObjects[i].activeSelf;

                for (int j = 0; j < _listOfObjects[i].GetComponent<SubstanceContainer>().CurrentSubstances.Length; j++)
                {
                    Substance substance = null;
                    if (_listOfObjects[i].GetComponent<SubstanceContainer>().CurrentSubstances[j] is not null)
                    {
                        substance = new Substance(_listOfObjects[i].GetComponent<SubstanceContainer>().CurrentSubstances[j]);
                    }
                    savedObjectState.Substances[j] = substance;
                }
                _savedSceneState.SaveObjectsList.Add(savedObjectState);
            }

            _savedSceneState.SaveSnapPoint = new Dictionary<GameObject, GameObject>();
            foreach (var t in _snapZoneObjects)
            {
                if (t.GetComponent<SnapZone>().HeldItem is not null)
                {
                    _savedSceneState.SaveSnapPoint.Add(t, t.GetComponent<SnapZone>().HeldItem?.gameObject);
                }
            }
        }
    }
    
    public struct SceneState
    {
        public int TaskId;
        public List<SavedObjectState> SaveObjectsList;
        public Dictionary<GameObject, GameObject> SaveSnapPoint;
        public SavedTransform ExpTabletTransform;
    }

    public struct SavedObjectState
    {
        public int IndexInList;
        public SavedTransform Transform;
        [ItemCanBeNull] public Substance[] Substances;
        public bool IsActive;
    }

    public struct SavedTransform
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }
}