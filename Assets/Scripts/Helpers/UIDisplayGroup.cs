namespace BuilderTool.Helpers
{
    public class UIDisplayGroup : Singleton<UIDisplayGroup>
    {
        protected override void Awake()
        {
            DontDestroyOnLoad(this);
            base.Awake();
        }
    }
}
