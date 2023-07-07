using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Michsky.UI.Frost
{
    public class FeedbackSystem : MonoBehaviour
    {
        public void SendFeedback()
        {
            string email = "isa.steam@outlook.com";
            string subject = MyEscapeURL("Frost UI - Feedback");
            string body = MyEscapeURL("Please enter your message here\n\n" +
             "------------------------------------------" +
             "\nPlease Do Not Modify This Area\n\n" +
             "Unity: " + Application.unityVersion + "\n" +
             "OS: " + SystemInfo.operatingSystem + "\n" +
             "Platform: " + Application.platform + "\n" +
             "GPU: " + SystemInfo.graphicsDeviceName + "\n" +
             "CPU: " + SystemInfo.processorType + "\n" +
             "---------------------------------------------------------------------\n\n");
 
            Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
        }

        string MyEscapeURL(string url)
        {
            return WWW.EscapeURL(url).Replace("+", "%20");
        }
    }
}