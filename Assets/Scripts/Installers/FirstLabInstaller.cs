using System.Collections.Generic;
using System.Linq;
using BNG;
using Generators;
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
            GameObject oculusRigInst = Container.InstantiatePrefab(xrRigAdvanced);

            List<Grabber> grabbers = oculusRigInst.GetComponentsInChildren<Grabber>().ToList();
            Container.Bind<List<Grabber>>().FromInstance(grabbers).AsSingle();
            
            SubstancesParamsCollection substancesCollection = new SubstancesParamsCollection();
            Container.Bind<SubstancesParamsCollection>().FromInstance(substancesCollection).AsSingle();
            
            TasksCntrl tasksCntrl = new TasksCntrl();
            Container.Bind<TasksCntrl>().FromInstance(tasksCntrl).AsSingle();
        }
    }
}
