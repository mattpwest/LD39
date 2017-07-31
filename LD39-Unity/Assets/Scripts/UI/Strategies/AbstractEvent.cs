using UnityEngine;

namespace Assets.Scripts.UI.Strategies
{
    public abstract class AbstractEvent : MonoBehaviour, IEvent
    {
        public GameObject Dialog;

        protected Skins skins;
        
        protected virtual void Start()
        {
        }

        protected virtual void Awake()
        {
            this.skins = GameObject.FindObjectOfType<Skins>();
        }

        public abstract bool HasDialog();
        public abstract void HideDialog();

        public virtual void ShowDialog()
        {
            Dialog.SetActive(true);

            if (skins != null)
            {
                skins.Skin(Dialog);
                skins = null;
            }
        }

        public abstract void ExecuteStep();
        public abstract EventResult GetResult(bool wasAttacked);
        public abstract bool IsBattle { get; }
    }
}
