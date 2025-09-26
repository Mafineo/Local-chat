namespace LocalChat.Entities
{
    public class User
    {
        private string _username;

        public string Username => _username;

        public User(string username)
        {
            _username = username;
        }
    }
}