using System.Collections.Generic;
using System.Linq;
using Containers;
using Substances;
using Tasks;
using UnityEngine;

namespace Data
{
    public class SceneSetter
    {
        private List<GameObject> _listOfObjects;
        private SceneState _savedSceneState;
        private TasksCntrl _tasksCntrl;

        public SceneSetter()
        {
            _listOfObjects = GameObject.FindGameObjectsWithTag("Serializable").ToList();
            _savedSceneState = new SceneState();
            SaveSceneState();
        }
        
        public void SaveSceneState()
        {
            _savedSceneState.SaveObjectsList = GetSaveContentListFromGameObjectList(_listOfObjects);
            _savedSceneState.TaskId = _tasksCntrl.CurrentTask().Id;
        }

        public void GetSavedSceneState()
        {
            _tasksCntrl.SetCurrentTaskId(_savedSceneState.TaskId);
            for (var i = 0; i < _savedSceneState.SaveObjectsList.Count; i++)
            {
                var indexInList = _savedSceneState.SaveObjectsList[i].IndexInList;
                _listOfObjects[indexInList].transform.position = _savedSceneState.SaveObjectsList[i].Position.GetVector();
                _listOfObjects[indexInList].transform.rotation = _savedSceneState.SaveObjectsList[i].Quaternion.GetQuaternion();
                /*if (_listOfObjects[indexInList].GetComponent<DisplaySubstance>() is not null)
                {
                    _listOfObjects[indexInList].GetComponent<DisplaySubstance>().CurrentSubstance = (_savedSceneState.SaveObjectsList[i].Substance);
                    _listOfObjects[indexInList].GetComponent<DisplaySubstance>().UpdateDisplaySubstance();
                }*/
            }
        }

        private List<SaveObject> GetSaveContentListFromGameObjectList(List<GameObject> list)
        {
            var res = new List<SaveObject>();
            for(var i = 0; i < _listOfObjects.Count; i++)
            {
                var currentVecPos = _listOfObjects[i].GetComponent<Transform>().position;
                var currentVecRot = _listOfObjects[i].GetComponent<Transform>().rotation;
                var saveContent = new SaveObject(){IndexInList = i,
                    Position = new FVector(currentVecPos),
                    Quaternion = new FQuaternion(currentVecRot)};
                /*if (_listOfObjects[i].GetComponent<BaseContainer>() != null)
                {
                    saveContent.Substance = _listOfObjects[i].GetComponent<BaseContainer>().CurrentSubstance;
                }*/
                res.Add(saveContent);
            }

            return res;
        }
    }
    
    public struct SceneState
    {
        public int TaskId;
        public List<SaveObject> SaveObjectsList;
    }
    
    public struct SaveObject
    {
        public int IndexInList { get; set; }
        public FVector Position { get; set; }
        public FQuaternion Quaternion { get; set; }
        //public SubstanceSplit Substance { get; set; }
    }
    
    public struct FVector
    {
        public float X, Y, Z;

        public FVector(Vector3 vector)
        {
            X = vector.x;
            Y = vector.y;
            Z = vector.z;
        }

        public Vector3 GetVector()
        {
            return new Vector3(X, Y, Z);
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }

    public struct FQuaternion
    {
        public float X, Y, Z, W;

        public FQuaternion(Quaternion quaternion)
        {
            X = quaternion.x;
            Y = quaternion.y;
            Z = quaternion.z;
            W = quaternion.w;
        }
        
        public Quaternion GetQuaternion()
        {
            return new Quaternion(X, Y, Z, W);
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z}, {W})";
        }
    }
}