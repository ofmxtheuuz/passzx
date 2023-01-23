using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Passzx.database;
using Passzx.database.models;
using Passzx.res;

namespace Passzx.helpers;

public class indexhelper
{
    public static void home()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine();
        Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Passzx"));
        Console.WriteLine();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("    1. Ver todas as contas");
        Console.WriteLine("    2. Buscar contas pela categoria");
        Console.WriteLine("    3. Inserir uma conta");
        Console.WriteLine("    4. Atualizar uma conta");
        Console.WriteLine("    5. Gerar um JSON");
        Console.WriteLine("    6. Carregar um JSON");
        Console.WriteLine("    7. Sair");
        Console.WriteLine();
        Console.Write("    Qual sua opção: ");
        string response = Console.ReadLine();
        switch_response(response);
        
    }

    private static void switch_response(string response)
    {
        switch (response)
        {
            case "1":
                all_accounts();
                break;
            case "2":
                accounts_category();
                break;
            case "3":
                insert_account();
                break;
            case "4":
                update_account();
                break;
            case "5":
                generate_json();
                break;
            case "6":
                load_json();
                break;
            case "7":
		    
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Clear();
			
                Environment.Exit(1);
                break;
        }
    } 


    private static void all_accounts()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine();
        Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Todas as contas"));
        Console.WriteLine();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;

        sqlserverdbcontext db = new sqlserverdbcontext();
        var accounts = db.Accounts.ToList();
        if (accounts.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine("    Nenhuma conta encontrada!");
            Console.WriteLine("    Retornando em 3 segundos.");
            Thread.Sleep(3000);
            home();
        }
        foreach (var account in accounts)
        {
            Console.WriteLine("    Id: {0}, Email: {1}, Usuário: {2}, Senha: {3}, Categoria: {4}",
                account.Id, account.Email, account.Username, account.Password, account.Category);
        }

        Console.WriteLine();
        Console.Write("Pressione qualquer tecla para retornar");
        Console.ReadKey();
        home();
    }

    private static void insert_account()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine();
        Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Inserir uma conta"));
        Console.WriteLine();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        using (var db = new sqlserverdbcontext())
        {
            Console.Write("Email: ");
            var email = Console.ReadLine();
            Console.Write("Username: ");
            var username = Console.ReadLine();
            Console.Write("Senha: ");
            var password = Console.ReadLine();
            Console.Write("Categoria: ");
            var category = Console.ReadLine();

            var newAccount = new account
            {
                Email = email ?? "vazio",
                Username = username ?? "vazio",
                Password = password,
                Category = category
            };

            db.Accounts.Add(newAccount);
            db.SaveChanges();
            Console.WriteLine();
            Console.WriteLine("    Conta adicionada!");
            Console.WriteLine("    Retornando em 3 segundos.");
            Thread.Sleep(3000);
            home();
        }
    }
    private static void accounts_category()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine();
        Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Buscar por categoria"));
        Console.WriteLine();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine();
        Console.Write("    Digite a categoria: ");
        string category = Console.ReadLine();

        sqlserverdbcontext db = new sqlserverdbcontext();

        Console.WriteLine();
        var filteredAccounts = db.Accounts.Where(a => a.Category.Contains(category)).ToList();
        if (filteredAccounts.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine("    Nenhuma conta encontrada!");
            Console.WriteLine("    Retornando em 3 segundos.");
            Thread.Sleep(3000);
            home();
        }
        foreach (var account in filteredAccounts)
        {
            Console.WriteLine("    Id: {0}, Email: {1}, Usuário: {2}, Senha: {3}, Categoria: {4}",
                account.Id, account.Email, account.Username, account.Password, account.Category);
        }

        Console.WriteLine();
        Console.Write("Pressione qualquer tecla para retornar");
        Console.ReadKey();
        home();
    }
    private static void update_account()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine();
        Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Atualizar uma conta"));
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;

        sqlserverdbcontext db = new sqlserverdbcontext();
        
        Console.Write("Id da conta: ");
        var id = int.Parse(Console.ReadLine());
        Console.Write("Email: ");
        var email = Console.ReadLine();
        Console.Write("Usuário: ");
        var username = Console.ReadLine();
        Console.Write("Senha: ");
        var password = Console.ReadLine();
        Console.Write("Categoria :");
        var category = Console.ReadLine();

        var account = new account
        {
            Id = id,
            Email = email,
            Username = username,
            Password = password,
            Category = category
        };

        db.Accounts.Update(account);
        db.SaveChanges();            
        Console.WriteLine();
        Console.WriteLine("    Conta atualizada!");
        Console.WriteLine("    Retornando em 3 segundos.");
        Thread.Sleep(3000);
        home();
    }
    
    private static void generate_json()
    {
        using (var db = new sqlserverdbcontext())
        {
            List<accountres> res = new List<accountres>();
            var accounts = db.Accounts.ToList();
            foreach(var account in accounts) { res.Add(new accountres() { Email = account.Email, Username = account.Username, Password = account.Password, Category = account.Category}); }
            var json = JsonConvert.SerializeObject(res);
            File.WriteAllText("accounts.json", json);
        }
        home();
    }
    
    private static void load_json()
    {
        using (var db = new sqlserverdbcontext())
        {
            Console.WriteLine("    Insira o JSON:");
            var json = Console.ReadLine();
            var accounts = JsonConvert.DeserializeObject<List<account>>(json);
            db.Accounts.AddRange(accounts);
            db.SaveChanges();
            home();
        }
    }
}