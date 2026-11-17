using System;
using System.Collections.Generic;
using System.Text;

namespace TTOptimize.Core
{
    public interface ISettingsService
    {
        string GetSetting(string settingId);

        string this[string settingType] { get; }
    }
}
