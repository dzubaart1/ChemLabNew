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

            List<Grabber> grabbers = rigInst.GetComponentsInChildren<Grabber>().ToList();
            Container.Bind<List<Grabber>>().FromInstance(grabbers).AsSingle();
            
            SubstancesParamsCollection substancesCollection = new SubstancesParamsCollection();
            Container.Bind<SubstancesParamsCollection>().FromInstance(substancesCollection).AsSingle();
            
            SceneSetter sceneSetter = new SceneSetter(tasksCntrl);
            Container.Bind<SceneSetter>().FromInstance(sceneSetter).AsSingle();
        }
    }
}
