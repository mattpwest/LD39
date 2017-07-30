using System;
using Assets.Scripts.UI;

namespace UI.Events
{
    public class JumpEvent : IEvent
    {
        private ShipResources shipResources;
        private String title;
        private String flavourText;
        private float startTime;

        public JumpEvent(ShipResources shipResources, String title, String flavourText)
        {
            this.shipResources = shipResources;
            this.title = title;
            this.flavourText = flavourText;
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
            this.startTime = this.shipResources.TimeLeft;

            this.shipResources.Jump();
        }

        public EventResult GetResult(bool wasAttacked)
        {
            var timeGained = (int) Math.Round(this.shipResources.TimeLeft - this.startTime);

            return new EventResult()
            {
                Title = this.title,
                FlavourText = this.flavourText,
                Gain1 = new EventResultItem() {Name = "Time", Value = timeGained}
            };
        }

        public bool IsBattle => false;
    }
}
