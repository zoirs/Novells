    using UnityEngine.Events;

    public class InfoDialogParam {
        private string body;
        private UnityAction close;

        public InfoDialogParam(string body, UnityAction close)
        {
            this.body = body;
            this.close = close;
        }

        public string Body => body;

        public UnityAction Close => close;
    }