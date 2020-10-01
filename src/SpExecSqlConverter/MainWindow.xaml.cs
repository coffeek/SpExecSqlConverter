using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using SpExecSqlConverter.Properties;

namespace SpExecSqlConverter
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private readonly Lazy<PlainSqlConverter> converter = new Lazy<PlainSqlConverter>();
    private PlainSqlConverter Converter => this.converter.Value;

    private readonly Lazy<SqlFormatter> formatter = new Lazy<SqlFormatter>();
    private SqlFormatter Formatter => this.formatter.Value;

    public static readonly RoutedUICommand ConvertCommand = new RoutedUICommand(
      "Convert", "Convert", typeof(MainWindow));

    public MainWindow()
    {
      this.InitializeComponent();
      this.LoadSettings();
      DataObject.AddPastingHandler(this.TextEditor, this.OnPasteHandler);
    }

    private void OnPasteHandler(object sender, DataObjectPastingEventArgs e)
    {
      if (this.ConvertOnPasteCheckbox.IsChecked == true)
      {
        if (e.DataObject.GetData(typeof(string)) is string sourceData)
          e.DataObject = new DataObject(DataFormats.Text, this.ConvertQuery(sourceData));
      }
    }

    private static IDisposable WaitCursor()
    {
      var oldCursor = Mouse.OverrideCursor;
      Mouse.OverrideCursor = Cursors.Wait;
      return new DisposableObject(() => Mouse.OverrideCursor = oldCursor);
    }

    private void ConvertCommandExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      this.TextEditor.Text = this.ConvertQuery(this.TextEditor.Text);
    }

    private string ConvertQuery(string sqlCommand)
    {
      try
      {
        using (WaitCursor())
        {
          sqlCommand = this.Converter.ConvertFromSpExecSql(sqlCommand);
          sqlCommand = this.Formatter.Format(sqlCommand);
        }
      }
      catch (FormatSqlException ex)
      {
        new TaskDialog
        {
          Caption = "Предупреждение",
          StandardButtons = TaskDialogStandardButtons.Ok,
          Text = "Не удалось отформатировать запрос",
          Icon = TaskDialogStandardIcon.Warning,
          DetailsExpandedText = string.Join(Environment.NewLine, ex.ParseErrors.Select(error => error.Message))
        }.Show();
      }
      return sqlCommand;
    }

    private void LoadSettings()
    {
      this.Width = Math.Min(SystemParameters.VirtualScreenWidth, Settings.Default.WindowWidth);
      this.Height = Math.Min(SystemParameters.VirtualScreenHeight, Settings.Default.WindowHeight);
    }

    private void SaveSettings()
    {
      Settings.Default.WindowWidth = this.Width;
      Settings.Default.WindowHeight = this.Height;

      Settings.Default.Save();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      this.SaveSettings();
    }
  }
}
