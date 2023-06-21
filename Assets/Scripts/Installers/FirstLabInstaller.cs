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
        public Transform SpawnPoint;
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<ToggleCanvasSignal>();
            Container.DeclareSignal<CheckTasksSignal>();
            Container.DeclareSignal<TransferSubstanceSignal>();
            Container.DeclareSignal<StartMachineWorkSignal>();
            Container.DeclareSignal<FinishMashineWorkSignal>();
            Container.DeclareSignal<EnterIntoMachineSignal>();
            Container.DeclareSignal<StartGameSignal>();
            Container.DeclareSignal<SaveSignal>();
            Container.DeclareSignal<LoadSignal>();
            Container.DeclareSignal<RevertTaskSignal>();

            GameObject rigInst = Container.InstantiatePrefab(XRRigAdvancedPrefab);
            rigInst.transform.position = SpawnPoint.transform.position;
            Container.Bind<GameObject>().FromInstance(rigInst).AsSingle();
            
            List<Grabber> grabbers = rigInst.GetComponentsInChildren<Grabber>().ToList();
            Container.Bind<List<Grabber>>().FromInstance(grabbers).AsSingle();

            SubstancesParamsCollection substancesCollection = new SubstancesParamsCollection();
            SubstancesCntrl substancesCntrl = new SubstancesCntrl(substancesCollection);
            Container.Bind<SubstancesCntrl>().FromInstance(substancesCntrl).AsSingle();
            
        }
    }

    public class StartGameSignal
    {
    }
    public class ToggleCanvasSignal
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

    public class SaveSignal
    {
        public int TaskId;
    }

    public class LoadSignal
    {
    }

    public class RevertTaskSignal
    {
        public int TaskId;
    }
}
