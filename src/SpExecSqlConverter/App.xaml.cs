using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace SpExecSqlConverter
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
      using (var dialog = new TaskDialog
      {
        Caption = "Ошибка",
        StandardButtons = TaskDialogStandardButtons.Ok,
        InstructionText = "Что-то пошло не так",
        Icon = TaskDialogStandardIcon.Error,
        DetailsExpandedText = e.Exception.Message,
        DetailsExpanded = false
      })
      {
        dialog.Show();
      }
      e.Handled = true;
    }
  }
}
