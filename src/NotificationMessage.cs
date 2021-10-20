namespace CustomDialogs
{
    /// <summary>
    /// Class NotificationMessage.
    /// </summary>
    public class NotificationMessage
    {
        private string message = "";
        private string title = "";

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message
        {
            get => message;
            set => message = value;
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get => title;
            set => title = value;
        }
    }
}
