using System.Collections.Generic;
using System.Linq;
using BNG;
using Containers;
using Installers;
using Substances;
using Tasks;
using UnityEngine;
using Zenject;

namespace Data
{
    public class SceneSetter : MonoBehaviour
    {
        private List<GameObject> _listOfObjects;
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
            
            _savedSceneState = new SceneState();
            _savedSceneState.SaveObjectsList = new List<SavedObjectState>();
        }
        
        public void LoadState()
        {
            foreach (var t in _savedSceneState.SaveObjectsList)
            {

                var objInList = _listOfObjects[t.IndexInList];

                objInList.transform.position = t.Position;
                objInList.transform.rotation = t.Rotation;
                    
                var subCont = objInList.GetComponent<SubstanceContainer>();
                var grab = objInList.GetComponent<Grabbable>();
                
                if (subCont is not null)
                {
                    subCont.ClearSubstances();
                    foreach (var substance in t.Substances)
                    {
                        if (substance is not null)
                        {
                            subCont.AddSubstanceToArray(substance);
                        }
                    }
                }

                if (grab is not null)
                {
                    grab.enabled = true;
                }
            }
            _signalBus.Fire(new RevertTaskSignal(){TaskId = _savedSceneState.TaskId});
        }

        private void SaveState(SaveSignal saveSignal)
        {
            _savedSceneState.SaveObjectsList.Clear();
            _savedSceneState.TaskId = saveSignal.TaskId;
            for(var i = 0; i < _listOfObjects.Count; i++)
            {
                var savedObjectState = new SavedObjectState()
                {
                    IndexInList = i,
                    Position = _listOfObjects[i].transform.position,
                    Rotation = _listOfObjects[i].transform.rotation
                };
                savedObjectState.Substances = new Substance[3];
                _listOfObjects[i].GetComponent<SubstanceContainer>().CurrentSubstances.CopyTo(savedObjectState.Substances,0);
                _savedSceneState.SaveObjectsList.Add(savedObjectState);
            }
        }
    }
    
    public struct SceneState
    {
        public int TaskId;
        public List<SavedObjectState> SaveObjectsList;
    }

    public struct SavedObjectState
    {
        public int IndexInList;
        public Vector3 Position;
        public Quaternion Rotation;
        public Substance[] Substances;
    }
}