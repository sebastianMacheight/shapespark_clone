using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using ReupVirtualTwin.models;
using ReupVirtualTwin.modelInterfaces;
using ReupVirtualTwinTests.instantiators;
using System;

namespace ReupVirtualTwinTests.Registry
{
    public class ObjectRegistryTests : MonoBehaviour
    {
        ReupSceneInstantiator.SceneObjects sceneObjects;
        IObjectRegistry objectRegistry;
        GameObject testObj0;
        GameObject testObj1;
        int originalObjectsCount;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            sceneObjects = ReupSceneInstantiator.InstantiateScene();
            objectRegistry = sceneObjects.objectRegistry;
            originalObjectsCount = objectRegistry.GetObjectsCount();
            yield return null;
        }

        [UnityTearDown]
        public IEnumerator TearDownCoroutine()
        {
            Destroy(testObj0);
            Destroy(testObj1);
            objectRegistry.ClearRegistry();
            ReupSceneInstantiator.DestroySceneObjects(sceneObjects);
            yield return null;
        }

        [UnityTest]
        public IEnumerator ShouldAddAnItemToRegistry()
        {
            testObj0 = new GameObject("testObj");
            IUniqueIdentifier uniqueIdentifier = testObj0.AddComponent<UniqueId>();
            string id = uniqueIdentifier.GenerateId();
            objectRegistry.AddObject(testObj0);
            var retrievedObj = objectRegistry.GetObjectWithGuid(id);
            Assert.AreEqual(testObj0, retrievedObj);
            Assert.AreEqual(originalObjectsCount + 1, objectRegistry.GetObjectsCount());
            yield return null;
        }

        [UnityTest]
        public IEnumerator ShouldNotAddAnItemWithNoIdentifierToRegistry()
        {
            testObj0 = new GameObject("testObj");
            Assert.That(() => objectRegistry.AddObject(testObj0), Throws.Exception);
            yield return null;
        }

        [UnityTest]
        public IEnumerator ShouldNotAddAnItemWithNoIdToRegistry()
        {
            testObj0 = new GameObject("testObj");
            testObj0.AddComponent<UniqueId>();
            Assert.That(() => objectRegistry.AddObject(testObj0), Throws.Exception);
            yield return null;
        }

        [UnityTest]
        public IEnumerator ShouldAddSeveralItemsToRegistry()
        {
            testObj0 = new GameObject("testObj0");
            IUniqueIdentifier uniqueIdentifier0 = testObj0.AddComponent<UniqueId>();
            string id0 = uniqueIdentifier0.GenerateId();
            objectRegistry.AddObject(testObj0);
            var retrievedObj0 = objectRegistry.GetObjectWithGuid(id0);
            Assert.AreEqual(testObj0, retrievedObj0);
            Assert.AreEqual(originalObjectsCount + 1, objectRegistry.GetObjectsCount());
            yield return null;
            testObj1 = new GameObject("testObj1");
            IUniqueIdentifier uniqueIdentifier1 = testObj1.AddComponent<UniqueId>();
            string id1 = uniqueIdentifier1.GenerateId();
            objectRegistry.AddObject(testObj1);
            var retrievedObj1 = objectRegistry.GetObjectWithGuid(id1);
            Assert.AreEqual(testObj1, retrievedObj1);
            Assert.AreEqual(originalObjectsCount + 2, objectRegistry.GetObjectsCount());
            yield return null;
        }

        [UnityTest]
        public IEnumerator ShouldRemoveItem()
        {
            testObj0 = new GameObject("testObj0");
            IUniqueIdentifier uniqueIdentifier0 = testObj0.AddComponent<UniqueId>();
            string id = uniqueIdentifier0.GenerateId();
            objectRegistry.AddObject(testObj0);
            var retrievedObj = objectRegistry.GetObjectWithGuid(id);
            Assert.AreEqual(testObj0, retrievedObj);
            Assert.AreEqual(originalObjectsCount + 1, objectRegistry.GetObjectsCount());
            yield return null;

            objectRegistry.RemoveObject(id, testObj0);
            Assert.AreEqual(originalObjectsCount, objectRegistry.GetObjectsCount());
            Assert.IsNull(objectRegistry.GetObjectWithGuid(id));
            yield return null;
        }
        [UnityTest]
        public IEnumerator ShouldClearRegistry()
        {
            testObj0 = new GameObject("testObj0");
            IUniqueIdentifier uniqueIdentifier0 = testObj0.AddComponent<UniqueId>();
            uniqueIdentifier0.GenerateId();
            objectRegistry.AddObject(testObj0);
            testObj1 = new GameObject("testObj1");
            IUniqueIdentifier uniqueIdentifier1 = testObj1.AddComponent<UniqueId>();
            uniqueIdentifier1.GenerateId();
            objectRegistry.AddObject(testObj1);
            Assert.AreEqual(originalObjectsCount + 2, objectRegistry.GetObjectsCount());
            yield return null;
            objectRegistry.ClearRegistry();
            Assert.AreEqual(0, objectRegistry.GetObjectsCount());
            yield return null;
        }

        [UnityTest]
        public IEnumerator ShouldRaiseError_if_twoObjectsWithSameIdAreAttemptedToBeRegistered()
        {
            string repeatedId = "repeated-id";
            testObj0 = new GameObject("testObj0");
            testObj0.AddComponent<UniqueId>().AssignId(repeatedId);

            testObj1 = new GameObject("testObj1");
            testObj1.AddComponent<UniqueId>().AssignId(repeatedId);

            objectRegistry.AddObject(testObj0);
            Assert.That(() => objectRegistry.AddObject(testObj1),
                Throws.TypeOf<Exception>()
            );

            yield return null;
        }

        [UnityTest]
        public IEnumerator ShouldRaiseError_if_provideIncorrectIdAndObjectToBeRemoved()
        {
            string id = "test-id";
            testObj0 = new GameObject("testObj0");
            testObj0.AddComponent<UniqueId>().AssignId(id);
            yield return null;

            objectRegistry.AddObject(testObj0);
            testObj1 = new GameObject("testObj1");
            Assert.That(() => objectRegistry.RemoveObject(id, testObj1),
                Throws.TypeOf<Exception>()
            );

            yield return null;
        }

    }
}
