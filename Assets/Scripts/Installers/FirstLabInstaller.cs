using System.Collections.Generic;
using System.Linq;
using BNG;
using Containers;
using Machines;
using Substances;
using Tasks;
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
            Container.DeclareSignal<EndGameSignal>();
            Container.DeclareSignal<CheckTasksSignal>();
            Container.DeclareSignal<TransferSubstanceSignal>();
            Container.DeclareSignal<MachineWorkSignal>();
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
    public class EndGameSignal
    {
        
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
    public class MachineWorkSignal
    {
        public MachinesTypes MachinesType;
        public SubstancePropertyBase SubstancePropertyBase;
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
