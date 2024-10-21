    using UnityEngine.Events;

    public class AboutMeDialogParam {
        private string body;
        private UnityAction close;
        private UnityAction rate;

        public AboutMeDialogParam(string body, UnityAction close, UnityAction rate)
        {
            this.body = body;
            this.close = close;
            this.rate = rate;
        }

        public string Body => body;

        public UnityAction Close => close;

        public UnityAction Rate => rate;
    }