using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymManager.Forms
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        // Evento cuando el usuario entra al campo de email
        private void txtEmail_Enter(object sender, EventArgs e)
        {
            if (txtEmail.Text == "Email")
            {
                txtEmail.Text = "";
                txtEmail.ForeColor = Color.Black;
            }
        }

        // Evento cuando el usuario deja el campo de email
        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                txtEmail.Text = "Email";
                txtEmail.ForeColor = Color.Gray;
            }
        }

        // Evento cuando el usuario entra al campo de contraseña
        private void txtPassword_Enter(object sender, EventArgs e)
        {
            // Si el texto es el placeholder, se borra
            if (txtPassword.Text == "contraseña")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Black;
                txtPassword.PasswordChar = '*'; // Oculta el texto
            }
        }

        // Evento cuando el usuario deja el campo de contraseña
        private void txtPassword_Leave(object sender, EventArgs e)
        {
            // Si está vacío, volvemos al placeholder
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Text = "contraseña";
                txtPassword.ForeColor = Color.Gray;
                txtPassword.PasswordChar = '\0'; // Muestra el texto normalmente
            }
        }
    }
}
