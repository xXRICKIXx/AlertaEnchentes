using AlertaEnchentes.Desktop.Models;
using AlertaEnchentes.Desktop.Services;

namespace AlertaEnchentes.Desktop.Forms;

/// <summary>Aba de alertas: visualização e criação manual.</summary>
public class AlertasView : UserControl
{
    private readonly ApiClient _api;
    private readonly Action<string> _status;

    private readonly DataGridView _grid = UiHelper.CriarGrid();
    private readonly TextBox _txtMensagem = new() { Width = 420, MaxLength = 300, Font = UiHelper.Fonte };
    private readonly ComboBox _cmbRisco = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 100, Font = UiHelper.Fonte };
    private readonly Button _btnCriar = new() { Text = "Criar alerta", AutoSize = true, Font = UiHelper.Fonte };
    private readonly Button _btnAtualizar = new() { Text = "Atualizar", AutoSize = true, Font = UiHelper.Fonte };

    public AlertasView(ApiClient api, Action<string> status)
    {
        _api = api;
        _status = status;

        UiHelper.AdicionarColuna(_grid, "ID", "Id", 0.4f);
        UiHelper.AdicionarColuna(_grid, "Mensagem", "Mensagem", 3f);
        UiHelper.AdicionarColuna(_grid, "Risco", "NivelRisco", 0.6f);
        UiHelper.AdicionarColuna(_grid, "Data", "Data", 1.1f, "dd/MM/yyyy HH:mm");
        UiHelper.AdicionarColuna(_grid, "Status", "Status", 0.7f);

        _cmbRisco.Items.AddRange(NivelRisco.Niveis);
        _cmbRisco.SelectedItem = NivelRisco.Alto;

        var barra = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 48, Padding = new Padding(8, 8, 8, 0) };
        barra.Controls.Add(UiHelper.Rotulo("Mensagem:"));
        barra.Controls.Add(_txtMensagem);
        barra.Controls.Add(UiHelper.Rotulo("Risco:"));
        barra.Controls.Add(_cmbRisco);
        barra.Controls.AddRange(new Control[] { _btnCriar, _btnAtualizar });

        Controls.Add(_grid);
        Controls.Add(barra);

        _btnAtualizar.Click += async (_, _) => await CarregarAsync();
        _btnCriar.Click += async (_, _) => await CriarAsync();
    }

    public async Task CarregarAsync()
    {
        try
        {
            var alertas = await _api.ListarAlertasAsync();
            _grid.DataSource = alertas.OrderByDescending(a => a.Data).ToList();
            _status($"{alertas.Count} alerta(s) carregado(s).");
        }
        catch (Exception ex) when (ex is ApiException or HttpRequestException or TaskCanceledException)
        {
            UiHelper.MostrarErro(this, ex);
            _status("Falha ao carregar alertas.");
        }
    }

    private async Task CriarAsync()
    {
        var novo = new NovoAlerta
        {
            Mensagem = _txtMensagem.Text.Trim(),
            NivelRisco = _cmbRisco.SelectedItem as string ?? string.Empty
        };

        var erros = Validador.Validar(novo);
        if (erros.Count > 0)
        {
            MessageBox.Show(this, string.Join(Environment.NewLine, erros), "Dados inválidos",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            await _api.CriarAlertaAsync(novo);
            _txtMensagem.Clear();
            _status("Alerta criado.");
            await CarregarAsync();
        }
        catch (Exception ex) when (ex is ApiException or HttpRequestException or TaskCanceledException)
        {
            UiHelper.MostrarErro(this, ex);
        }
    }
}
