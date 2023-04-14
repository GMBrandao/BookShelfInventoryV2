internal class Program
{
    private static void Main(string[] args)
    {
        do
        {
            switch (Menu())
            {
                case "1":
                    //adicionar livro
                    break;

                case "2":
                    //editar livro
                    break;

                case "3":
                    //Deletar livro
                    break;

                case "4":
                    //ler livro
                    break;

                case "5":
                    //Printar Estante
                    break;

                case "9":
                    Console.WriteLine("Encerrando...");
                    Thread.Sleep(1000);
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Opção inválida");
                    break;
            }
        } while (true);
    }

    static string Menu()
    {
        Console.WriteLine("\t>>>>> MENU <<<<<\n1 - Inserir Livro na Estante\n2 - Editar Livro\n3 - Deletar Livro" +
            "\n4 - Inserir sessão de leitura de um Livro\n5 - Mostrar Estante\n9 - Sair\n\nEscolha uma opção: ");
        return Console.ReadLine();
    }
}