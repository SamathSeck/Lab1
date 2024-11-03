using System;
using System.IO;

class DistributeurDeTickets
{
    static string fichierTickets = @"temp\fnumero.txt";
    static string fichierClients = @"temp\clients.txt";
    static string[] types = { "V", "R", "I" };

    static void Main()
    {
        try
        {
            if (!File.Exists(fichierTickets))
            {
                Directory.CreateDirectory("temp");
                File.WriteAllText(fichierTickets, "V-0\nR-0\nI-0");
            }

            if (!File.Exists(fichierClients))
            {
                File.Create(fichierClients).Close();
            }

            bool continuer = true;
            while (continuer)
            {
                string choix;

                while (true)
                {
                    Console.WriteLine("Choisissez une opération : (V)ersement, (R)etrait, (I)nformations, (Q) pour Quitter");
                    choix = Console.ReadLine()?.ToUpper();

                    if (choix == "Q" || Array.Exists(types, t => t == choix))
                    {
                        break;
                    }
                    Console.WriteLine("Choix invalide. Veuillez entrer 'V', 'R', 'I' ou 'Q'.");
                }

                if (choix == "Q")
                {
                    Console.WriteLine("Merci et à bientôt !");
                    break;
                }

                string nom = DemanderNomOuPrenom("nom");
                string prenom = DemanderNomOuPrenom("prénom");

                string ticket = GenererTicket(choix, out int enAttente);
                Console.WriteLine($"Votre numéro est \"{ticket}\", et il y a {enAttente} personnes qui attendent avant vous.");
                EnregistrerClient($"{nom} {prenom} - Ticket: {ticket}");

                Console.WriteLine("Voulez-vous prendre un autre ticket ? Tapez 'O' ou sur n'importe quelle touche pour quitter. (O)");
                continuer = Console.ReadLine()?.ToUpper() == "O";
            }

            Console.WriteLine("\nListe des clients :");
            AfficherClients();
        }
        finally
        {
            if (File.Exists(fichierTickets))
            {
                File.Delete(fichierTickets);
            }

            Console.WriteLine("\nAppuyez sur Entrée pour fermer le programme");
            Console.ReadLine();
        }
    }

    static string DemanderNomOuPrenom(string type)
    {
        string entree;
        while (true)
        {
            Console.Write($"Veuillez entrer votre {type} (au moins 2 caractères) : ");
            entree = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(entree) && entree.Length >= 2)
            {
                break;
            }
            Console.WriteLine($"{type} invalide. Veuillez entrer au moins 2 caractères.");
        }
        return entree;
    }

    static string GenererTicket(string type, out int enAttente)
    {
        string[] lignes = File.ReadAllLines(fichierTickets);
        int index = Array.IndexOf(types, type);
        string[] parts = lignes[index].Split('-');
        int numero = int.Parse(parts[1]) + 1;
        lignes[index] = $"{type}-{numero}";
        File.WriteAllLines(fichierTickets, lignes);

        enAttente = numero - 1;
        return lignes[index];
    }

    static void EnregistrerClient(string clientInfo)
    {
        using (StreamWriter writer = new StreamWriter(fichierClients, true))
        {
            writer.WriteLine(clientInfo);
        }
    }

    static void AfficherClients()
    {
        if (File.Exists(fichierClients))
        {
            string[] clients = File.ReadAllLines(fichierClients);
            foreach (string client in clients)
            {
                Console.WriteLine(client);
            }
        }
    }
}
