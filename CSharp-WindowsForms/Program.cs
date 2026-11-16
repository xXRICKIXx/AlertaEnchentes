using AlertaEnchentes.Desktop.Forms;

namespace AlertaEnchentes.Desktop;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}
