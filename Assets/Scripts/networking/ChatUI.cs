using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.Chat
{
    public class ChatUI : NetworkBehaviour
    {
        [Header("UI Elements")]
        public InputField chatMessage;
        public Text chatHistory;
        public Scrollbar scrollbar;

        [Header("Diagnostic - Do Not Edit")]
        public string localPlayerName;

        Dictionary<NetworkConnectionToClient, string> connNames = new Dictionary<NetworkConnectionToClient, string>();
        private bool clientName;
        public static ChatUI instance;
        private string name;
        
        void Awake()
        {
            instance = this;
        }

        [Command(requiresAuthority = false)]
        public void CmdSend(string message, NetworkConnectionToClient sender = null)
        {
            if (!connNames.ContainsKey(sender))
            {
                
                if (isLocalPlayer)
                {
                    name = "Player Left";
                }
                else
                {
                    name = "Player Right";
                }
                
                Debug.Log(sender.identity.GetComponent<PlayerManager>().isLeftPlayer);
                
                connNames.Add(sender, name);
            }

            if (!string.IsNullOrWhiteSpace(message))
            {
                RpcReceive(connNames[sender], message.Trim());
            }
        }
        
        [ClientRpc]
        public void RpcReceive(string playerName, string message)
        {
            string prettyMessage = playerName == localPlayerName ?
                $"<color=red>{playerName}:</color> {message}" :
                $"<color=blue>{playerName}:</color> {message}";
            AppendMessage(prettyMessage);
        }
        
        // Called by UI element SendButton.OnClick
        public void SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(chatMessage.text))
            {
                CmdSend(chatMessage.text.Trim());
                chatMessage.text = string.Empty;
                chatMessage.ActivateInputField();
            }
        }

        internal void AppendMessage(string message)
        {
            StartCoroutine(AppendAndScroll(message));
        }

        IEnumerator AppendAndScroll(string message)
        {
            chatHistory.text += message + "\n";

            // it takes 2 frames for the UI to update ?!?!
            yield return null;
            yield return null;

            // slam the scrollbar down
            scrollbar.value = 0;
        }
    }
}
