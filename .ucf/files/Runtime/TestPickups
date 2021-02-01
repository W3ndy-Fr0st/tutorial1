using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEditor;
// using UnityEngine.SceneManager;


namespace Tests
{
    public class RollABallTestsPickups : InputTestFixture
    {

        [UnityTest]
        public IEnumerator TestPickupConfiguration()
        {
            // var action1 = new InputAction("action1", binding: "<keyboard>/upArrow", interactions: "Hold");
            // UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
            // yield return null;
            GameObject pickup = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Pick Up.prefab", typeof(GameObject));// (GameObject)Resources.Load("Prefabs/Pick Up");
            GameObject instance = GameObject.Instantiate(pickup, new Vector3(0, 0, 3), new Quaternion(0, 0, 0, 0));
            Assert.True(instance != null);
            // Press(keyboard.rightArrowKey);
            yield return new WaitForSeconds(1.0f);
            Assert.True(instance.GetComponent<Rigidbody>() != null);
            Assert.True(instance.GetComponent<Rigidbody>().isKinematic);
            Assert.True(instance.GetComponent<BoxCollider>().isTrigger);
        }

        [UnityTest]
        public IEnumerator TestPickUpRemoved()
        {
            Keyboard keyboard = InputSystem.AddDevice<Keyboard>();
            // var action1 = new InputAction("action1", binding: "<keyboard>/upArrow", interactions: "Hold");
            UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
            yield return null;
            GameObject pickup = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Pick Up.prefab", typeof(GameObject));// (GameObject)Resources.Load("Prefabs/Pick Up");
            GameObject instance = GameObject.Instantiate(pickup, new Vector3(0, 0, 3), new Quaternion(0, 0, 0, 0));
            Assert.True(instance != null);
            Press(keyboard.upArrowKey);
            yield return new WaitForSeconds(2.0f);
            Assert.False(instance.activeInHierarchy);
        }

        [UnityTest]
        public IEnumerator TestPickUpsHaveTag()
        {
            GameObject pickup = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Pick Up.prefab", typeof(GameObject));// (GameObject)Resources.Load("Prefabs/Pick Up");
            GameObject instance = GameObject.Instantiate(pickup, new Vector3(0, 0, 3), new Quaternion(0, 0, 0, 0));
            yield return new WaitForSeconds(1.0f);
            Assert.True(instance != null);
            Assert.True(instance.CompareTag("Pick Up"));
        }

        [UnityTest]
        public IEnumerator TestRotatorPresent()
        {
            GameObject pickup = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Pick Up.prefab", typeof(GameObject));// (GameObject)Resources.Load("Prefabs/Pick Up");
            GameObject instance = GameObject.Instantiate(pickup, new Vector3(0, 0, 3), new Quaternion(0, 0, 0, 0));
            // Press(keyboard.rightArrowKey);
            yield return new WaitForSeconds(1.0f);
            Assert.True(pickup.transform.rotation.x != 0);
            Assert.True(pickup.transform.rotation.y != 0);
            Assert.True(pickup.transform.rotation.z != 0);
        }
    }
}
