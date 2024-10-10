using System;
using System.Collections.Generic;

public class Livro
{
	public string Titulo { get; set; }
	public string Autor { get; set; }
	public int Quantidade { get; set; }

	public Livro(string titulo, string autor, int quantidade)
	{
		Titulo = titulo;
		Autor = autor;
		Quantidade = quantidade;
	}
}

public class Usuario
{
	public string Nome { get; set; }
	public List<Livro> LivrosEmprestados { get; } = new List<Livro>();
	public bool IsAdministrador { get; set; }

	public Usuario(string nome, bool isAdmin)
	{
		Nome = nome;
		IsAdministrador = isAdmin;
	}

	public bool PodeEmprestar() => LivrosEmprestados.Count < 3;
	public void EmprestarLivro(Livro livro)
	{
		LivrosEmprestados.Add(livro);
		livro.Quantidade--;
	}

	public void DevolverLivro(Livro livro)
	{
		LivrosEmprestados.Remove(livro);
		livro.Quantidade++;
	}
}

public class Biblioteca
{
	public List<Livro> Livros { get; } = new List<Livro>();

	public void CadastrarLivro(Livro livro) => Livros.Add(livro);
	public void ConsultarCatalogo()
	{
		foreach (var livro in Livros)
			Console.WriteLine($"Título: {livro.Titulo}, Autor: {livro.Autor}, Quantidade: {livro.Quantidade}");
	}

	public Livro EncontrarLivro(string titulo) => Livros.Find(l => l.Titulo.Equals(titulo, StringComparison.OrdinalIgnoreCase));
}

public class Programa
{
	static void Main(string[] args)
	{
		Biblioteca biblioteca = new Biblioteca();
		Usuario admin = new Usuario("Admin", true);
		Usuario usuario = new Usuario("User", false);
		while (true)
		{
			Console.WriteLine("1. Cadastrar Livro (Admin)\n2. Consultar Catálogo\n3. Emprestar Livro\n4. Devolver Livro\n5. Sair");
			string opcao = Console.ReadLine();
			if (opcao == "1" && admin.IsAdministrador)
			{
				Console.Write("Título: ");
				string titulo = Console.ReadLine();
				Console.Write("Autor: ");
				string autor = Console.ReadLine();
				Console.Write("Quantidade: ");
				int quantidade = int.Parse(Console.ReadLine());
				biblioteca.CadastrarLivro(new Livro(titulo, autor, quantidade));
			}
			else if (opcao == "2")
				biblioteca.ConsultarCatalogo();
			else if (opcao == "3")
			{
				Console.Write("Título do livro: ");
				string titulo = Console.ReadLine();
				Livro livro = biblioteca.EncontrarLivro(titulo);
				if (livro != null && livro.Quantidade > 0 && usuario.PodeEmprestar())
				{
					usuario.EmprestarLivro(livro);
					Console.WriteLine("Livro emprestado!");
				}
				else
					Console.WriteLine("Livro não disponível ou limite atingido.");
			}
			else if (opcao == "4")
			{
				Console.Write("Título do livro: ");
				string titulo = Console.ReadLine();
				Livro livro = usuario.LivrosEmprestados.Find(l => l.Titulo.Equals(titulo, StringComparison.OrdinalIgnoreCase));
				if (livro != null)
				{
					usuario.DevolverLivro(livro);
					Console.WriteLine("Livro devolvido!");
				}
				else
					Console.WriteLine("Você não possui esse livro.");
			}
			else if (opcao == "5")
				break;
			else
				Console.WriteLine("Opção inválida.");
		}
	}
}