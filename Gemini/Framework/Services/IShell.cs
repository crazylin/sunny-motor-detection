using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Gemini.Modules.MainMenu;
using Gemini.Modules.Shell.ViewModels;
using Gemini.Modules.StatusBar;
using Gemini.Modules.ToolBars;

namespace Gemini.Framework.Services
{
    public interface IShell : IGuardClose, IDeactivate
	{
        event EventHandler ActiveDocumentChanging;
        event EventHandler ActiveDocumentChanged;

        bool ShowFloatingWindowsInTaskbar { get; set; }
        
		IMenu MainMenu { get; }
        IToolBars ToolBars { get; }
		IStatusBar StatusBar { get; }

        // TODO: Rename this to ActiveItem.
        ILayoutItem ActiveLayoutItem { get; set; }

        // TODO: Rename this to SelectedDocument.
		IDocument ActiveItem { get; }

		IObservableCollection<IDocument> Documents { get; }
		IObservableCollection<ITool> Tools { get; }

        void ShowTool<TTool>() where TTool : ITool;
		void ShowTool(ITool model);

		Task OpenDocumentAsync(IDocument model);
		Task CloseDocumentAsync(IDocument document);

		void Close();


        BusyIndicatorModel BusyIndicator { set; get; }
    }
}
