namespace GitHelper.Forms;

public partial class FrmSettings : Form
{
    public FrmSettings()
    {
        InitializeComponent();
        LoadSettingsData();
        UpdateButtonStates();
    }

    #region Load

    private void LoadSettingsData()
    {
        LoadDgvPaths();
        LoadDgvIgnoreRules();
        UpdateButtonStates();
        LoadGeneralConfig();
    }

    private void LoadDgvPaths()
    {
        dgvPaths.Rows.Clear();
        foreach (var pathInfo in Program.PathSetting.PathInfo)
        {
            dgvPaths.Rows.Add(pathInfo.Path, pathInfo.Depth.ToString());
        }
    }

    private void LoadDgvIgnoreRules()
    {
        cmbIgnoreTypeInput.DataSource = Enum.GetValues<IgnoreType>();
        if (dgvIgnoreRules.Columns["colIgnoreType"] is DataGridViewComboBoxColumn comboBoxColumn)
        {
            comboBoxColumn.DataSource = Enum.GetValues<IgnoreType>();
            comboBoxColumn.ValueType = typeof(IgnoreType);
        }

        dgvIgnoreRules.Rows.Clear();
        foreach (var ignoreInfo in Program.PathSetting.IgnoreList)
        {
            var rowIndex = dgvIgnoreRules.Rows.Add(ignoreInfo.Name);
            dgvIgnoreRules.Rows[rowIndex].Cells["colIgnoreType"].Value = ignoreInfo.IgnoreType;
        }
    }

    private void LoadGeneralConfig()
    {
        numMaxParallel.Value = Properties.Settings.Default.MaxParallel;
        
        // Add logging configuration tab if it doesn't exist
        if (!tabControlSettings.TabPages.Cast<TabPage>().Any(tp => tp.Name == "tabPageLogging"))
        {
            CreateLoggingTab();
        }
        
        LoadLoggingConfig();
    }

    private TabPage? _tabPageLogging;
    private NumericUpDown? _numUIUpdateInterval;
    private NumericUpDown? _numFileFlushInterval;
    private NumericUpDown? _numBatchSize;

    private void CreateLoggingTab()
    {
        _tabPageLogging = new TabPage
        {
            Name = "tabPageLogging",
            Text = "Logging",
            UseVisualStyleBackColor = true,
            Padding = new Padding(3)
        };

        var tableLayoutPanel = new TableLayoutPanel
        {
            AutoSize = true,
            ColumnCount = 2,
            Dock = DockStyle.Top,
            RowCount = 3
        };

        tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
        tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        
        for (int i = 0; i < 3; i++)
        {
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        }

        // UI Update Interval
        var lblUIUpdate = new Label
        {
            Text = "UI Update Interval (ms):",
            Anchor = AnchorStyles.Left,
            AutoSize = true
        };
        _numUIUpdateInterval = new NumericUpDown
        {
            Minimum = 50,
            Maximum = 5000,
            Value = Properties.Settings.Default.LogUIUpdateInterval,
            Dock = DockStyle.Fill
        };

        // File Flush Interval
        var lblFileFlush = new Label
        {
            Text = "File Flush Interval (ms):",
            Anchor = AnchorStyles.Left,
            AutoSize = true
        };
        _numFileFlushInterval = new NumericUpDown
        {
            Minimum = 1000,
            Maximum = 60000,
            Value = Properties.Settings.Default.LogFileFlushInterval,
            Dock = DockStyle.Fill
        };

        // Batch Size
        var lblBatchSize = new Label
        {
            Text = "Batch Size:",
            Anchor = AnchorStyles.Left,
            AutoSize = true
        };
        _numBatchSize = new NumericUpDown
        {
            Minimum = 10,
            Maximum = 500,
            Value = Properties.Settings.Default.LogBatchSize,
            Dock = DockStyle.Fill
        };

        tableLayoutPanel.Controls.Add(lblUIUpdate, 0, 0);
        tableLayoutPanel.Controls.Add(_numUIUpdateInterval, 1, 0);
        tableLayoutPanel.Controls.Add(lblFileFlush, 0, 1);
        tableLayoutPanel.Controls.Add(_numFileFlushInterval, 1, 1);
        tableLayoutPanel.Controls.Add(lblBatchSize, 0, 2);
        tableLayoutPanel.Controls.Add(_numBatchSize, 1, 2);

        _tabPageLogging.Controls.Add(tableLayoutPanel);
        tabControlSettings.TabPages.Add(_tabPageLogging);
    }

    private void LoadLoggingConfig()
    {
        if (_numUIUpdateInterval != null)
            _numUIUpdateInterval.Value = Properties.Settings.Default.LogUIUpdateInterval;
        if (_numFileFlushInterval != null)
            _numFileFlushInterval.Value = Properties.Settings.Default.LogFileFlushInterval;
        if (_numBatchSize != null)
            _numBatchSize.Value = Properties.Settings.Default.LogBatchSize;
    }

    #endregion Load

    #region Events

    private void BtnSaveSettings_Click(object sender, EventArgs e)
    {
        Program.PathSetting.PathInfo.Clear();
        foreach (DataGridViewRow row in dgvPaths.Rows)
        {
            if (row.IsNewRow) continue;
            var path = row.Cells["colPath"].Value?.ToString() ?? "";
            var depth = int.TryParse(row.Cells["colDepth"].Value?.ToString(), out var d) ? d : 0;
            if (!string.IsNullOrWhiteSpace(path))
            {
                Program.PathSetting.PathInfo.Add(new PathInfo(path, depth));
            }
        }

        Program.PathSetting.IgnoreList.Clear();
        var ignoreTypeComboBoxColumn = dgvIgnoreRules.Columns["colIgnoreType"] as DataGridViewComboBoxColumn;

        var defaultIgnoreType = IgnoreType.Contains; // Ultimate fallback
        if (ignoreTypeComboBoxColumn?.Items.Count > 0)
        {
            if (ignoreTypeComboBoxColumn.Items[0] is IgnoreType firstItem)
            {
                defaultIgnoreType = firstItem;
            }
            else if (Enum.TryParse(ignoreTypeComboBoxColumn.Items[0]?.ToString(), out IgnoreType parsedFirstItem))
            {
                defaultIgnoreType = parsedFirstItem;
            }
        }

        foreach (DataGridViewRow row in dgvIgnoreRules.Rows)
        {
            if (row.IsNewRow) continue;
            var name = row.Cells["colIgnoreName"].Value?.ToString() ?? "";
            var type = defaultIgnoreType;

            if (row.Cells["colIgnoreType"].Value != null)
            {
                if (row.Cells["colIgnoreType"].Value is IgnoreType cellValue)
                {
                    type = cellValue;
                }
                else if (Enum.TryParse(row.Cells["colIgnoreType"].Value?.ToString(), out IgnoreType parsedType))
                {
                    type = parsedType;
                }
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                Program.PathSetting.IgnoreList.Add(new IgnoreInfo { Name = name, IgnoreType = type });
            }
        }

        Program.PathSetting.Save();

        Properties.Settings.Default.MaxParallel = (int)numMaxParallel.Value;
        
        // Save logging settings if they exist
        if (_numUIUpdateInterval != null)
            Properties.Settings.Default.LogUIUpdateInterval = (int)_numUIUpdateInterval.Value;
        if (_numFileFlushInterval != null)
            Properties.Settings.Default.LogFileFlushInterval = (int)_numFileFlushInterval.Value;
        if (_numBatchSize != null)
            Properties.Settings.Default.LogBatchSize = (int)_numBatchSize.Value;
            
        Properties.Settings.Default.Save();

        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCancelSettings_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    #endregion Events

    #region Path CRUD Logic

    private void BtnDirectBrowsePath_Click(object sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog();
        dialog.ShowNewFolderButton = true;

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            txtPathInput.Text = dialog.SelectedPath;
        }
    }

    private void BtnAddPath_Click(object sender, EventArgs e)
    {
        var path = txtPathInput.Text.Trim();
        var depth = (int)numDepthInput.Value;

        if (string.IsNullOrWhiteSpace(path))
        {
            MessageBox.Show(@"Path cannot be empty.", @"Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        dgvPaths.Rows.Add(path, depth.ToString());

        txtPathInput.Clear();
        numDepthInput.Value = 0; // Reset to default
    }

    private void BtnEditPath_Click(object sender, EventArgs e)
    {
        if (dgvPaths.SelectedRows.Count > 0)
        {
            var selectedRow = dgvPaths.SelectedRows[0];
            var path = txtPathInput.Text.Trim();
            var depth = (int)numDepthInput.Value;

            if (string.IsNullOrWhiteSpace(path))
            {
                MessageBox.Show(@"Path cannot be empty.", @"Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            selectedRow.Cells["colPath"].Value = path;
            selectedRow.Cells["colDepth"].Value = depth.ToString();
        }
        else
        {
            MessageBox.Show(@"Please select a path to edit.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void BtnRemovePath_Click(object sender, EventArgs e)
    {
        if (dgvPaths.SelectedRows.Count > 0)
        {
            foreach (DataGridViewRow row in dgvPaths.SelectedRows)
            {
                dgvPaths.Rows.Remove(row);
            }
        }
        else
        {
            MessageBox.Show(@"Please select a path to remove.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void DgvPaths_SelectionChanged(object sender, EventArgs e)
    {
        if (dgvPaths.SelectedRows.Count > 0)
        {
            var selectedRow = dgvPaths.SelectedRows[0];
            txtPathInput.Text = selectedRow.Cells["colPath"].Value?.ToString() ?? "";
            numDepthInput.Value = int.TryParse(selectedRow.Cells["colDepth"].Value?.ToString(), out var depth) ?
                Math.Max(numDepthInput.Minimum, Math.Min(depth, numDepthInput.Maximum)) :
                numDepthInput.Minimum; // Default if parsing fails
        }

        UpdateButtonStates();
    }

    #endregion Path CRUD Logic

    #region Ignore Rule CRUD Logic

    private void BtnAddIgnoreRule_Click(object sender, EventArgs e)
    {
        var pattern = txtIgnorePatternInput.Text.Trim();
        var type = cmbIgnoreTypeInput.SelectedValue is IgnoreType selectedType ? selectedType : IgnoreType.Contains;

        if (string.IsNullOrWhiteSpace(pattern))
        {
            MessageBox.Show(@"Pattern cannot be empty.", @"Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var rowIndex = dgvIgnoreRules.Rows.Add(pattern);
        dgvIgnoreRules.Rows[rowIndex].Cells["colIgnoreType"].Value = type;

        txtIgnorePatternInput.Clear();
        if (cmbIgnoreTypeInput.Items.Count > 0)
            cmbIgnoreTypeInput.SelectedIndex = 0; // Reset to default
    }

    private void BtnEditIgnoreRule_Click(object sender, EventArgs e)
    {
        if (dgvIgnoreRules.SelectedRows.Count > 0)
        {
            var selectedRow = dgvIgnoreRules.SelectedRows[0];
            var pattern = txtIgnorePatternInput.Text.Trim();
            var type = cmbIgnoreTypeInput.SelectedValue is IgnoreType selectedType ? selectedType : IgnoreType.Contains;

            if (string.IsNullOrWhiteSpace(pattern))
            {
                MessageBox.Show(@"Pattern cannot be empty.", @"Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            selectedRow.Cells["colIgnoreName"].Value = pattern;
            selectedRow.Cells["colIgnoreType"].Value = type;
        }
        else
        {
            MessageBox.Show(@"Please select an ignore rule to edit.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void BtnRemoveIgnoreRule_Click(object sender, EventArgs e)
    {
        if (dgvIgnoreRules.SelectedRows.Count > 0)
        {
            foreach (DataGridViewRow row in dgvIgnoreRules.SelectedRows)
            {
                dgvIgnoreRules.Rows.Remove(row);
            }
        }
        else
        {
            MessageBox.Show(@"Please select an ignore rule to remove.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void DgvIgnoreRules_SelectionChanged(object sender, EventArgs e)
    {
        if (dgvIgnoreRules.SelectedRows.Count > 0)
        {
            var selectedRow = dgvIgnoreRules.SelectedRows[0];
            txtIgnorePatternInput.Text = selectedRow.Cells["colIgnoreName"].Value?.ToString() ?? "";
            if (selectedRow.Cells["colIgnoreType"].Value is IgnoreType ignoreType)
            {
                cmbIgnoreTypeInput.SelectedItem = ignoreType;
            }
            else if (selectedRow.Cells["colIgnoreType"].Value != null && Enum.TryParse<IgnoreType>(selectedRow.Cells["colIgnoreType"].Value?.ToString(), out var parsedIgnoreType))
            {
                cmbIgnoreTypeInput.SelectedItem = parsedIgnoreType;
            }
            else if (cmbIgnoreTypeInput.Items.Count > 0)
            {
                cmbIgnoreTypeInput.SelectedIndex = 0; // Default
            }
        }
        UpdateButtonStates();
    }

    #endregion Ignore Rule CRUD Logic

    #region UI Helper

    private void UpdateButtonStates()
    {
        var pathSelected = dgvPaths.SelectedRows.Count > 0;
        btnEditPath.Enabled = pathSelected;
        btnRemovePath.Enabled = pathSelected;

        var ignoreRuleSelected = dgvIgnoreRules.SelectedRows.Count > 0;
        btnEditIgnoreRule.Enabled = ignoreRuleSelected;
        btnRemoveIgnoreRule.Enabled = ignoreRuleSelected;
    }

    #endregion UI Helper
}