using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace Proyecto6
{
    public partial class fmEditor : Form
    {
        public fmEditor()
        {

            InitializeComponent();
            Application.Idle += AplicacionOciosa;
        }

        private void AplicacionOciosa(object sender, EventArgs e)
        {
            int linea = rtbEditor.GetLineFromCharIndex(rtbEditor.SelectionStart);
            int columna = rtbEditor.SelectionStart - rtbEditor.GetFirstCharIndexOfCurrentLine();

            stEstadoEditor.Items[1].Text = "Lín." + Convert.ToString(linea + 1) +
            " Col." + Convert.ToString(columna);

            stEstadoEditor.Items[2].Text = "Car. " + Convert.ToString(rtbEditor.SelectionStart);

            bool HaySeleccion = rtbEditor.SelectionLength > 0;
            if (HaySeleccion)
            {
                stEstadoEditor.Items[0].Text = "Hay Texto seleccionado";
            }

            tsbCopiar.Enabled = HaySeleccion;
            tsbCortar.Enabled = HaySeleccion;

            tsbDeshacer.Enabled = rtbEditor.CanUndo;
            tsbRehacer.Enabled = rtbEditor.CanRedo;

            tsbPegar.Enabled = Clipboard.ContainsText();

            itBorrar.Enabled = HaySeleccion;
            itPegar.Enabled = tsbPegar.Enabled;
            itCopiar.Enabled = tsbCopiar.Enabled;
            itCortar.Enabled = tsbCortar.Enabled;
            itDeshacer.Enabled = tsbDeshacer.Enabled;
            itRehacer.Enabled = tsbRehacer.Enabled;
        }

        private void tamanyosestado() {
            stEstadoEditor.Items[0].Width = Width - 350;
            stEstadoEditor.Items[1].Width = 100;
            stEstadoEditor.Items[2].Width = 60;
            stEstadoEditor.Items[3].Width = 70;
            stEstadoEditor.Items[4].Width = 90;
        }

        private void fmEditor_Resize(object sender, EventArgs e)
        {
            tamanyosestado();
        }

        private void fmEditor_Load(object sender, EventArgs e)
        {
            tamanyosestado();
            foreach (FontFamily misfuentes in FontFamily.Families)
            {
                cbFuentes.Items.Add(misfuentes.Name);
            }
            Text = "Documento1";

            rtbEditor.ClearUndo();
            itQuitarFormato.PerformClick();

            rtbEditor.Modified = false;
            rtbEditor.Focus();
        }

        private void itQuitarFormatos_Click(object sender, EventArgs e) {
            itIzquierda.Checked = true;
            tsbAlineacionIzq.Checked = true;
            tsbNegrita.Checked = false;
            tsbCursiva.Checked = false;
            tsbSubrayado.Checked = false;
            tsbTachado.Checked = false;
            tsbColores.Checked = false;
            FontStyle estilo = new FontStyle();
            rtbEditor.SelectionFont = new Font("Arial", 10, estilo);
            rtbEditor.SelectionColor = Color.Black;
            rtbEditor.SelectionAlignment = HorizontalAlignment.Left;
            cbFuentes.SelectedIndex = cbFuentes.Items.IndexOf("Arial");
            cbTamanyo.Text = "10";
            rtbEditor.BackColor = Color.White;
            rtbEditor.SelectionRightIndent = 0;
            rtbEditor.SelectionIndent = 0;
            rtbEditor.SelectionBullet = false;
            itVinyetas.Checked = false;
        }

        private void timeEditor_Tick(object sender, EventArgs e)
        {
            DateTime fecha = DateTime.Now;
            stEstadoEditor.Items[3].Text = fecha.ToString("dd/MM/yyyy");
            stEstadoEditor.Items[4].Text = fecha.ToString("HH:mm:ss");
        }

        private void tsbNegrita_Click(object sender, EventArgs e)
        {
            FontStyle negrita = new FontStyle();
            FontStyle subrayado = new FontStyle();
            FontStyle tachado = new FontStyle();
            FontStyle cursiva = new FontStyle();

            if (tsbNegrita.Checked)
            {
                negrita = FontStyle.Bold;
            }
            if (tsbSubrayado.Checked)
            {
                subrayado = FontStyle.Underline;
            }
            if (tsbTachado.Checked)
            {
                tachado = FontStyle.Strikeout;
            }
            if (tsbCursiva.Checked)
            {
                cursiva = FontStyle.Italic;
            }

            rtbEditor.SelectionFont = new Font(rtbEditor.SelectionFont, negrita | subrayado | tachado | cursiva);
            rtbEditor.Focus();
        }

        void desmarca()
        {
            tsbAlineacionIzq.Checked = false;
            tsbAlineacionCent.Checked = false;
            tsbAlineacionDer.Checked = false;
            for (int i = 0; i < itJustificacion.DropDownItems.Count; i++)
            {
                ((ToolStripMenuItem)itJustificacion.DropDownItems[i]).Checked = false;
            }
        }

        private void tsbAlineacion_Click(object sender, EventArgs e)
        {
            desmarca();

            if (sender.Equals(tsbAlineacionIzq) || sender.Equals(itIzquierda)) {
                tsbAlineacionIzq.Checked = true;
                itIzquierda.Checked = true;
                rtbEditor.SelectionAlignment = HorizontalAlignment.Left;
            } else if (sender.Equals(tsbAlineacionCent) || sender.Equals(itCentrado)) {
                tsbAlineacionCent.Checked = true;
                itCentrado.Checked = true;
                rtbEditor.SelectionAlignment = HorizontalAlignment.Center;
            } else if (sender.Equals(tsbAlineacionDer) || sender.Equals(itDerecha)) {
                tsbAlineacionDer.Checked = true;
                itDerecha.Checked = true;
                rtbEditor.SelectionAlignment = HorizontalAlignment.Right;
            }
        }

        private void cbFuentes_TextChanged(object sender, EventArgs e)
        {
            FontStyle estilo = new FontStyle();
            float tamanyo = 11;

            if (rtbEditor.SelectionFont != null)
            {
                estilo = rtbEditor.SelectionFont.Style;
                tamanyo = rtbEditor.SelectionFont.Size;
            }

            if (cbTamanyo.Text != "") {
                tamanyo = float.Parse(cbTamanyo.Text);
            }

            string fuente = cbFuentes.Text;

            rtbEditor.SelectionFont = new Font(fuente, tamanyo, estilo);
            rtbEditor.Focus();
        }

        private void cbTamanyo_TextChanged(object sender, EventArgs e)
        {
            FontStyle estilo = new FontStyle();
            string fuente = "";
            if (rtbEditor.SelectionFont != null) {
                estilo = rtbEditor.SelectionFont.Style;
                fuente = rtbEditor.SelectionFont.Name;
            }

            if (cbTamanyo.Text != "")
            {
                rtbEditor.SelectionFont = new Font(fuente, Convert.ToInt32(cbTamanyo.Text), estilo);
            }
        }

        private void cbTamanyo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // Intro
            {
                rtbEditor.Focus();
            }

            switch (e.KeyChar)
            {
                case (char)8:
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '0': break;
                default:
                    e.KeyChar = (char)0;
                    break;
            }
        }

        private void tsbColores_Click(object sender, EventArgs e)
        {
            dlgColor.Color = rtbEditor.SelectionColor;
            if (dlgColor.ShowDialog() == DialogResult.OK)
            {
                rtbEditor.SelectionColor = dlgColor.Color;
                rtbEditor.Modified = true;
            }
        }

        private void itColorFondo_Click(object sender, EventArgs e)
        {
            dlgColor.Color = rtbEditor.BackColor;
            if (dlgColor.ShowDialog() == DialogResult.OK)
            {
                rtbEditor.BackColor = dlgColor.Color;
                rtbEditor.Modified = true;
            }
        }

        private void itAbrir_Click(object sender, EventArgs e)
        {
            stEstadoEditor.Items[0].Text = "Abriendo Archivo de diferentes formatos";
            if (rtbEditor.Modified)
            {
                DialogResult resultado = MessageBox.Show("Hay Cambios sin Guardar. Guardas? ", "Guardar Cambios", MessageBoxButtons.YesNoCancel);
                switch (resultado)
                {
                    case DialogResult.Yes:
                        itGuardar.PerformClick();
                        break;
                    case DialogResult.Cancel:
                        rtbEditor.Focus();
                        return;
                }
            }
            if (dlgAbrir.ShowDialog() == DialogResult.OK && dlgAbrir.FileName.Length > 0)
            {
                if (dlgAbrir.FilterIndex == 1)
                {
                    rtbEditor.LoadFile(dlgAbrir.FileName, RichTextBoxStreamType.PlainText);
                }
                else
                {
                    rtbEditor.LoadFile(dlgAbrir.FileName, RichTextBoxStreamType.RichText);
                }
                Text = dlgAbrir.FileName;
                rtbEditor.Modified = false;
            }
            stEstadoEditor.Items[0].Text = "";
            itQuitarFormato.PerformClick();
            rtbEditor.Focus();
        }

        private void itGuardar_Click(object sender, EventArgs e)
        {
            if (Text == "Documento1")
            {
                itGuardarComo.PerformClick();
            }
            else
            {
                rtbEditor.SaveFile(Text);
                rtbEditor.Modified = false;
                rtbEditor.Focus();
            }
        }

        private void itGuardarComo_Click(object sender, EventArgs e)
        {
            dlgGuardar.FileName = Text;
            if (dlgGuardar.ShowDialog() == DialogResult.OK && dlgGuardar.FileName.Length > 0)
            {
                if (dlgGuardar.FilterIndex == 1)
                {
                    rtbEditor.SaveFile(dlgGuardar.FileName, RichTextBoxStreamType.PlainText);
                }
                else
                {
                    rtbEditor.SaveFile(dlgGuardar.FileName, RichTextBoxStreamType.RichText);
                }
                Text = dlgGuardar.FileName;
                rtbEditor.Modified = false;
            }
        }

        private void itNuevo_Click(object sender, EventArgs e)
        {
            stEstadoEditor.Items[0].Text = "Generando un documento nuevo, guardando el anterior si procede";
            if (rtbEditor.Modified)
            {
                DialogResult resultado = MessageBox.Show("Hay Cambios pendientes de guardar. Guardas? ","Guardar Cambios", MessageBoxButtons.YesNoCancel);
                switch (resultado)
                {
                    case DialogResult.Yes:
                        itGuardar.PerformClick();
                        break;
                    case DialogResult.Cancel:
                        rtbEditor.Focus();
                        return;
                }
            }
            rtbEditor.Clear();
            Text = "Documento2";
            itQuitarFormato.PerformClick();
            rtbEditor.Modified = false;
            stEstadoEditor.Items[0].Text = "";
        }

        private void fmEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((rtbEditor.Modified) && (rtbEditor.Text.Length > 0))
            {
                DialogResult resultado = MessageBox.Show("Hay Cambios pendientes de guardar. Guardas? ", "Guardar Cambios", MessageBoxButtons.YesNoCancel);
                switch (resultado)
                {
                    case DialogResult.Yes:
                        itGuardar.PerformClick();
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        rtbEditor.Focus();
                        break;
                }
            }
        }

        // Para boton imprimir
        string[] linea;
        int totalLineasImpresas;

        private void itImprimir_Click(object sender, EventArgs e)
        {
            if (dlgImprimir.ShowDialog() == DialogResult.OK)
            {
                string texto = rtbEditor.Text;
                char[] seps = { '\n', '\r' };
                linea = texto.Split(seps);
                totalLineasImpresas = 0;
                prindocEditor.Print();
            }
        }

        private void prindocEditor_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            float lineasPorPag;
            float pos_Y;
            float margenIzq = e.MarginBounds.Left;
            float margenSup = e.MarginBounds.Top;

            Font fuente = rtbEditor.Font;
            float altoFuente = fuente.GetHeight(e.Graphics);
            lineasPorPag = e.MarginBounds.Height / altoFuente;
            int lineasImpresasPorPag = 0;
            while (totalLineasImpresas < linea.Length && lineasImpresasPorPag < lineasPorPag)
            {
                pos_Y = margenSup + (lineasImpresasPorPag * altoFuente);
                e.Graphics.DrawString(linea[totalLineasImpresas], fuente, Brushes.Black, margenIzq, pos_Y, new StringFormat());
                lineasImpresasPorPag += 1;
                totalLineasImpresas += 1;
            }

            if (totalLineasImpresas < linea.Length)
                e.HasMorePages = true;
            else
                e.HasMorePages = false;
        }

        private void itFuentes_Click(object sender, EventArgs e)
        {
            dlgFuentes.Color = rtbEditor.SelectionColor;
            dlgFuentes.Font = rtbEditor.SelectionFont;
            if (dlgFuentes.ShowDialog() == DialogResult.OK)
            {
                rtbEditor.SelectionFont = dlgFuentes.Font;
                rtbEditor.SelectionColor = dlgFuentes.Color;
                rtbEditor_SelectionChanged(sender, e);
                rtbEditor.Modified = true;
            }
        }

        private void rtbEditor_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                cbTamanyo.Text = Convert.ToString(Math.Truncate(rtbEditor.SelectionFont.Size));
                itVinyetas.Checked = rtbEditor.SelectionBullet;
                tsbNegrita.Checked = rtbEditor.SelectionFont.Bold;
                tsbSubrayado.Checked = rtbEditor.SelectionFont.Underline;
                tsbTachado.Checked = rtbEditor.SelectionFont.Strikeout;
                tsbCursiva.Checked = rtbEditor.SelectionFont.Italic;
                cbFuentes.SelectedIndex =
                cbFuentes.Items.IndexOf(rtbEditor.SelectionFont.Name);
            }
            catch
            {
                return;
            }
            tsbAlineacionIzq.Checked = rtbEditor.SelectionAlignment == HorizontalAlignment.Left;
            tsbAlineacionDer.Checked = rtbEditor.SelectionAlignment == HorizontalAlignment.Right;
            tsbAlineacionCent.Checked = rtbEditor.SelectionAlignment == HorizontalAlignment.Center;
            itIzquierda.Checked = tsbAlineacionIzq.Checked;
            itDerecha.Checked = tsbAlineacionDer.Checked;
            itCentrado.Checked = tsbAlineacionCent.Checked;

        }

        private void itFormatoPagina_Click(object sender, EventArgs e)
        {
            //TODO
            dlgFuentes.Color = rtbEditor.SelectionColor;
            dlgFuentes.Font = rtbEditor.SelectionFont;
            if (dlgFuentes.ShowDialog() == DialogResult.OK)
            {
                rtbEditor.SelectionFont = dlgFuentes.Font;
                rtbEditor.SelectionColor = dlgFuentes.Color;
                rtbEditor_SelectionChanged(sender, e);
                rtbEditor.Modified = true;
            }
        }

        private void itSeleccionarTodo_Click(object sender, EventArgs e)
        {
            rtbEditor.SelectAll();
        }

        private void itBorrar_Click(object sender, EventArgs e)
        {
            if (rtbEditor.SelectionLength > 0)
            {
                rtbEditor.SelectedText = "";
            }
        }

        private void itDeshacer_Click(object sender, EventArgs e)
        {
            if (rtbEditor.CanUndo)
            {
                rtbEditor.Undo();
            }
        }

        private void itRehacer_Click(object sender, EventArgs e)
        {
            if (rtbEditor.CanRedo)
            {
                rtbEditor.Redo();
            }
        }

        private void itCortar_Click(object sender, EventArgs e)
        {
            rtbEditor.Cut();
        }

        private void itCopiar_Click(object sender, EventArgs e)
        {
            rtbEditor.Copy();
        }

        private void itPegar_Click(object sender, EventArgs e)
        {
            rtbEditor.Paste();
        }

        fmDatos VentanaIntroduccion = new fmDatos();
        private void itIrA_Click(object sender, EventArgs e)
        {
            VentanaIntroduccion.tbDato.Clear();
            if (VentanaIntroduccion.ShowDialog() == DialogResult.OK)
            {
                int numlinea = 0, conta = 0, acumula = 0;
                try
                {
                    numlinea = Convert.ToInt32(VentanaIntroduccion.tbDato.Text);
                }
                catch (Exception mierror)
                {
                    MessageBox.Show(mierror.Message);
                }

                while ((conta < (numlinea - 1)) && (conta < (rtbEditor.Lines.Length - 1)))
                {
                    acumula += ((rtbEditor.Lines[conta].Length) + 1);
                    conta++;
                }
                rtbEditor.SelectionStart = acumula;
            }
        }

        private void itSalir_Click(object sender, EventArgs e)
        { // TODO:
            if ((rtbEditor.Modified) && (rtbEditor.Text.Length > 0))
            {
                DialogResult resultado = MessageBox.Show("Hay Cambios pendientes de guardar. Guardas? ", "Guardar Cambios", MessageBoxButtons.YesNoCancel);
                switch (resultado)
                {
                    case DialogResult.Yes:
                        itGuardar.PerformClick();
                        break;
                    case DialogResult.Cancel:
                        //e.Cancel = true;
                        rtbEditor.Focus();
                        break;
                    case DialogResult.No:
                        Close();
                        break;
                }
            }
            else {
                Close();
            }
        }

        fmMargenes VentanaMargenes = new fmMargenes();
        private void itMargenes_Click(object sender, EventArgs e)
        {
            VentanaMargenes.cbIzquierda.Text = Convert.ToString(rtbEditor.SelectionIndent);
            VentanaMargenes.cbDerecha.Text = Convert.ToString(rtbEditor.SelectionRightIndent);
            if (VentanaMargenes.ShowDialog() == DialogResult.OK)
            {
                rtbEditor.SelectionRightIndent = Convert.ToInt32(VentanaMargenes.cbDerecha.Text);
                rtbEditor.SelectionIndent = Convert.ToInt32(VentanaMargenes.cbIzquierda.Text);
                rtbEditor.Modified = true;
            }
        }

        private void itVinyetas_Click(object sender, EventArgs e)
        {
            itVinyetas.Checked = !itVinyetas.Checked;
            rtbEditor.SelectionBullet = itVinyetas.Checked;
            rtbEditor.Modified = true;
        }

        private void itBarraHerramientas_Click(object sender, EventArgs e)
        {
            itBarraEstandar.Checked = !itBarraEstandar.Checked;
            itcBarraEstandar.Checked = !itcBarraEstandar.Checked;
            tsBarraEstandar.Visible = itBarraEstandar.Checked;
        }

        private void itBarraFormato_Click(object sender, EventArgs e)
        {
            itBarraFormato.Checked = !itBarraFormato.Checked;
            itcBarraFormato.Checked = !itcBarraFormato.Checked;
            tsBarraFormato.Visible = itBarraFormato.Checked;
        }

        private void itBarraEstado_Click(object sender, EventArgs e)
        {
            itBarraEstado.Checked = !itBarraEstado.Checked;
            itcBarraEstado.Checked = !itcBarraEstado.Checked;
            stEstadoEditor.Visible = itBarraEstado.Checked;
        }

        FontStyle miestilo = new FontStyle();
        string mifuente;
        float mitamanyo;
        Color micolor;
        private void tsbCopiarFormato_Click(object sender, EventArgs e)
        {
            if (tsbCopiarFormato.Checked)
            {
                stEstadoEditor.Items[0].Text = "Vas a copiar formato a la nueva ubicación, donde hagas click"; 
            }
            else
            {
                stEstadoEditor.Items[0].Text = "";
            }

            miestilo = rtbEditor.SelectionFont.Style;
            mifuente = rtbEditor.SelectionFont.Name;
            mitamanyo = rtbEditor.SelectionFont.Size;
            micolor = rtbEditor.SelectionColor;
        }

        private void rtbEditor_MouseDown(object sender, MouseEventArgs e)
        {
            stEstadoEditor.Items[0].Text = "Si el botón de copiar formatos está pulsado se aplicarán los formatos copiados"; 
            if (tsbCopiarFormato.Checked)
            {
                rtbEditor.SelectionFont = new Font(mifuente, mitamanyo, miestilo);
                rtbEditor.SelectionColor = micolor;
            }
        }

        private void ayudaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fmAcercaDe acercaDe = new fmAcercaDe();
            acercaDe.ShowDialog();
        }
    }
}
