using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using ReupVirtualTwin.managers;
using ReupVirtualTwin.enums;
using ReupVirtualTwin.managerInterfaces;
using System;
using ReupVirtualTwin.models;
using ReupVirtualTwin.modelInterfaces;
using ReupVirtualTwin.controllers;
using ReupVirtualTwin.helpers;
using ReupVirtualTwin.dataModels;

public class DeleteObjectsManagerTest : MonoBehaviour
{
    GameObject containerGameObject;
    DeleteObjectsManager deleteObjectsManager;
    MockMediator mockMediator;
    MockRegistry mockRegistry;
    public List<GameObject> allObjects = new List<GameObject>();

    [SetUp]
    public void SetUp()
    {
        containerGameObject = new GameObject("containerGameObject");
        deleteObjectsManager = containerGameObject.AddComponent<DeleteObjectsManager>();
        deleteObjectsManager.tagsController = new TagsController();
        mockMediator = new MockMediator();
        deleteObjectsManager.mediator = mockMediator;
        mockRegistry = new MockRegistry();
        deleteObjectsManager.registry = mockRegistry;
        allObjects = mockRegistry.allObjects;
    }
    [TearDown]
    public void TearDown()
    {
        GameObject.DestroyImmediate(containerGameObject);
        mockRegistry.DestroyTestObjects();
    }
    public string ListToString(List<string> idsList)
    {
        string idsString = string.Join(",", idsList);
        return idsString;
    }
    public List<string> GetIDsList(List<GameObject> gameObjects)
    {
        List<string> stringIDs = new List<string>();
        foreach (GameObject obj in gameObjects)
        {
            stringIDs.Add(obj.GetComponent<UniqueId>().getId());
        }
        return stringIDs;
    }
    [UnityTest]
    public IEnumerator ShouldDeleteDeletableObjects()
    {
        List<GameObject> gameObjects = new List<GameObject>() { allObjects[0], allObjects[1] };
        string stringIDs = ListToString(GetIDsList(gameObjects));
        Assert.IsNotEmpty(deleteObjectsManager.GetDeletableObjects(stringIDs));
        yield return null;

    }
    [UnityTest]
    public IEnumerator ShouldFailWhenEmptyIDsString()
    {
        Assert.IsEmpty(deleteObjectsManager.GetDeletableObjects(""));
        yield return null;
    }
    [UnityTest]
    public IEnumerator ShouldFailWhenTryingToDeleteNonDeletableObjects()
    {
        List<GameObject> gameObjects = new List<GameObject>() { allObjects[0], allObjects[1], allObjects[2] };
        string stringIDs = ListToString(GetIDsList(gameObjects));
        Assert.IsEmpty(deleteObjectsManager.GetDeletableObjects(stringIDs));
        yield return null;

    }
    private class MockMediator : IMediator
    {
        public bool deleteModeActive = false;
        public bool notified = false;

        public void Notify(ReupEvent eventName)
        {
            if (eventName == ReupEvent.objectsDeleted)
            {
                notified = true;
            }
        }

        public void Notify<T>(ReupEvent eventName, T payload)
        {
            throw new System.NotImplementedException();
        }
    }
    private class MockRegistry : IObjectRegistry
    {
        public List<GameObject> allObjects = new List<GameObject>();
        public MockRegistry()
        {
            GameObject deletableObject0 = new GameObject("deletableObject0");
            deletableObject0.AddComponent<ObjectTags>().AddTags(new Tag[2] { EditionTagsCreator.CreateSelectableTag(), EditionTagsCreator.CreateDeletableTag() });
            deletableObject0.AddComponent<UniqueId>().GenerateId();
            GameObject deletableObject1 = new GameObject("deletableObject1");
            deletableObject1.AddComponent<ObjectTags>().AddTags(new Tag[2] { EditionTagsCreator.CreateSelectableTag(), EditionTagsCreator.CreateDeletableTag() });
            deletableObject1.AddComponent<UniqueId>().GenerateId();
            GameObject nonDeletableObject = new GameObject("nonDeletableObject");
            nonDeletableObject.AddComponent<ObjectTags>().AddTags(new Tag[1] { EditionTagsCreator.CreateSelectableTag() });
            nonDeletableObject.AddComponent<UniqueId>().GenerateId();
            allObjects.Add(deletableObject0);
            allObjects.Add(deletableObject1);
            allObjects.Add(nonDeletableObject);
        }

        public void AddObject(GameObject obj)
        {
            throw new NotImplementedException();
        }
        public GameObject GetObjectWithGuid(string guid)
        {
            foreach (GameObject obj in allObjects)
            {
                if (obj == null) continue;
                var uniqueIdentifier = obj.GetComponent<UniqueId>();
                if (uniqueIdentifier.isIdCorrect(guid))
                {
                    return obj;
                }
            }
            return null;
        }
        public List<GameObject> GetObjectsWithGuids(string[] guids)
        {
            var foundObjects = new List<GameObject>();
            foreach (string guid in guids)
            {
                foundObjects.Add(GetObjectWithGuid(guid));
            }
            return foundObjects;
        }
        public List<GameObject> GetItemTreesWithParentGuids(List<string> stringIDs)
        {
            throw new NotImplementedException();
        }

        public void RemoveObject(string id, GameObject item)
        {
            throw new NotImplementedException();
        }

        public int GetObjectsCount()
        {
            throw new NotImplementedException();
        }

        public void ClearRegistry()
        {
            throw new NotImplementedException();
        }

        public List<GameObject> GetObjects()
        {
            throw new NotImplementedException();
        }
        public void DestroyTestObjects()
        {
            foreach (GameObject obj in allObjects)
            {
                GameObject.DestroyImmediate(obj);
            }
        }
    }

}
