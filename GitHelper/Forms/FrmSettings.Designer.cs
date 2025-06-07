namespace GitHelper.Forms
{
    partial class FrmSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControlSettings = new TabControl();
            tabPagePaths = new TabPage();
            dgvPaths = new DataGridView();
            colPath = new DataGridViewTextBoxColumn();
            colDepth = new DataGridViewTextBoxColumn();
            panelPathButtons = new Panel();
            btnRemovePath = new Button();
            btnEditPath = new Button();
            btnAddPath = new Button();
            tlpPathInputs = new TableLayoutPanel();
            lblPath = new Label();
            txtPathInput = new TextBox();
            btnDirectBrowsePath = new Button();
            lblDepth = new Label();
            numDepthInput = new NumericUpDown();
            tabPageIgnoreRules = new TabPage();
            dgvIgnoreRules = new DataGridView();
            colIgnoreName = new DataGridViewTextBoxColumn();
            colIgnoreType = new DataGridViewComboBoxColumn();
            panelIgnoreButtons = new Panel();
            btnRemoveIgnoreRule = new Button();
            btnEditIgnoreRule = new Button();
            btnAddIgnoreRule = new Button();
            tlpIgnoreInputs = new TableLayoutPanel();
            lblPattern = new Label();
            txtIgnorePatternInput = new TextBox();
            lblIgnoreType = new Label();
            cmbIgnoreTypeInput = new ComboBox();
            panelMainButtons = new Panel();
            btnCancelSettings = new Button();
            btnSaveSettings = new Button();
            tabControlSettings.SuspendLayout();
            tabPagePaths.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPaths).BeginInit();
            panelPathButtons.SuspendLayout();
            tlpPathInputs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDepthInput).BeginInit();
            tabPageIgnoreRules.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvIgnoreRules).BeginInit();
            panelIgnoreButtons.SuspendLayout();
            tlpIgnoreInputs.SuspendLayout();
            panelMainButtons.SuspendLayout();
            SuspendLayout();
            // 
            // tabControlSettings
            // 
            tabControlSettings.Controls.Add(tabPagePaths);
            tabControlSettings.Controls.Add(tabPageIgnoreRules);
            tabControlSettings.Dock = DockStyle.Fill;
            tabControlSettings.Location = new Point(0, 0);
            tabControlSettings.Name = "tabControlSettings";
            tabControlSettings.SelectedIndex = 0;
            tabControlSettings.Size = new Size(780, 410);
            tabControlSettings.TabIndex = 0;
            // 
            // tabPagePaths
            // 
            tabPagePaths.Controls.Add(dgvPaths);
            tabPagePaths.Controls.Add(panelPathButtons);
            tabPagePaths.Controls.Add(tlpPathInputs);
            tabPagePaths.Location = new Point(4, 24);
            tabPagePaths.Name = "tabPagePaths";
            tabPagePaths.Padding = new Padding(3);
            tabPagePaths.Size = new Size(772, 382);
            tabPagePaths.TabIndex = 0;
            tabPagePaths.Text = "Paths";
            tabPagePaths.UseVisualStyleBackColor = true;
            // 
            // dgvPaths
            // 
            dgvPaths.AllowUserToAddRows = false;
            dgvPaths.AllowUserToDeleteRows = false;
            dgvPaths.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPaths.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPaths.Columns.AddRange(new DataGridViewColumn[] { colPath, colDepth });
            dgvPaths.Dock = DockStyle.Fill;
            dgvPaths.Location = new Point(3, 63);
            dgvPaths.Name = "dgvPaths";
            dgvPaths.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPaths.Size = new Size(766, 276);
            dgvPaths.TabIndex = 1;
            dgvPaths.SelectionChanged += DgvPaths_SelectionChanged;
            // 
            // colPath
            // 
            colPath.HeaderText = "Path";
            colPath.Name = "colPath";
            colPath.ReadOnly = true;
            // 
            // colDepth
            // 
            colDepth.HeaderText = "Depth";
            colDepth.Name = "colDepth";
            colDepth.ReadOnly = true;
            // 
            // panelPathButtons
            // 
            panelPathButtons.Controls.Add(btnRemovePath);
            panelPathButtons.Controls.Add(btnEditPath);
            panelPathButtons.Controls.Add(btnAddPath);
            panelPathButtons.Dock = DockStyle.Bottom;
            panelPathButtons.Location = new Point(3, 339);
            panelPathButtons.Name = "panelPathButtons";
            panelPathButtons.Size = new Size(766, 40);
            panelPathButtons.TabIndex = 0;
            // 
            // btnRemovePath
            // 
            btnRemovePath.Location = new Point(168, 8);
            btnRemovePath.Name = "btnRemovePath";
            btnRemovePath.Size = new Size(75, 23);
            btnRemovePath.TabIndex = 2;
            btnRemovePath.Text = "Remove";
            btnRemovePath.UseVisualStyleBackColor = true;
            btnRemovePath.Click += BtnRemovePath_Click;
            // 
            // btnEditPath
            // 
            btnEditPath.Location = new Point(87, 8);
            btnEditPath.Name = "btnEditPath";
            btnEditPath.Size = new Size(75, 23);
            btnEditPath.TabIndex = 1;
            btnEditPath.Text = "Edit Selected";
            btnEditPath.UseVisualStyleBackColor = true;
            btnEditPath.Click += BtnEditPath_Click;
            // 
            // btnAddPath
            // 
            btnAddPath.Location = new Point(6, 8);
            btnAddPath.Name = "btnAddPath";
            btnAddPath.Size = new Size(75, 23);
            btnAddPath.TabIndex = 0;
            btnAddPath.Text = "Add New";
            btnAddPath.UseVisualStyleBackColor = true;
            btnAddPath.Click += BtnAddPath_Click;
            // 
            // tlpPathInputs
            // 
            tlpPathInputs.AutoSize = true;
            tlpPathInputs.ColumnCount = 3;
            tlpPathInputs.ColumnStyles.Add(new ColumnStyle());
            tlpPathInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPathInputs.ColumnStyles.Add(new ColumnStyle());
            tlpPathInputs.Controls.Add(lblPath, 0, 0);
            tlpPathInputs.Controls.Add(txtPathInput, 1, 0);
            tlpPathInputs.Controls.Add(btnDirectBrowsePath, 2, 0);
            tlpPathInputs.Controls.Add(lblDepth, 0, 1);
            tlpPathInputs.Controls.Add(numDepthInput, 1, 1);
            tlpPathInputs.Dock = DockStyle.Top;
            tlpPathInputs.Location = new Point(3, 3);
            tlpPathInputs.Name = "tlpPathInputs";
            tlpPathInputs.RowCount = 2;
            tlpPathInputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpPathInputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpPathInputs.Size = new Size(766, 60);
            tlpPathInputs.TabIndex = 2;
            // 
            // lblPath
            // 
            lblPath.Anchor = AnchorStyles.Left;
            lblPath.AutoSize = true;
            lblPath.Location = new Point(3, 7);
            lblPath.Name = "lblPath";
            lblPath.Size = new Size(34, 15);
            lblPath.TabIndex = 0;
            lblPath.Text = "Path:";
            // 
            // txtPathInput
            // 
            txtPathInput.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtPathInput.Location = new Point(51, 3);
            txtPathInput.Name = "txtPathInput";
            txtPathInput.Size = new Size(662, 23);
            txtPathInput.TabIndex = 1;
            // 
            // btnDirectBrowsePath
            // 
            btnDirectBrowsePath.Anchor = AnchorStyles.Left;
            btnDirectBrowsePath.Location = new Point(719, 3);
            btnDirectBrowsePath.Name = "btnDirectBrowsePath";
            btnDirectBrowsePath.Size = new Size(44, 23);
            btnDirectBrowsePath.TabIndex = 2;
            btnDirectBrowsePath.Text = "...";
            btnDirectBrowsePath.UseVisualStyleBackColor = true;
            btnDirectBrowsePath.Click += BtnDirectBrowsePath_Click;
            // 
            // lblDepth
            // 
            lblDepth.Anchor = AnchorStyles.Left;
            lblDepth.AutoSize = true;
            lblDepth.Location = new Point(3, 37);
            lblDepth.Name = "lblDepth";
            lblDepth.Size = new Size(42, 15);
            lblDepth.TabIndex = 3;
            lblDepth.Text = "Depth:";
            // 
            // numDepthInput
            // 
            numDepthInput.Anchor = AnchorStyles.Left;
            numDepthInput.Location = new Point(51, 33);
            numDepthInput.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numDepthInput.Name = "numDepthInput";
            numDepthInput.Size = new Size(120, 23);
            numDepthInput.TabIndex = 4;
            // 
            // tabPageIgnoreRules
            // 
            tabPageIgnoreRules.Controls.Add(dgvIgnoreRules);
            tabPageIgnoreRules.Controls.Add(panelIgnoreButtons);
            tabPageIgnoreRules.Controls.Add(tlpIgnoreInputs);
            tabPageIgnoreRules.Location = new Point(4, 24);
            tabPageIgnoreRules.Name = "tabPageIgnoreRules";
            tabPageIgnoreRules.Padding = new Padding(3);
            tabPageIgnoreRules.Size = new Size(772, 382);
            tabPageIgnoreRules.TabIndex = 1;
            tabPageIgnoreRules.Text = "Ignore Rules";
            tabPageIgnoreRules.UseVisualStyleBackColor = true;
            // 
            // dgvIgnoreRules
            // 
            dgvIgnoreRules.AllowUserToAddRows = false;
            dgvIgnoreRules.AllowUserToDeleteRows = false;
            dgvIgnoreRules.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvIgnoreRules.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvIgnoreRules.Columns.AddRange(new DataGridViewColumn[] { colIgnoreName, colIgnoreType });
            dgvIgnoreRules.Dock = DockStyle.Fill;
            dgvIgnoreRules.Location = new Point(3, 63);
            dgvIgnoreRules.Name = "dgvIgnoreRules";
            dgvIgnoreRules.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvIgnoreRules.Size = new Size(766, 276);
            dgvIgnoreRules.TabIndex = 1;
            dgvIgnoreRules.SelectionChanged += DgvIgnoreRules_SelectionChanged;
            // 
            // colIgnoreName
            // 
            colIgnoreName.HeaderText = "Pattern";
            colIgnoreName.Name = "colIgnoreName";
            colIgnoreName.ReadOnly = true;
            // 
            // colIgnoreType
            // 
            colIgnoreType.HeaderText = "Type";
            colIgnoreType.Name = "colIgnoreType";
            colIgnoreType.ReadOnly = true;
            // 
            // panelIgnoreButtons
            // 
            panelIgnoreButtons.Controls.Add(btnRemoveIgnoreRule);
            panelIgnoreButtons.Controls.Add(btnEditIgnoreRule);
            panelIgnoreButtons.Controls.Add(btnAddIgnoreRule);
            panelIgnoreButtons.Dock = DockStyle.Bottom;
            panelIgnoreButtons.Location = new Point(3, 339);
            panelIgnoreButtons.Name = "panelIgnoreButtons";
            panelIgnoreButtons.Size = new Size(766, 40);
            panelIgnoreButtons.TabIndex = 0;
            // 
            // btnRemoveIgnoreRule
            // 
            btnRemoveIgnoreRule.Location = new Point(168, 8);
            btnRemoveIgnoreRule.Name = "btnRemoveIgnoreRule";
            btnRemoveIgnoreRule.Size = new Size(75, 23);
            btnRemoveIgnoreRule.TabIndex = 2;
            btnRemoveIgnoreRule.Text = "Remove";
            btnRemoveIgnoreRule.UseVisualStyleBackColor = true;
            btnRemoveIgnoreRule.Click += BtnRemoveIgnoreRule_Click;
            // 
            // btnEditIgnoreRule
            // 
            btnEditIgnoreRule.Location = new Point(87, 8);
            btnEditIgnoreRule.Name = "btnEditIgnoreRule";
            btnEditIgnoreRule.Size = new Size(75, 23);
            btnEditIgnoreRule.TabIndex = 1;
            btnEditIgnoreRule.Text = "Edit Selected";
            btnEditIgnoreRule.UseVisualStyleBackColor = true;
            btnEditIgnoreRule.Click += BtnEditIgnoreRule_Click;
            // 
            // btnAddIgnoreRule
            // 
            btnAddIgnoreRule.Location = new Point(6, 8);
            btnAddIgnoreRule.Name = "btnAddIgnoreRule";
            btnAddIgnoreRule.Size = new Size(75, 23);
            btnAddIgnoreRule.TabIndex = 0;
            btnAddIgnoreRule.Text = "Add New";
            btnAddIgnoreRule.UseVisualStyleBackColor = true;
            btnAddIgnoreRule.Click += BtnAddIgnoreRule_Click;
            // 
            // tlpIgnoreInputs
            // 
            tlpIgnoreInputs.AutoSize = true;
            tlpIgnoreInputs.ColumnCount = 2;
            tlpIgnoreInputs.ColumnStyles.Add(new ColumnStyle());
            tlpIgnoreInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpIgnoreInputs.Controls.Add(lblPattern, 0, 0);
            tlpIgnoreInputs.Controls.Add(txtIgnorePatternInput, 1, 0);
            tlpIgnoreInputs.Controls.Add(lblIgnoreType, 0, 1);
            tlpIgnoreInputs.Controls.Add(cmbIgnoreTypeInput, 1, 1);
            tlpIgnoreInputs.Dock = DockStyle.Top;
            tlpIgnoreInputs.Location = new Point(3, 3);
            tlpIgnoreInputs.Name = "tlpIgnoreInputs";
            tlpIgnoreInputs.RowCount = 2;
            tlpIgnoreInputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpIgnoreInputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpIgnoreInputs.Size = new Size(766, 60);
            tlpIgnoreInputs.TabIndex = 2;
            // 
            // lblPattern
            // 
            lblPattern.Anchor = AnchorStyles.Left;
            lblPattern.AutoSize = true;
            lblPattern.Location = new Point(3, 7);
            lblPattern.Name = "lblPattern";
            lblPattern.Size = new Size(48, 15);
            lblPattern.TabIndex = 0;
            lblPattern.Text = "Pattern:";
            // 
            // txtIgnorePatternInput
            // 
            txtIgnorePatternInput.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtIgnorePatternInput.Location = new Point(57, 3);
            txtIgnorePatternInput.Name = "txtIgnorePatternInput";
            txtIgnorePatternInput.Size = new Size(706, 23);
            txtIgnorePatternInput.TabIndex = 1;
            // 
            // lblIgnoreType
            // 
            lblIgnoreType.Anchor = AnchorStyles.Left;
            lblIgnoreType.AutoSize = true;
            lblIgnoreType.Location = new Point(3, 37);
            lblIgnoreType.Name = "lblIgnoreType";
            lblIgnoreType.Size = new Size(34, 15);
            lblIgnoreType.TabIndex = 2;
            lblIgnoreType.Text = "Type:";
            // 
            // cmbIgnoreTypeInput
            // 
            cmbIgnoreTypeInput.Anchor = AnchorStyles.Left;
            cmbIgnoreTypeInput.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbIgnoreTypeInput.FormattingEnabled = true;
            cmbIgnoreTypeInput.Location = new Point(57, 33);
            cmbIgnoreTypeInput.Name = "cmbIgnoreTypeInput";
            cmbIgnoreTypeInput.Size = new Size(200, 23);
            cmbIgnoreTypeInput.TabIndex = 3;
            // 
            // panelMainButtons
            // 
            panelMainButtons.Controls.Add(btnCancelSettings);
            panelMainButtons.Controls.Add(btnSaveSettings);
            panelMainButtons.Dock = DockStyle.Bottom;
            panelMainButtons.Location = new Point(0, 410);
            panelMainButtons.Name = "panelMainButtons";
            panelMainButtons.Size = new Size(780, 40);
            panelMainButtons.TabIndex = 1;
            // 
            // btnCancelSettings
            // 
            btnCancelSettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancelSettings.Location = new Point(693, 8);
            btnCancelSettings.Name = "btnCancelSettings";
            btnCancelSettings.Size = new Size(75, 23);
            btnCancelSettings.TabIndex = 1;
            btnCancelSettings.Text = "Cancel";
            btnCancelSettings.UseVisualStyleBackColor = true;
            btnCancelSettings.Click += BtnCancelSettings_Click;
            // 
            // btnSaveSettings
            // 
            btnSaveSettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveSettings.Location = new Point(612, 8);
            btnSaveSettings.Name = "btnSaveSettings";
            btnSaveSettings.Size = new Size(75, 23);
            btnSaveSettings.TabIndex = 0;
            btnSaveSettings.Text = "Save";
            btnSaveSettings.UseVisualStyleBackColor = true;
            btnSaveSettings.Click += BtnSaveSettings_Click;
            // 
            // FrmSettings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(780, 450);
            Controls.Add(tabControlSettings);
            Controls.Add(panelMainButtons);
            Name = "FrmSettings";
            Text = "Settings";
            tabControlSettings.ResumeLayout(false);
            tabPagePaths.ResumeLayout(false);
            tabPagePaths.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPaths).EndInit();
            panelPathButtons.ResumeLayout(false);
            tlpPathInputs.ResumeLayout(false);
            tlpPathInputs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDepthInput).EndInit();
            tabPageIgnoreRules.ResumeLayout(false);
            tabPageIgnoreRules.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvIgnoreRules).EndInit();
            panelIgnoreButtons.ResumeLayout(false);
            tlpIgnoreInputs.ResumeLayout(false);
            tlpIgnoreInputs.PerformLayout();
            panelMainButtons.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlSettings;
        private System.Windows.Forms.TabPage tabPagePaths;
        private System.Windows.Forms.DataGridView dgvPaths;
        private System.Windows.Forms.Panel panelPathButtons;
        private System.Windows.Forms.TabPage tabPageIgnoreRules;
        private System.Windows.Forms.Button btnAddPath;
        private System.Windows.Forms.Button btnEditPath;
        private System.Windows.Forms.Button btnRemovePath;
        // Removed btnBrowsePath
        private System.Windows.Forms.DataGridView dgvIgnoreRules;
        private System.Windows.Forms.Panel panelIgnoreButtons;
        private System.Windows.Forms.Button btnAddIgnoreRule;
        private System.Windows.Forms.Button btnEditIgnoreRule;
        private System.Windows.Forms.Button btnRemoveIgnoreRule;
        private System.Windows.Forms.Panel panelMainButtons;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnCancelSettings;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDepth;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIgnoreName;
        private System.Windows.Forms.DataGridViewComboBoxColumn colIgnoreType;
        private System.Windows.Forms.TableLayoutPanel tlpPathInputs;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.TextBox txtPathInput;
        private System.Windows.Forms.Button btnDirectBrowsePath;
        private System.Windows.Forms.Label lblDepth;
        private System.Windows.Forms.NumericUpDown numDepthInput;
        private System.Windows.Forms.TableLayoutPanel tlpIgnoreInputs;
        private System.Windows.Forms.Label lblPattern;
        private System.Windows.Forms.TextBox txtIgnorePatternInput;
        private System.Windows.Forms.Label lblIgnoreType;
        private System.Windows.Forms.ComboBox cmbIgnoreTypeInput;
    }
}