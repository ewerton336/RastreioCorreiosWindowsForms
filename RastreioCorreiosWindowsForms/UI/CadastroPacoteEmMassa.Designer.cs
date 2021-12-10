
namespace RastreioCorreiosWindowsForms.UI
{
    partial class CadastroPacoteEmMassa
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
            this.caixaTexto = new System.Windows.Forms.TextBox();
            this.botaoCadastrar = new System.Windows.Forms.Button();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.checkPacoteClientes = new DevExpress.XtraEditors.CheckEdit();
            this.textoDescricao = new System.Windows.Forms.TextBox();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.checkPacoteClientes.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // caixaTexto
            // 
            this.caixaTexto.Location = new System.Drawing.Point(148, 116);
            this.caixaTexto.Multiline = true;
            this.caixaTexto.Name = "caixaTexto";
            this.caixaTexto.Size = new System.Drawing.Size(394, 222);
            this.caixaTexto.TabIndex = 0;
            // 
            // botaoCadastrar
            // 
            this.botaoCadastrar.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.botaoCadastrar.Location = new System.Drawing.Point(412, 473);
            this.botaoCadastrar.Name = "botaoCadastrar";
            this.botaoCadastrar.Size = new System.Drawing.Size(106, 35);
            this.botaoCadastrar.TabIndex = 2;
            this.botaoCadastrar.Text = "Cadastrar";
            this.botaoCadastrar.UseVisualStyleBackColor = true;
            this.botaoCadastrar.Click += new System.EventHandler(this.botaoCadastrar_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 12.25F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(174, 73);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(344, 19);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "Insira os códigos de rastreio separados por linha";
            // 
            // checkPacoteClientes
            // 
            this.checkPacoteClientes.Location = new System.Drawing.Point(148, 471);
            this.checkPacoteClientes.Name = "checkPacoteClientes";
            this.checkPacoteClientes.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12.25F);
            this.checkPacoteClientes.Properties.Appearance.Options.UseFont = true;
            this.checkPacoteClientes.Properties.Caption = "Pacote de clientes";
            this.checkPacoteClientes.Size = new System.Drawing.Size(144, 23);
            this.checkPacoteClientes.TabIndex = 4;
            // 
            // textoDescricao
            // 
            this.textoDescricao.Location = new System.Drawing.Point(148, 373);
            this.textoDescricao.Multiline = true;
            this.textoDescricao.Name = "textoDescricao";
            this.textoDescricao.Size = new System.Drawing.Size(360, 32);
            this.textoDescricao.TabIndex = 5;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 12.25F);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(148, 348);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(67, 19);
            this.labelControl2.TabIndex = 6;
            this.labelControl2.Text = "Descrição";
            // 
            // CadastroPacoteEmMassa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 549);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.textoDescricao);
            this.Controls.Add(this.checkPacoteClientes);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.botaoCadastrar);
            this.Controls.Add(this.caixaTexto);
            this.Name = "CadastroPacoteEmMassa";
            this.Text = "CadastroPacoteEmMassa";
            ((System.ComponentModel.ISupportInitialize)(this.checkPacoteClientes.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox caixaTexto;
        private System.Windows.Forms.Button botaoCadastrar;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit checkPacoteClientes;
        private System.Windows.Forms.TextBox textoDescricao;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}