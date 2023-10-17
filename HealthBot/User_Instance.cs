using Bot.code;

namespace Bot.scripts
{
    class User_Instance
    {
        public State state { get; set; }
        public User_Instance(State s)
        {
            state = s;
        } // users in database
        public User_Instance()
        {
            state = State.Menu;
        } // new users
    }
}