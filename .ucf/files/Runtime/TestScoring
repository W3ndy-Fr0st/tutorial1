using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEditor;
using System.Text.RegularExpressions;
// using UnityEngine.SceneManager;


namespace Tests
{
    public class RollABallTestsScoring : InputTestFixture
    {
        [UnityTest]
        public IEnumerator TestScorePresent()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
            yield return null;
            Text score = GameObject.Find("Count Text").GetComponent<Text>();
            PlayerController pc = GameObject.Find("Player").GetComponent<PlayerController>();
            Assert.True(pc.countText != null);
            Assert.True(Regex.Match(score.text, @"Count: \d+").Success);
        }

        [UnityTest]
        public IEnumerator TestScoreOnCollide()
        {
            Keyboard keyboard = InputSystem.AddDevice<Keyboard>();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
            yield return null;
            GameObject.Find("Pick Ups").SetActive(false);

            GameObject pickup = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Pick Up.prefab", typeof(GameObject));// (GameObject)Resources.Load("Prefabs/Pick Up");
            GameObject instance = GameObject.Instantiate(pickup, new Vector3(0, 0, 3), new Quaternion(0, 0, 0, 0));
            Text score = GameObject.Find("Count Text").GetComponent<Text>();
            var startScore = score.text;
            Debug.Log(Regex.Match(startScore, @"\d").Value);
            int curScore;
            int.TryParse(Regex.Match(startScore, @"\d").Value, out curScore);
            Press(keyboard.upArrowKey);
            yield return new WaitForSeconds(2.0f);
            StringAssert.AreEqualIgnoringCase(score.text, "Count: " + (curScore + 1));
        }

        [UnityTest]
        public IEnumerator TestWinText()
        {
            Keyboard keyboard = InputSystem.AddDevice<Keyboard>();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
            yield return null;
            GameObject.Find("Pick Ups").SetActive(false);

            GameObject pickup = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Pick Up.prefab", typeof(GameObject));// (GameObject)Resources.Load("Prefabs/Pick Up");
            for (var i = 0; i < 12; i++)
            {
                GameObject instance1 = GameObject.Instantiate(pickup, new Vector3(0, 0, (3f / 12f) * i), new Quaternion(0, 0, 0, 0));
            }

            Text winText = GameObject.Find("Win Text").GetComponent<Text>();
            var starttext = winText.text;
            StringAssert.AreEqualIgnoringCase(starttext, "");

            Press(keyboard.upArrowKey);
            yield return new WaitForSeconds(2.0f);
            Match rm = Regex.Match(winText.text, "you win", RegexOptions.IgnoreCase);
            Assert.True(rm.Success);
        }
    }
}
