using UnityEngine;
using Zenject;

public class ProjectInstaler : MonoInstaller
{
    public override void InstallBindings()
    {
        BindInGameDataBase();
        BindInPutAction();
        BindMasterSave();

    }

    public override void Start()
    {
        Container.Resolve<InputSystem_Actions>().Enable();
    }

    private void BindInGameDataBase()
    {
        Container
                    .Bind<InGameDataBase>()
                    .FromNew()
                    .AsSingle()
                    .NonLazy();
    }

    private void BindInPutAction()
    {
        Container
                    .Bind<InputSystem_Actions>()
                    .FromNew()
                    .AsSingle()
                    .NonLazy();
    }

    private void BindMasterSave()
    {
        Container
                    .Bind<MasterSave>()
                    .FromNew()
                    .AsSingle()
                    .NonLazy();
    }
}