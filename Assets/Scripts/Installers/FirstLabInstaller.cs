using System.Collections.Generic;
using System.Linq;
using BNG;
using Canvases;
using Data;
using Substances;
using Tasks;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class FirstLabInstaller : MonoInstaller
    {
        public GameObject XRRigAdvancedPrefab;
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<ShowCanvasSignal>();
            
            GameObject rigInst = Container.InstantiatePrefab(XRRigAdvancedPrefab);
            Container.Bind<GameObject>().FromInstance(rigInst).AsSingle();
            
            List<Grabber> grabbers = rigInst.GetComponentsInChildren<Grabber>().ToList();
            Container.Bind<List<Grabber>>().FromInstance(grabbers).AsSingle();

            SubstancesParamsCollection substancesCollection = new SubstancesParamsCollection();
            SubstancesCntrl substancesCntrl = new SubstancesCntrl(substancesCollection);
            Container.Bind<SubstancesCntrl>().FromInstance(substancesCntrl).AsSingle();

            TasksCntrl tasksCntrl = new TasksCntrl();
            Container.Bind<TasksCntrl>().FromInstance(tasksCntrl).AsSingle();
            
            SceneSetter sceneSetter = new SceneSetter(tasksCntrl);
            Container.Bind<SceneSetter>().FromInstance(sceneSetter).AsSingle();
        }
    }

    public class ShowCanvasSignal
    {
        public CanvasId Id;
    }
}
