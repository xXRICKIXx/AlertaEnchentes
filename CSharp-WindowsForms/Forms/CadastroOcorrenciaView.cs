using AlertaEnchentes.Desktop.Models;
using AlertaEnchentes.Desktop.Services;

namespace AlertaEnchentes.Desktop.Forms;

/// <summary>Aba de cadastro de ocorrências (com validação e sugestão de risco).</summary>
public class CadastroOcorrenciaView : UserControl
{
    private readonly ApiClient _api;
    private readonly Action<string> _status;

    private readonly TextBox _txtLocal = new() { Text = "Brooklin", Dock = DockStyle.Fill, Font = UiHelper.Fonte };
    private readonly ComboBox _cmbTipo = new() { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = UiHelper.Fonte };
    private readonly ComboBox _cmbRisco = new() { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = UiHelper.Fonte };
    private readonly TextBox _txtDescricao = new() { Dock = DockStyle.Fill, Multiline = true, MaxLength = 500, ScrollBars = ScrollBars.Vertical, Font = UiHelper.Fonte };
    private readonly Button _btnSugerir = new() { Text = "Sugerir risco pela descrição", AutoSize = true, Font = UiHelper.Fonte };
    private readonly Button _btnRegistrar = new() { Text = "Registrar ocorrência", AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Bold) };
    private readonly Button _btnLimpar = new() { Text = "Limpar", AutoSize = true, Font = UiHelper.Fonte };

    public event EventHandler? OcorrenciaRegistrada;

    public CadastroOcorrenciaView(ApiClient api, Action<string> status)
    {
        _api = api;
        _status = status;

        _cmbTipo.Items.AddRange(new object[] { "Alagamento", "Via interditada", "Bueiro entupido", "Queda de árvore", "Falta de energia" });
        _cmbTipo.SelectedIndex = 0;
        _cmbRisco.Items.AddRange(NivelRisco.Niveis);
        _cmbRisco.SelectedIndex = 0;

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 5,
            Padding = new Padding(16)
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));

        layout.Controls.Add(UiHelper.Rotulo("Local:"), 0, 0);
        layout.Controls.Add(_txtLocal, 1, 0);
        layout.Controls.Add(UiHelper.Rotulo("Tipo:"), 0, 1);
        layout.Controls.Add(_cmbTipo, 1, 1);
        layout.Controls.Add(UiHelper.Rotulo("Nível de risco:"), 0, 2);
        layout.Controls.Add(_cmbRisco, 1, 2);
        layout.Controls.Add(UiHelper.Rotulo("Descrição:"), 0, 3);
        layout.Controls.Add(_txtDescricao, 1, 3);

        var botoes = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
        botoes.Controls.AddRange(new Control[] { _btnRegistrar, _btnSugerir, _btnLimpar });
        layout.Controls.Add(botoes, 1, 4);

        Controls.Add(layout);

        _btnSugerir.Click += (_, _) =>
        {
            _cmbRisco.SelectedItem = NivelRisco.Sugerir(_txtDescricao.Text);
            _status($"Risco sugerido: {_cmbRisco.SelectedItem}.");
        };
        _btnLimpar.Click += (_, _) => Limpar();
        _btnRegistrar.Click += async (_, _) => await RegistrarAsync();
    }

    private async Task RegistrarAsync()
    {
        var nova = new NovaOcorrencia
        {
            Local = _txtLocal.Text.Trim(),
            Tipo = _cmbTipo.SelectedItem as string ?? string.Empty,
            NivelRisco = _cmbRisco.SelectedItem as string ?? string.Empty,
            Descricao = _txtDescricao.Text.Trim()
        };

        var erros = Validador.Validar(nova);
        if (erros.Count > 0)
        {
            MessageBox.Show(this, string.Join(Environment.NewLine, erros), "Dados inválidos",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _btnRegistrar.Enabled = false;
        try
        {
            var criada = await _api.CriarOcorrenciaAsync(nova);
            var aviso = nova.NivelRisco == NivelRisco.Alto ? " Um alerta foi gerado automaticamente." : string.Empty;

            MessageBox.Show(this, $"Ocorrência #{criada?.Id} registrada com sucesso.{aviso}", "Alerta Enchentes",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            _status($"Ocorrência #{criada?.Id} registrada.");
            Limpar();
            OcorrenciaRegistrada?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex) when (ex is ApiException or HttpRequestException or TaskCanceledException)
        {
            UiHelper.MostrarErro(this, ex);
            _status("Falha ao registrar a ocorrência.");
        }
        finally
        {
            _btnRegistrar.Enabled = true;
        }
    }

    private void Limpar()
    {
        _txtLocal.Text = "Brooklin";
        _cmbTipo.SelectedIndex = 0;
        _cmbRisco.SelectedIndex = 0;
        _txtDescricao.Clear();
    }
}
