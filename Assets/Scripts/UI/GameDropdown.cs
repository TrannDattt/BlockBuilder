using System;
using TMPro;

namespace BuilderTool.UIElement
{
    public abstract class GameDropdown<EOption> : TMP_Dropdown where EOption : Enum
    {
        protected virtual void FetchAllItem()
        {
            ClearOptions();

            foreach (var option in Enum.GetValues(typeof(EOption)))
            {
                options.Add(new OptionData(option.ToString()));
            }
        }

        protected override void Start()
        {
            base.Start();

            FetchAllItem();
            value = 0;

            if (captionText)
            {
                captionText.text = ((EOption)Enum.ToObject(typeof(EOption), value)).ToString();
            }
        }
    }
}
