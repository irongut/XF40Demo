using System.Drawing;
using XF40Demo.Models;

namespace XF40Demo.Helpers
{
    public interface IEnvironment
    {
        Theme GetOSTheme();
        void SetStatusBarColor(Color color, bool darkStatusBarTint);
    }
}
