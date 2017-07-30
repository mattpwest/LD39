using System;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.Events
{
    class LostEscapeEvent : IEvent
    {
        private float startTime;


        public LostEscapeEvent()
        {
        }

        public bool HasDialog()
        {
            return false;
        }

        public void HideDialog()
        {
            throw new NotImplementedException();
        }

        public void ShowDialog()
        {
            throw new NotImplementedException();
        }

        public void ExecuteStep()
        {
            SceneManager.LoadScene("LostEscape", LoadSceneMode.Single);
        }

        public EventResult GetResult(bool wasAttacked)
        {
            throw new NotImplementedException();
        }

        public bool IsBattle => false;
    }
}
