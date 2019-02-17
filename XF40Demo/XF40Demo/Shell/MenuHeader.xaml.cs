using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XF40Demo.Shell
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuHeader : ContentView
	{
		public MenuHeader ()
		{
			InitializeComponent ();
		}
	}
}