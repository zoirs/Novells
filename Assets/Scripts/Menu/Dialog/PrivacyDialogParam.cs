using UnityEngine.Events;

    public class PrivacyDialogParam {
        private string body;
        private UnityAction agree;
        private UnityAction read;

        public PrivacyDialogParam(string body, UnityAction agree, UnityAction read)
        {
            this.body = body;
            this.agree = agree;
            this.read = read;
        }

        public string Body => body;

        public UnityAction Agree => agree;
        public UnityAction Read => read;
    }