using AlertaEnchentes.Desktop.Models;
using AlertaEnchentes.Desktop.Services;

namespace AlertaEnchentes.Desktop.Forms;

/// <summary>Aba de consulta: lista, filtro por risco, detalhe e atualização de status.</summary>
public class ConsultaOcorrenciasView : UserControl
{
    private readonly ApiClient _api;
    private readonly Action<string> _status;

    private readonly DataGridView _grid = UiHelper.CriarGrid();
    private readonly ComboBox _cmbFiltro = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130, Font = UiHelper.Fonte };
    private readonly Button _btnAtualizar = new() { Text = "Atualizar", AutoSize = true, Font = UiHelper.Fonte };
    private readonly Button _btnAcompanhar = new() { Text = "Em acompanhamento", AutoSize = true, Font = UiHelper.Fonte };
    private readonly Button _btnResolver = new() { Text = "Marcar como resolvida", AutoSize = true, Font = UiHelper.Fonte };

    private List<Ocorrencia> _todas = new();

    public ConsultaOcorrenciasView(ApiClient api, Action<string> status)
    {
        _api = api;
        _status = status;

        UiHelper.AdicionarColuna(_grid, "ID", "Id", 0.4f);
        UiHelper.AdicionarColuna(_grid, "Local", "Local", 1f);
        UiHelper.AdicionarColuna(_grid, "Tipo", "Tipo", 1f);
        UiHelper.AdicionarColuna(_grid, "Risco", "NivelRisco", 0.6f);
        UiHelper.AdicionarColuna(_grid, "Descrição", "Descricao", 2.2f);
        UiHelper.AdicionarColuna(_grid, "Data", "Data", 1.1f, "dd/MM/yyyy HH:mm");
        UiHelper.AdicionarColuna(_grid, "Status", "Status", 1f);

        _cmbFiltro.Items.Add("Todos");
        _cmbFiltro.Items.AddRange(NivelRisco.Niveis);
        _cmbFiltro.SelectedIndex = 0;

        var barra = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 48, Padding = new Padding(8, 8, 8, 0) };
        barra.Controls.Add(UiHelper.Rotulo("Filtrar por risco:"));
        barra.Controls.Add(_cmbFiltro);
        barra.Controls.AddRange(new Control[] { _btnAtualizar, _btnAcompanhar, _btnResolver });

        Controls.Add(_grid);
        Controls.Add(barra);

        _cmbFiltro.SelectedIndexChanged += (_, _) => AplicarFiltro();
        _btnAtualizar.Click += async (_, _) => await CarregarAsync();
        _btnAcompanhar.Click += async (_, _) => await AlterarStatusAsync("Em acompanhamento");
        _btnResolver.Click += async (_, _) => await AlterarStatusAsync("Resolvida");
        _grid.CellDoubleClick += async (_, e) => { if (e.RowIndex >= 0) await MostrarDetalheAsync(); };
    }

    public async Task CarregarAsync()
    {
        try
        {
            _todas = await _api.ListarOcorrenciasAsync();
            AplicarFiltro();
            _status($"{_todas.Count} ocorrência(s) carregada(s).");
        }
        catch (Exception ex) when (ex is ApiException or HttpRequestException or TaskCanceledException)
        {
            UiHelper.MostrarErro(this, ex);
            _status("Falha ao carregar ocorrências.");
        }
    }

    private void AplicarFiltro()
    {
        var filtro = _cmbFiltro.SelectedItem as string;
        IEnumerable<Ocorrencia> itens = _todas;

        if (!string.IsNullOrEmpty(filtro) && filtro != "Todos")
            itens = itens.Where(o => o.NivelRisco == filtro);

        _grid.DataSource = itens.OrderByDescending(o => o.Data).ToList();
    }

    private Ocorrencia? Selecionada => _grid.CurrentRow?.DataBoundItem as Ocorrencia;

    private async Task AlterarStatusAsync(string novoStatus)
    {
        var item = Selecionada;
        if (item is null)
        {
            MessageBox.Show(this, "Selecione uma ocorrência na lista.", "Alerta Enchentes",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            await _api.AtualizarStatusAsync(item.Id, novoStatus);
            _status($"Ocorrência #{item.Id} agora está \"{novoStatus}\".");
            await CarregarAsync();
        }
        catch (Exception ex) when (ex is ApiException or HttpRequestException or TaskCanceledException)
        {
            UiHelper.MostrarErro(this, ex);
        }
    }

    private async Task MostrarDetalheAsync()
    {
        var item = Selecionada;
        if (item is null) return;

        try
        {
            var o = await _api.ObterOcorrenciaAsync(item.Id) ?? item;
            MessageBox.Show(this,
                $"Ocorrência #{o.Id}\n\nLocal: {o.Local}\nTipo: {o.Tipo}\nRisco: {o.NivelRisco}\n" +
                $"Status: {o.Status}\nData: {o.Data:dd/MM/yyyy HH:mm}\n\n{o.Descricao}",
                "Detalhe da ocorrência", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex) when (ex is ApiException or HttpRequestException or TaskCanceledException)
        {
            UiHelper.MostrarErro(this, ex);
        }
    }
}
