using Zenject;
using UnityEngine;
using ReupVirtualTwin.managers;
using ReupVirtualTwin.inputs;
using ReupVirtualTwin.controllerInterfaces;
using ReupVirtualTwin.controllers;

namespace ReupVirtualTwin.dependencyInjectors
{
    public class ReupDependenciesInstaller : MonoInstaller
    {
        public GameObject character;
        public GameObject innerCharacter;
        public DiContainer container;
        public override void InstallBindings()
        {
            container = Container;
            Container.BindInterfacesAndSelfTo<InputProvider>().AsSingle();
            Container.Bind<GameObject>().WithId("character").FromInstance(character);
            Container.Bind<GameObject>().WithId("innerCharacter").FromInstance(innerCharacter);
            Container.BindInterfacesAndSelfTo<CharacterPositionManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<CharacterRotationManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<DragManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<GesturesManager>().AsSingle();
            Container.Bind<ITagsController>().To<TagsController>().AsSingle();
            Container.BindInterfacesAndSelfTo<DhvNavigationManager>().AsSingle();
        }
    }
}
