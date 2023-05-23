using System.Collections.Generic;
using System.Linq;
using BNG;
using Data;
using Substances;
using Tasks;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class FirstLabInstaller : MonoInstaller
    {
        public GameObject xrRigAdvanced;
        public override void InstallBindings()
        {
            TasksCntrl tasksCntrl = new TasksCntrl();
            Container.Bind<TasksCntrl>().FromInstance(tasksCntrl).AsSingle();
            
            GameObject rigInst = Container.InstantiatePrefab(xrRigAdvanced);
            Container.Bind<GameObject>().FromInstance(rigInst).AsSingle();
            
            List<Grabber> grabbers = rigInst.GetComponentsInChildren<Grabber>().ToList();
            Container.Bind<List<Grabber>>().FromInstance(grabbers).AsSingle();
            
            SceneSetter sceneSetter = new SceneSetter(tasksCntrl);
            Container.Bind<SceneSetter>().FromInstance(sceneSetter).AsSingle();
            
            SubstancesParamsCollection substancesCollection = new SubstancesParamsCollection();
            SubstancesCntrl substancesCntrl = new SubstancesCntrl(substancesCollection);
            Container.Bind<SubstancesCntrl>().FromInstance(substancesCntrl).AsSingle();
        }
    }
}
