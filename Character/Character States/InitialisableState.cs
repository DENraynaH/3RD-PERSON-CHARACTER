namespace GameProject.Character
{
    public interface InitialisableState<in T>
    {
        public void InitialiseController(T playerStateController);
        public void InitialiseState();
    }
}