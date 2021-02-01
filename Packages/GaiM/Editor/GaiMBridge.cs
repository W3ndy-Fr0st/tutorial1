using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
namespace edu.ucf.caam
{
    [Serializable]
    public struct LogMessage
    {
        public string msg;
        public string st;
    }
    // ensure class initializer is called whenever scripts recompile
    [InitializeOnLoadAttribute]
    public static class CamBridge
    {

        public static string output = "";
        public static string stack = "";
        public static string secret = "";
        public static string repo = "";
        public struct LogMessages
        {
            public string ts;
            public List<LogMessage> log;
        }
        public static LogMessages logMessages = new LogMessages { log = new List<LogMessage>() };
        public static int logCount = 0;
        static LogMessage lm;
        /// <summary>
        /// This function is called when this object becomes enabled and active
        /// </summary>
        static CamBridge()
        {
            string[] s = Application.dataPath.Split('/');
            string projectName = s[s.Length - 2];
            if (!EditorPrefs.HasKey("secret-" + projectName) || EditorPrefs.GetString("secret-" + projectName).Length == 0)
            {
                UnityEditor.EditorApplication.playModeStateChanged += DidPlay;
                var secretPath = Application.dataPath.Substring(0, Application.dataPath.Length - 7) + "/.ucf/.secret";
                if (File.Exists(secretPath))
                {
                    StreamReader sr = new StreamReader(secretPath, false);
                    secret = sr.ReadLine();
                    sr.Close();
                    EditorPrefs.SetString("secret-" + projectName, secret);
                }
            }
            if (!EditorPrefs.HasKey("repo-" + projectName) || EditorPrefs.GetString("repo-" + projectName).Length == 0)
            {
                var repoPath = Application.dataPath.Substring(0, Application.dataPath.Length - 7) + "/.ucf/.repo";
                if (File.Exists(repoPath))
                {
                    StreamReader sr = new StreamReader(repoPath, false);
                    repo = sr.ReadLine();
                    sr.Close();
                    EditorPrefs.SetString("repo-" + projectName, repo);
                }
            }
            else
            {
                secret = EditorPrefs.GetString("secret-" + projectName);
                repo = EditorPrefs.GetString("repo-" + projectName);
            }
            Application.logMessageReceived += HandleLog;
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void DidReloadScripts()
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(Post("https://plato.mrl.ai:8081/git/event", "{\"compiled\":true}"));
            File.Delete(Application.dataPath.Substring(0, Application.dataPath.Length - 7) + "/.ucf/.compilerError");
        }
        private static String prettyPrintErrors()
        {
            string str = "";
            foreach (var msg in logMessages.log)
            {
                str += msg.msg + "\n\r";
            }
            return str;
        }

        private static void DidPlay(PlayModeStateChange state)
        {
            //
        }
        static IEnumerator Post(string url, string bodyJsonString)
        {
            var request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            // request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            // Debug.Log("secret: " + secret);
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("secret", secret);
            request.SetRequestHeader("repo", repo);
            yield return request.SendWebRequest();
        }
        // keep a copy of the executing script
        private static EditorCoroutine coroutine;

        static IEnumerator EditorAttempt(float waitTime)
        {
            yield return new EditorWaitForSeconds(waitTime);
            if (logMessages.log.Count > 0)
            {
                EditorCoroutineUtility.StartCoroutineOwnerless(Post("https://plato.mrl.ai:8081/git/event", JsonUtility.ToJson(logMessages)));
                File.WriteAllText(Application.dataPath.Substring(0, Application.dataPath.Length - 7) + "/.ucf/.compilerError", prettyPrintErrors());
                logMessages.log.Clear();
            }
        }
        // 
        static void HandleLog(string logString, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                    // Debug.Log("Error Message");
                    break;
                default:
                    return;
            }
            output = logString;
            stack = stackTrace;
            lm = new LogMessage
            {
                st = stack,
                msg = output,
            };
            logMessages.log.Add(lm);
            if (logCount == 0)
            {
                logMessages.ts = DateTime.Now.ToString();
                coroutine = EditorCoroutineUtility.StartCoroutineOwnerless(EditorAttempt(0.5f));
            }
            else
            {
                EditorCoroutineUtility.StopCoroutine(coroutine);
                coroutine = EditorCoroutineUtility.StartCoroutineOwnerless(EditorAttempt(0.5f));
            }
            // sw.Close();
        }
    }
}