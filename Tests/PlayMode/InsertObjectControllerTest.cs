using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using System.Threading.Tasks;

using ReupVirtualTwin.controllerInterfaces;
using ReupVirtualTwin.controllers;
using ReupVirtualTwin.dataModels;
using ReupVirtualTwin.enums;
using ReupVirtualTwin.managerInterfaces;
using ReupVirtualTwin.webRequestersInterfaces;
using System.Collections;

namespace ReupVirtualTwinTests.controllers
{
    public class InsertObjectControllerTest: MonoBehaviour
    {
        GameObject ObjectRegistryPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Packages/com.reup.romulo/Assets/ScriptHolders/ObjectRegistry.prefab");
        GameObject objectRegistryGameObject;
        MediatorSpy mediatorSpy;
        MeshDownloaderSpy meshDownloaderSpy;
        InsertObjectMessagePayload insertObjectMessagePayload;
        InserObjectController controller;
        ITagsController tagsReader;
        IIdGetterController idReader;
        Vector3 insertPosition;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // we set the objectRegistry only once because some objects that depend on it are using the ObjectFinder class to find it
            // if we create a different objectRegistry for each test in the SetUp method, the ObjectFinder sometimes finds
            // an old objectRegistry why this happens is still unknown to me
            objectRegistryGameObject = (GameObject)PrefabUtility.InstantiatePrefab(ObjectRegistryPrefab);
        }

        private void RequesObject(InsertObjectMessagePayload message)
        {
            mediatorSpy.IncreaseObjectRequestedCount();
            controller.InsertObject(message);
        }
        private bool CheckMeshesCollider(GameObject obj)
        {
            MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
            Collider collider = obj.GetComponent<Collider>();
            if (meshFilter != null && meshFilter.sharedMesh != null && collider == null)
            {
                return false;
            }
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                if (!CheckMeshesCollider(obj.transform.GetChild(i).gameObject))
                {
                    return false;
                }
            }
            return true;
        }

        [SetUp]
        public void SetUp()
        {
            mediatorSpy = new MediatorSpy();
            meshDownloaderSpy = new MeshDownloaderSpy();
            insertObjectMessagePayload = new InsertObjectMessagePayload()
            {
                objectId = "object-id",
                objectUrl = "object-url",
                selectObjectAfterInsertion = true,
                deselectPreviousSelection = true,
            };
            insertPosition = new Vector3(1, 2, 3);
            controller = new InserObjectController(mediatorSpy, meshDownloaderSpy, insertPosition);
            RequesObject(insertObjectMessagePayload);
            tagsReader = new TagsController();
            idReader = new IdController();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Destroy(objectRegistryGameObject);
        }

        [UnityTest]
        public IEnumerator ShouldCreateInsertObjectController()
        {
            yield return new WaitUntil(() => mediatorSpy.allRequestedObjectsAreLoaded);
            Assert.IsNotNull(controller);
            yield return null;
        }

        [UnityTest]
        public IEnumerator ShouldRequestMeshDownload()
        {
            yield return new WaitUntil(() => mediatorSpy.allRequestedObjectsAreLoaded);
            Assert.AreEqual(1, meshDownloaderSpy.numberOfCalls);
            yield return null;
        }

        [UnityTest]
        public IEnumerator ShouldNotifyProgressToMediator()
        {
            yield return new WaitUntil(() => mediatorSpy.allRequestedObjectsAreLoaded);
            Assert.AreEqual(4, mediatorSpy.onProgressNumberOfCalls);
            yield return null;
        }

        [UnityTest]
        public IEnumerator ShouldNotifyWhenObjectLoadsToMediator()
        {
            yield return new WaitUntil(() => mediatorSpy.allRequestedObjectsAreLoaded);
            Assert.AreEqual(meshDownloaderSpy.GetLastLoadedObject(), mediatorSpy.GetLastLoadedObject());
            Assert.IsTrue(mediatorSpy.GetLastLoadedObject().activeInHierarchy);
            yield return null;
        }

        [UnityTest]
        public IEnumerator InsertedObjectShouldHaveSelectableTag()
        {
            yield return new WaitUntil(() => mediatorSpy.allRequestedObjectsAreLoaded);
            Assert.IsTrue(tagsReader.DoesObjectHaveTag(mediatorSpy.GetLastLoadedObject(), ObjectTag.SELECTABLE));
            yield return null;
        }

        [UnityTest]
        public IEnumerator InsertedObjectShouldHaveTransformableTag()
        {
            yield return new WaitUntil(() => mediatorSpy.allRequestedObjectsAreLoaded);
            Assert.IsTrue(tagsReader.DoesObjectHaveTag(mediatorSpy.GetLastLoadedObject(), ObjectTag.TRANSFORMABLE));
            yield return null;
        }

        [UnityTest]
        public IEnumerator InsertedObjectShouldHaveDeletableTag()
        {
            yield return new WaitUntil(() => mediatorSpy.allRequestedObjectsAreLoaded);
            Assert.IsTrue(tagsReader.DoesObjectHaveTag(mediatorSpy.GetLastLoadedObject(), ObjectTag.DELETABLE));
            yield return null;
        }

        [UnityTest]
        public IEnumerator InsertedObjectShouldHaveCorrectPosition()
        {
            yield return new WaitUntil(() => mediatorSpy.allRequestedObjectsAreLoaded);
            Assert.AreEqual(insertPosition, mediatorSpy.GetLastLoadedObject().transform.position);
            yield return null;
        }

        [UnityTest]
        public IEnumerator InsertedObjectShouldHaveDefinedId()
        {
            yield return new WaitUntil(() => mediatorSpy.allRequestedObjectsAreLoaded);
            Assert.AreEqual(insertObjectMessagePayload.objectId, idReader.GetIdFromObject(mediatorSpy.GetLastLoadedObject()));
            yield return null;
        }

        [UnityTest]
        public IEnumerator InsertedObjectShouldHaveColliders()
        {
            yield return new WaitUntil(() => mediatorSpy.allRequestedObjectsAreLoaded);
            Assert.IsTrue(CheckMeshesCollider(mediatorSpy.GetLastLoadedObject()));
            yield return null;
        }

        [UnityTest]
        public IEnumerator ShouldPreserveDifferentSettingsForSimultaneouslyInsertedObjects()
        {
            InsertObjectMessagePayload anotherInsertMessagePayload = new()
            {
                objectId = "object-id-2",
                objectUrl = "object-url-2",
                selectObjectAfterInsertion = false,
                deselectPreviousSelection = false,
            };
            RequesObject(anotherInsertMessagePayload);
            yield return new WaitUntil(() => mediatorSpy.allRequestedObjectsAreLoaded);

            InsertedObjectPayload firstObjectPayload = mediatorSpy.loadedObjectsPayloads.Find(payload => payload.selectObjectAfterInsertion == true);
            InsertedObjectPayload secondObjectPayload = mediatorSpy.loadedObjectsPayloads.Find(payload => payload.selectObjectAfterInsertion == false);

            Assert.AreEqual(meshDownloaderSpy.loadedObjects[0], firstObjectPayload.loadedObject);
            Assert.AreEqual(meshDownloaderSpy.loadedObjects[1], secondObjectPayload.loadedObject);
            Assert.AreEqual(insertObjectMessagePayload.deselectPreviousSelection, firstObjectPayload.deselectPreviousSelection);
            Assert.AreEqual(anotherInsertMessagePayload.deselectPreviousSelection, secondObjectPayload.deselectPreviousSelection);
            yield return null;

            //Assert.AreEqual(insertObjectMessagePayload.selectObjectAfterInsertion, firstObjectPayload.selectObjectAfterInsertion);
            //Assert.AreEqual(insertObjectMessagePayload.deselectPreviousSelection, firstObjectPayload.deselectPreviousSelection);

            //Assert.AreEqual(meshDownloaderSpy.loadedObjects[0], mediatorSpy.loadedObjectsPayloads[0].loadedObject);
            //Assert.AreEqual(insertObjectMessagePayload.selectObjectAfterInsertion, mediatorSpy.loadedObjectsPayloads[1].selectObjectAfterInsertion);
            //Assert.AreEqual(insertObjectMessagePayload.deselectPreviousSelection, mediatorSpy.loadedObjectsPayloads[1].deselectPreviousSelection);
            //Assert.AreEqual(anotherPayload.selectObjectAfterInsertion, mediatorSpy.loadedObjectsPayloads[0].selectObjectAfterInsertion);
            //Assert.AreEqual(anotherPayload.deselectPreviousSelection, mediatorSpy.loadedObjectsPayloads[0].deselectPreviousSelection);
            yield return null;
        }


    }

    class MediatorSpy : IMediator
    {
        public int onProgressNumberOfCalls = 0;
        public List<InsertedObjectPayload> loadedObjectsPayloads;
        public bool allRequestedObjectsAreLoaded;

        private int requestedObjectsCount;
        private int loadedObjectsCount;
        private delegate void ObjectLoadedEventHandler();
        private event ObjectLoadedEventHandler ObjectLoaded;


        public MediatorSpy()
        {
            loadedObjectsPayloads = new List<InsertedObjectPayload>();
            requestedObjectsCount = 0;
            allRequestedObjectsAreLoaded = false;
            ObjectLoaded += () => NewLoadedObject();
        }
        public void IncreaseObjectRequestedCount()
        {
            requestedObjectsCount++;
        }
        public InsertedObjectPayload GetLastInsertedObjectPayload()
        {
            return loadedObjectsPayloads[loadedObjectsPayloads.Count - 1];
        }
        public GameObject GetLastLoadedObject()
        {
            return GetLastInsertedObjectPayload().loadedObject;
        }
        private void NewLoadedObject()
        {
            loadedObjectsCount++;
            if (loadedObjectsCount == requestedObjectsCount)
            {
                allRequestedObjectsAreLoaded = true;
                ObjectLoaded -= NewLoadedObject;
            }
        }

        public void Notify(ReupEvent eventName)
        {
            throw new System.NotImplementedException();
        }

        public void Notify<T>(ReupEvent eventName, T payload)
        {
            switch (eventName)
            {
                case ReupEvent.insertedObjectStatusUpdate:
                    onProgressNumberOfCalls++;
                    break;
                case ReupEvent.insertedObjectLoaded:
                    loadedObjectsPayloads.Add(((InsertedObjectPayload)(object)payload));
                    ObjectLoaded?.Invoke();
                    break;
            }
        }
    }

    class MeshDownloaderSpy : IMeshDownloader
    {
        public int numberOfCalls;
        public List<GameObject> loadedObjects;

        public MeshDownloaderSpy()
        {
            loadedObjects = new List<GameObject>();
            numberOfCalls = 0;
        }
        public GameObject GetLastLoadedObject()
        {
            return loadedObjects[loadedObjects.Count - 1];
        }

        private GameObject CreateGameObject()
        {
            GameObject parent = new();
            GameObject child = new();
            MeshFilter meshFilter = child.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = new Mesh();
            child.transform.parent = parent.transform;
            return parent;
        }

        public async void downloadMesh(string meshUrl, Action<ModelLoaderContext, float> onProgress, Action<ModelLoaderContext> onLoad, Action<ModelLoaderContext> onMaterialsLoad)
        {
            numberOfCalls++;
            int downloadTime = 60;
            int processingTime = 30;
            GameObject obj = CreateGameObject();
            loadedObjects.Add(obj);
            ModelLoaderContext modelLoaderContext = new()
            {
                loadedObject = obj,
            };
            onProgress(modelLoaderContext, 0.3f);
            onProgress(modelLoaderContext, 0.6f);
            onProgress(modelLoaderContext, 0.9f);
            onProgress(modelLoaderContext, 1f);
            await Task.Delay(downloadTime);
            onLoad(modelLoaderContext);
            await Task.Delay(processingTime);
            onMaterialsLoad(modelLoaderContext);
        }

        private class AssetLoaderContextStub
        {
            public GameObject RootGameObject;
        }
    }
}
