using AlertaEnchentes.Desktop.Services;

namespace AlertaEnchentes.Desktop.Forms;

internal static class UiHelper
{
    public static readonly Font Fonte = new("Segoe UI", 10f);

    public static DataGridView CriarGrid()
    {
        var grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            AutoGenerateColumns = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            RowHeadersVisible = false,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            Font = Fonte,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };

        // Destaca a linha conforme o nível de risco (cor + texto, nunca só cor).
        grid.CellFormatting += (_, e) =>
        {
            if (e.RowIndex < 0) return;
            if (grid.Columns[e.ColumnIndex].DataPropertyName != "NivelRisco") return;
            if (e.Value is not string nivel) return;

            e.CellStyle.ForeColor = Color.Black;
            e.CellStyle.BackColor = nivel switch
            {
                "Alto" => Color.FromArgb(255, 205, 210),
                "Médio" => Color.FromArgb(255, 236, 179),
                _ => Color.FromArgb(200, 230, 201)
            };
        };

        return grid;
    }

    public static void AdicionarColuna(DataGridView grid, string titulo, string propriedade,
        float peso = 1f, string? formato = null)
    {
        var coluna = new DataGridViewTextBoxColumn
        {
            HeaderText = titulo,
            DataPropertyName = propriedade,
            FillWeight = peso * 100f
        };
        if (formato is not null) coluna.DefaultCellStyle.Format = formato;
        grid.Columns.Add(coluna);
    }

    public static Label Rotulo(string texto) => new()
    {
        Text = texto,
        AutoSize = true,
        Anchor = AnchorStyles.Left,
        Font = Fonte,
        Margin = new Padding(3, 8, 3, 3)
    };

    public static void MostrarErro(IWin32Window dono, Exception ex)
    {
        var mensagem = ex switch
        {
            ApiException api => api.Message,
            HttpRequestException => "Não foi possível conectar à API.\nVerifique se o projeto SOA-API está em execução.",
            TaskCanceledException => "A API demorou demais para responder. Tente novamente.",
            _ => ex.Message
        };

        MessageBox.Show(dono, mensagem, "Alerta Enchentes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
