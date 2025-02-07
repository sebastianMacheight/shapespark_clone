using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using System.Collections;

using ReupVirtualTwin.behaviours;
using ReupVirtualTwinTests.instantiators;


public class MaintainHeightTest : MonoBehaviour
{
    ReupSceneInstantiator.SceneObjects sceneObjects;
    GameObject platformPrefab;
    Transform character;
    GameObject widePlatform;

    float HEIGHT_CLOSENESS_THRESHOLD = 0.02f;

    [SetUp]
    public void SetUp()
    {
        platformPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Packages/com.reup.romulo/Tests/TestAssets/Platform.prefab");
        sceneObjects = ReupSceneInstantiator.InstantiateScene();
        character = sceneObjects.character;
        var posManager = sceneObjects.characterPositionManager;
        posManager.maxStepHeight = 0.25f;
        widePlatform = (GameObject)PrefabUtility.InstantiatePrefab(platformPrefab);
        SetPlatform();
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        GameObject.DestroyImmediate(widePlatform);
        ReupSceneInstantiator.DestroySceneObjects(sceneObjects);
        yield return null;
    }

    [UnityTest]
    public IEnumerator CharacterShouldFallToDesiredHeight()
    {
        character.transform.position = new Vector3(0, 4, 0);
        yield return new WaitForSeconds(1);
        var distanceToDesiredHeight = MaintainHeight.GetDesiredHeightInGround(0) - character.transform.position.y;
        Assert.LessOrEqual(distanceToDesiredHeight, HEIGHT_CLOSENESS_THRESHOLD);
    }

    [UnityTest]
    public IEnumerator CharacterShouldRiseToDesiredHeight()
    {
        character.transform.position = new Vector3(0, 1.5f, 0);
        yield return new WaitForSeconds(0.2f);
        var distanceToDesiredHeight = MaintainHeight.GetDesiredHeightInGround(0) - character.transform.position.y;
        Assert.LessOrEqual(distanceToDesiredHeight, HEIGHT_CLOSENESS_THRESHOLD);
    }

    [UnityTest]
    public IEnumerator CharacterShouldRiseToDesiredHeightWhenPlatformRises()
    {
        character.transform.position = new Vector3(0, 1.5f, 0);
        yield return new WaitForSeconds(0.2f);
        var upDistance = 0.1f;
        widePlatform.transform.position = new Vector3(0, upDistance, 0);
        yield return new WaitForSeconds(0.2f);
        var distanceToDesiredHeight = MaintainHeight.GetDesiredHeightInGround(0) + upDistance - character.transform.position.y;
        Assert.LessOrEqual(distanceToDesiredHeight, HEIGHT_CLOSENESS_THRESHOLD);
    }

    [UnityTest]
    public IEnumerator CharacterShouldNotRiseIfDistanceIsTooBig()
    {
        character.transform.position = new Vector3(0, 1.5f, 0);
        yield return new WaitForSeconds(0.2f);
        var upDistance = 0.3f;
        widePlatform.transform.position = new Vector3(0, upDistance, 0);
        yield return new WaitForSeconds(0.1f);
        var distanceToDesiredHeight = MaintainHeight.GetDesiredHeightInGround(0) - character.transform.position.y;
        Assert.LessOrEqual(distanceToDesiredHeight, HEIGHT_CLOSENESS_THRESHOLD);
    }

    private void SetPlatform()
    {
        widePlatform.transform.localScale = new Vector3(10, 0.1f, 10);
        widePlatform.transform.position = new Vector3(0, -0.05f, 0);
    }
}
