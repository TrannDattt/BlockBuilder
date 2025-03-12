namespace BuilderTool.Helpers
{
    public class UIManagerGroup : Singleton<UIManagerGroup>
    {
        protected override void Awake()
        {
            DontDestroyOnLoad(this);
            base.Awake();
        }
    }
}
