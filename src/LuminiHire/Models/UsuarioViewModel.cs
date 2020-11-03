using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LuminiHire.Models
{
	public class UsuarioRegistro
	{
		[Required(ErrorMessage = "O Campo {0} é obrigatório")]
		[EmailAddress(ErrorMessage = "O campo {0} está em um formato inválido")]
		public string Email { get; set; }

		[Required(ErrorMessage = "O Campo {0} é obrigatório")]
		[DataType(DataType.Password)]
		public string Senha { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirme a senha")]
		[Compare("Senha", ErrorMessage = "As senhas não conferem")]
		public string ConfirmarSenha { get; set; }
	}

	/// <summary>
	/// Model utilizada para o login do usuário na aplicação
	/// </summary>
	public class UsuarioLogin
	{
		[Required(ErrorMessage = "O Campo {0} é obrigatório")]
		[EmailAddress(ErrorMessage = "O campo {0} está em um formato inválido")]
		public string Email { get; set; }

		[Required(ErrorMessage = "O Campo {0} é obrigatório")]
		[DataType(DataType.Password)]
		public string Senha { get; set; }
	}
}
