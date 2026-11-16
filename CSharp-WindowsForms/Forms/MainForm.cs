using AlertaEnchentes.Desktop.Services;

namespace AlertaEnchentes.Desktop.Forms;

public class MainForm : Form
{
    private readonly ApiClient _api = new();
    private readonly ToolStripStatusLabel _lblStatus = new("Pronto");

    private readonly TabControl _tabs = new() { Dock = DockStyle.Fill, Font = UiHelper.Fonte };
    private readonly CadastroOcorrenciaView _cadastro;
    private readonly ConsultaOcorrenciasView _consulta;
    private readonly AlertasView _alertas;

    public MainForm()
    {
        Text = "Alerta Enchentes — Alagamentos no Brooklin, São Paulo";
        MinimumSize = new Size(900, 560);
        Size = new Size(1100, 640);
        StartPosition = FormStartPosition.CenterScreen;

        _cadastro = new CadastroOcorrenciaView(_api, SetStatus) { Dock = DockStyle.Fill };
        _consulta = new ConsultaOcorrenciasView(_api, SetStatus) { Dock = DockStyle.Fill };
        _alertas = new AlertasView(_api, SetStatus) { Dock = DockStyle.Fill };

        AdicionarAba("Registrar ocorrência", _cadastro);
        AdicionarAba("Consultar ocorrências", _consulta);
        AdicionarAba("Alertas", _alertas);

        var statusStrip = new StatusStrip();
        statusStrip.Items.Add(_lblStatus);

        Controls.Add(_tabs);
        Controls.Add(statusStrip);

        // Após registrar, mostra a lista atualizada (a API pode ter gerado um alerta).
        _cadastro.OcorrenciaRegistrada += async (_, _) =>
        {
            _tabs.SelectedIndex = 1;
            await _consulta.CarregarAsync();
            await _alertas.CarregarAsync();
        };

        _tabs.SelectedIndexChanged += async (_, _) =>
        {
            if (_tabs.SelectedIndex == 1) await _consulta.CarregarAsync();
            else if (_tabs.SelectedIndex == 2) await _alertas.CarregarAsync();
        };

        Shown += async (_, _) =>
        {
            await _consulta.CarregarAsync();
            await _alertas.CarregarAsync();
        };

        FormClosed += (_, _) => _api.Dispose();
    }

    private void AdicionarAba(string titulo, Control conteudo)
    {
        var aba = new TabPage(titulo);
        aba.Controls.Add(conteudo);
        _tabs.TabPages.Add(aba);
    }

    private void SetStatus(string mensagem) => _lblStatus.Text = mensagem;
}
