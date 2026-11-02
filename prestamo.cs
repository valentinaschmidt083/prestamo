using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace login
{
    public partial class prestamo : Form
    {
        // Lista donde se guardan los préstamos
        private List<Prestamo> prestamos = new List<Prestamo>();

        public prestamo()
        {
            InitializeComponent();

            // Estados del préstamo
            cmbEstado.Items.Add("Seleccione");
            cmbEstado.Items.Add("Activo");
            cmbEstado.Items.Add("Devuelto");
            cmbEstado.Items.Add("Vencido");

            cmbEstado.SelectedIndex = 0;

            // Eventos de los botones
            btnNuevo.Click += btnNuevo_Click;
            btnCancelarReserva.Click += btnCancelarReserva_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnGuardar.Click += btnGuardar_Click;
            btnCancelar.Click += btnCancelar_Click;
            btnSalir.Click += btnSalir_Click;

            btnBuscarCliente.Click += btnBuscarCliente_Click;
            btnBuscarLibro.Click += btnBuscarLibro_Click;
            btnCalcular.Click += btnCalcular_Click;
        }

        // =========================
        // NUEVO
        // =========================
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();

            MessageBox.Show(
                "Nuevo préstamo listo.",
                "Nuevo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // =========================
        // BUSCAR CLIENTE
        // =========================
        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            string dni = txtDni.Text.Trim();

            if (dni == "")
            {
                MessageBox.Show("Ingrese un DNI.");
                txtDni.Focus();
                return;
            }

            // Datos de ejemplo
            if (dni == "12345678")
            {
                txtApellido.Text = "Gómez";
                txtNombre.Text = "Juan";
                txtEstadoCliente.Text = "Activo";
            }
            else if (dni == "87654321")
            {
                txtApellido.Text = "Pérez";
                txtNombre.Text = "María";
                txtEstadoCliente.Text = "Activo";
            }
            else
            {
                MessageBox.Show(
                    "No se encontró un cliente con ese DNI.",
                    "Cliente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtApellido.Clear();
                txtNombre.Clear();
                txtEstadoCliente.Clear();
            }
        }

        // =========================
        // BUSCAR LIBRO
        // =========================
        private void btnBuscarLibro_Click(object sender, EventArgs e)
        {
            string codigo = txtCodigo.Text.Trim();

            if (codigo == "")
            {
                MessageBox.Show("Ingrese el código del libro.");
                txtCodigo.Focus();
                return;
            }

            // Libros de ejemplo
            if (codigo == "001")
            {
                txtTitulo.Text = "Harry Potter";
                txtAutor.Text = "J. K. Rowling";
                txtEstadoLibro.Text = "Disponible";
                txtPrecio.Text = "500";
            }
            else if (codigo == "002")
            {
                txtTitulo.Text = "El Principito";
                txtAutor.Text = "Antoine de Saint-Exupéry";
                txtEstadoLibro.Text = "Disponible";
                txtPrecio.Text = "400";
            }
            else if (codigo == "003")
            {
                txtTitulo.Text = "Don Quijote";
                txtAutor.Text = "Miguel de Cervantes";
                txtEstadoLibro.Text = "Disponible";
                txtPrecio.Text = "600";
            }
            else
            {
                MessageBox.Show(
                    "No se encontró el libro.",
                    "Libro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTitulo.Clear();
                txtAutor.Clear();
                txtEstadoLibro.Clear();
                txtPrecio.Clear();
            }
        }

        // =========================
        // CALCULAR
        // =========================
        private void btnCalcular_Click(object sender, EventArgs e)
        {
            DateTime inicio = dtpFechaInicio.Value.Date;
            DateTime fin = dtpFechaFin.Value.Date;

            if (fin < inicio)
            {
                MessageBox.Show(
                    "La fecha de devolución no puede ser anterior a la fecha de inicio.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
            {
                MessageBox.Show("El precio del libro no es válido.");
                return;
            }

            int dias = (fin - inicio).Days;

            // Si es el mismo día, contamos 1 día
            if (dias == 0)
                dias = 1;

            decimal total = precio * dias;

            MessageBox.Show(
                "Días del préstamo: " + dias +
                "\nPrecio por día: $" + precio +
                "\nTotal: $" + total,
                "Cálculo del préstamo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // =========================
        // GUARDAR
        // =========================
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarDatos())
                return;

            Prestamo nuevo = new Prestamo();

            nuevo.Dni = txtDni.Text;
            nuevo.Apellido = txtApellido.Text;
            nuevo.Nombre = txtNombre.Text;

            nuevo.CodigoLibro = txtCodigo.Text;
            nuevo.Titulo = txtTitulo.Text;
            nuevo.Autor = txtAutor.Text;

            nuevo.FechaInicio = dtpFechaInicio.Value;
            nuevo.FechaFin = dtpFechaFin.Value;

            nuevo.Estado = cmbEstado.Text;

            decimal.TryParse(txtPrecio.Text, out decimal precio);
            nuevo.Precio = precio;

            prestamos.Add(nuevo);

            MessageBox.Show(
                "El préstamo se guardó correctamente.",
                "Guardar",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            MostrarPrestamos();
        }

        // =========================
        // VALIDAR DATOS
        // =========================
        private bool ValidarDatos()
        {
            if (txtDni.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el DNI del cliente.");
                txtDni.Focus();
                return false;
            }

            if (txtApellido.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el apellido.");
                txtApellido.Focus();
                return false;
            }

            if (txtNombre.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el nombre.");
                txtNombre.Focus();
                return false;
            }

            if (txtCodigo.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el código del libro.");
                txtCodigo.Focus();
                return false;
            }

            if (txtTitulo.Text.Trim() == "")
            {
                MessageBox.Show("Busque un libro.");
                return false;
            }

            if (txtEstadoLibro.Text != "Disponible")
            {
                MessageBox.Show("El libro no está disponible.");
                return false;
            }

            if (cmbEstado.SelectedIndex == 0)
            {
                MessageBox.Show("Seleccione el estado del préstamo.");
                cmbEstado.Focus();
                return false;
            }

            if (dtpFechaFin.Value.Date < dtpFechaInicio.Value.Date)
            {
                MessageBox.Show("La fecha final no puede ser anterior a la inicial.");
                return false;
            }

            return true;
        }

        // =========================
        // CANCELAR RESERVA
        // =========================
        private void btnCancelarReserva_Click(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show(
                "¿Desea cancelar la reserva?",
                "Cancelar reserva",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                LimpiarCampos();

                MessageBox.Show("Reserva cancelada.");
            }
        }

        // =========================
        // ELIMINAR
        // =========================
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            string dni = txtDni.Text.Trim();
            string codigo = txtCodigo.Text.Trim();

            Prestamo encontrado = prestamos.FirstOrDefault(
                p => p.Dni == dni && p.CodigoLibro == codigo);

            if (encontrado == null)
            {
                MessageBox.Show(
                    "No se encontró el préstamo para eliminar.",
                    "Eliminar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Está seguro de eliminar este préstamo?",
                "Eliminar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                prestamos.Remove(encontrado);

                MessageBox.Show("Préstamo eliminado correctamente.");

                LimpiarCampos();
                MostrarPrestamos();
            }
        }

        // =========================
        // CANCELAR
        // =========================
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        // =========================
        // SALIR
        // =========================
        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show(
                "¿Desea salir?",
                "Salir",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                Close();
            }
        }

        // =========================
        // LIMPIAR CAMPOS
        // =========================
        private void LimpiarCampos()
        {
            txtDni.Clear();
            txtApellido.Clear();
            txtNombre.Clear();
            txtEstadoCliente.Clear();

            txtCodigo.Clear();
            txtTitulo.Clear();
            txtAutor.Clear();
            txtEstadoLibro.Clear();
            txtPrecio.Clear();

            cmbEstado.SelectedIndex = 0;

            dtpFechaInicio.Value = DateTime.Today;
            dtpFechaFin.Value = DateTime.Today;

            txtDni.Focus();
        }

        // =========================
        // MOSTRAR PRÉSTAMOS
        // =========================
        private void MostrarPrestamos()
        {
            // Por ahora muestra los préstamos guardados
            // en una ventana.

            if (prestamos.Count == 0)
                return;

            string mensaje = "";

            foreach (Prestamo p in prestamos)
            {
                mensaje +=
                    "DNI: " + p.Dni +
                    "\nCliente: " + p.Apellido + ", " + p.Nombre +
                    "\nLibro: " + p.Titulo +
                    "\nInicio: " + p.FechaInicio.ToShortDateString() +
                    "\nFin: " + p.FechaFin.ToShortDateString() +
                    "\nEstado: " + p.Estado +
                    "\n-------------------------\n";
            }

            // No mostramos automáticamente el mensaje
            // para no molestar cada vez que se guarda.
        }
    }

    // ===================================
    // CLASE PRÉSTAMO
    // ===================================
    public class Prestamo
    {
        public string Dni { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }

        public string CodigoLibro { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public string Estado { get; set; }

        public decimal Precio { get; set; }
    }
}