using System.Collections.Generic;
using System.Linq;
using BNG;
using Canvases;
using Containers;
using Data;
using Machines;
using Substances;
using Tasks;
using Unity.XR.CoreUtils;
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
            Container.DeclareSignal<CheckTasksSignal>();
            Container.DeclareSignal<TransferSubstanceSignal>();
            Container.DeclareSignal<StartMachineWorkSignal>();
            Container.DeclareSignal<FinishMashineWorkSignal>();
            Container.DeclareSignal<EnterIntoMachineSignal>();

            GameObject rigInst = Container.InstantiatePrefab(XRRigAdvancedPrefab);
            rigInst.GetComponentsInChildren<BNGPlayerController>()[0].transform.position = new Vector3(-7, 5, 0f );    
            rigInst.GetComponentsInChildren<BNGPlayerController>()[0].transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
            Container.Bind<GameObject>().FromInstance(rigInst).AsSingle();
            
            List<Grabber> grabbers = rigInst.GetComponentsInChildren<Grabber>().ToList();
            Container.Bind<List<Grabber>>().FromInstance(grabbers).AsSingle();

            SubstancesParamsCollection substancesCollection = new SubstancesParamsCollection();
            SubstancesCntrl substancesCntrl = new SubstancesCntrl(substancesCollection);
            Container.Bind<SubstancesCntrl>().FromInstance(substancesCntrl).AsSingle();

            /*SceneSetter sceneSetter = new SceneSetter();
            Container.Bind<SceneSetter>().FromInstance(sceneSetter).AsSingle();*/
        }
    }

    public class ShowCanvasSignal
    {
        public CanvasId Id;
    }
    public class CheckTasksSignal
    {
        public TaskParams CurrentTask;
    }
    public class TransferSubstanceSignal
    {
        public ContainersTypes From;
        public ContainersTypes To;
        public SubstancePropertyBase TranserProperty;
    }
    public class StartMachineWorkSignal
    {
        public MachinesTypes MachinesType;
    }
    public class FinishMashineWorkSignal
    {
        public MachinesTypes MachinesType;
        public SubstancePropertyBase SubstancePropertyBase;
    }
    public class EnterIntoMachineSignal
    {
        public MachinesTypes MachinesType;
        public ContainersTypes ContainersType;
    }
}
