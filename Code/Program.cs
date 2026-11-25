//Utilise l'add on SQLite de Microsoft

using System.ComponentModel.DataAnnotations;
using Microsoft.Data.Sqlite;

//Création ou Connexion a ma BDD SQL
string connectionString = "Data Source = escapegame.db";
using var connection = new SqliteConnection(connectionString);
connection.Open();
Console.WriteLine("Connecté !");

//Création de mes tables
var createTables = connection.CreateCommand();
createTables.CommandText = @"
    CREATE TABLE IF NOT EXISTS Joueur(
   Id_joueur INTEGER,
   Pseudo TEXT NOT NULL,
   Mot_de_passe text not null,
   PRIMARY KEY(Id_joueur),
        check (length(Mot_de_passe)>4)
);

CREATE TABLE IF NOT EXISTS Salle(
   Id_salle INTEGER,
   Nom TEXT NOT NULL,
   PRIMARY KEY(Id_salle),
   UNIQUE(Nom)
);

CREATE TABLE IF NOT EXISTS Questions(
   Id_question INTEGER,
   enonce TEXT NOT NULL,
   Choix_A TEXT NOT NULL,
   Choix_B TEXT NOT NULL,
   Choix_C TEXT NOT NULL,
   Reponse TEXT NOT NULL,
   Indice TEXT NOT NULL,
   Id_salle INTEGER NOT NULL,
   PRIMARY KEY(Id_question),
   FOREIGN KEY(Id_salle) REFERENCES Salle(Id_salle),
   UNIQUE(enonce)
);

CREATE TABLE IF NOT EXISTS Partie(
   Id_partie INTEGER,
   Question_index INTEGER NOT NULL,
   Vie_restante INTEGER NOT NULL,
   Temps_ecoule INTEGER NOT NULL,
   Termine NUMERIC NOT NULL,
   Id_salle INTEGER NOT NULL,
   Id_joueur INTEGER NOT NULL,
   PRIMARY KEY(Id_partie),
   FOREIGN KEY(Id_salle) REFERENCES Salle(Id_salle),
   FOREIGN KEY(Id_joueur) REFERENCES Joueur(Id_joueur)
);
";
createTables.ExecuteNonQuery();

SqliteCommand insertInSalles = connection.CreateCommand();
insertInSalles.CommandText = @"
INSERT OR IGNORE INTO Salle (Id_salle, Nom) VALUES          
(1,'Le Terminal d''Accès'),
(2,'L''Open Space des Devs'),
(3,'Le Datacenter de Radianite'),
(4,'La Régie Esport & Scène VCT'),
(5,'Le Noyau Anti-Cheat');
";

insertInSalles.ExecuteNonQuery();

SqliteCommand insertInQuestions = connection.CreateCommand();
insertInQuestions.CommandText = @"
INSERT OR IGNORE INTO Questions (enonce, Choix_A, Choix_B, Choix_C, 
Reponse, Indice, Id_salle) VALUES
--Salle 1
('Quelle valeur binaire correspond au nombre décimal 5 ?', '101', '110', '011', 'A', '4 + 1 en base 2.', 1),
('Combien de bits composent un octet ?', '4', '8', '16', 'B', 'C''est une puissance de 2 inférieure à 10.', 1),
('Quelle touche permet d''annuler la dernière action sous Windows ?', 'Ctrl + Z', 'Ctrl + Y', 'Ctrl + X', 'A', 'La dernière lettre de l''alphabet.', 1),
('Que signifie l''acronyme CPU ?', 'Central Processing Unit', 'Computer Personal Unit', 'Core Power Utility', 'A', 'C''est le processeur central de la machine.', 1),
('Quel système d''exploitation utilise un manchot comme mascotte ?', 'macOS', 'Linux', 'Windows', 'B', 'Il s''appelle Tux.', 1),
('En hexadécimal, quelle lettre représente la valeur 10 ?', 'F', 'A', 'B', 'B', 'C''est la première lettre après le chiffre 9.', 1),
('Quelle mémoire est volatile et se vide quand on éteint le PC ?', 'Le SSD', 'La RAM', 'La ROM', 'B', 'Mémoire vive à accès aléatoire.', 1),
('Quelle commande terminal permet de lister les fichiers sous Linux ?', 'ls', 'cd', 'mkdir', 'A', 'Abréviation de list.', 1),
('Que vaut 2 puissance 10 ?', '512', '1024', '2048', 'B', 'C''est la base d''un kilooctet informatique.', 1),
('Quel périphérique affiche l''image à l''écran en calculant les pixels ?', 'La carte mère', 'La carte graphique (GPU)', 'L''alimentation', 'B', 'Indispensable pour jouer à 240 Hz.', 1),
('Quelle extension correspond à un fichier exécutable sous Windows ?', '.exe', '.dll', '.bat', 'A', 'Les trois premières lettres d''exécutable.', 1),
('Quel raccourci ouvre le Gestionnaire des tâches sous Windows ?', 'Ctrl + Maj + Echap', 'Alt + F4', 'Win + L', 'A', 'Il combine Contrôle, Majuscule et Échap.', 1),

--Salle 2
('Quel mot-clé C# permet de sortir immédiatement d''une boucle ?', 'stop', 'break', 'exit', 'B', 'Il signifie casser en anglais.', 2),
('Quel type C# stocke uniquement vrai ou faux ?', 'int', 'bool', 'string', 'B', 'Abréviation de booléen.', 2),
('En C#, par quel index commence toujours un tableau ou une liste ?', '1', '0', '-1', 'B', 'En informatique, on compte à partir de zéro.', 2),
('Quelle commande Git permet d''enregistrer ses modifications en local ?', 'git push', 'git commit', 'git pull', 'B', 'Elle valide un instantané du code.', 2),
('En SQL, quel mot-clé permet de filtrer les lignes d''un SELECT ?', 'FILTER', 'WHERE', 'ORDER BY', 'B', 'Il signifie Où en anglais.', 2),
('Quel mot-clé C# permet au compilateur de deviner le type d''une variable ?', 'auto', 'var', 'let', 'B', 'Trois lettres, abréviation de variable.', 2),
('Quelle instruction SQLite empêche une erreur si la table existe déjà ?', 'IF NOT EXISTS', 'OR IGNORE', 'UNIQUE', 'A', 'Littéralement : Si n''existe pas.', 2),
('Quel symbole permet d''écrire une chaîne multi-lignes en C# ?', '$', '@', '#', 'B', 'Le symbole arobase devant les guillemets.', 2),
('En C#, quelle méthode lit une ligne tapée par l''utilisateur dans la console ?', 'Console.WriteLine()', 'Console.ReadLine()', 'Console.ReadKey()', 'B', 'Read signifie Lire.', 2),
('Quelle contrainte SQL interdit qu''une colonne soit laissée vide ?', 'UNIQUE', 'NOT NULL', 'PRIMARY KEY', 'B', 'Pas nul en anglais.', 2),
('Quelle méthode C# exécute un INSERT ou un CREATE TABLE en SQLite ?', 'ExecuteReader()', 'ExecuteNonQuery()', 'ExecuteScalar()', 'B', 'NonQuery car la requête ne renvoie pas de tableau.', 2),
('Quel opérateur C# vérifie l''égalité entre deux valeurs ?', '=', '==', '!=', 'B', 'Un seul signe égal sert à l''affectation.', 2),

--Salle 3
('Quel port réseau est utilisé par défaut par le protocole HTTPS ?', '80', '21', '443', 'C', 'Le HTTP classique est sur le 80, le sécurisé sur le 443.', 3),
('Que signifie le terme Ping dans un jeu en ligne ?', 'La fréquence de l''écran', 'La latence réseau en millisecondes', 'Le nombre d''images par seconde', 'B', 'C''est le temps d''aller-retour vers le serveur.', 3),
('Quelle adresse IP désigne toujours la machine locale (localhost) ?', '192.168.1.1', '127.0.0.1', '8.8.8.8', 'B', 'Boucle locale commençant par 127.', 3),
('Quel protocole traduit un nom de domaine (ex: riotgames.com) en adresse IP ?', 'DHCP', 'DNS', 'FTP', 'B', 'Domain Name System.', 3),
('Combien d''octets compose une adresse IPv4 classique ?', '4', '6', '8', 'A', 'Quatre nombres séparés par des points.', 3),
('Quel protocole est privilégié par les FPS compétitifs pour sa rapidité ?', 'TCP', 'UDP', 'SMTP', 'B', 'Il envoie les paquets sans attendre d''accusé de réception.', 3),
('Que signifie l''acronyme DDoS ?', 'Distributed Denial of Service', 'Direct Data Operating System', 'Dynamic Domain Server', 'A', 'Attaque par déni de service distribué qui sature un serveur.', 3),
('Quelle commande permet de tester la connexion vers une adresse IP ?', 'ipconfig', 'ping', 'netstat', 'B', 'Le même nom que la latence en jeu.', 3),
('Quel équipement relie deux réseaux différents et dirige les paquets ?', 'Un switch', 'Un routeur', 'Un câble RJ45', 'B', 'Il trace la route des données.', 3),
('Quel port est utilisé par défaut pour une connexion sécurisée SSH ?', '22', '25', '8080', 'A', 'Deux fois le chiffre 2.', 3),
('Que signifie le sigle FPS en jeu vidéo ?', 'First Person Shooter', 'Fast Packet System', 'Frame Power Sync', 'A', 'Tir à la première personne (comme Valorant).', 3),
('Quelle fréquence de rafraîchissement serveur (tickrate) utilise Valorant ?', '64 Hz', '128 Hz', '240 Hz', 'B', 'Le double du tickrate standard de 64.', 3),

--Salle 4
('Combien de manches faut-il remporter pour gagner une partie standard sur Valorant ?', '12', '13', '16', 'B', 'En 24 manches maximum hors prolongation, il en faut 13.', 4),
('Combien de crédits coûte un Vandal ou un Phantom dans la boutique ?', '2700', '2900', '3200', 'B', 'Juste en dessous de 3000 crédits.', 4),
('Combien de secondes dure le compte à rebours du Spike avant explosion ?', '40 secondes', '45 secondes', '50 secondes', 'B', 'Il faut 7 secondes pour le désamorcer.', 4),
('Quel rôle d''agent est spécialisé dans l''ouverture de site et les duels ?', 'Sentinelle', 'Duelliste', 'Contrôleur', 'B', 'Jett, Reyna et Raze en font partie.', 4),
('Quel est le nom du championnat mondial annuel de Valorant ?', 'The International', 'Valorant Champions', 'World Series', 'B', 'L''événement final du circuit VCT.', 4),
('Quelle équipe française évolue en VCT EMEA avec un maillot bleu et noir ?', 'Karmine Corp', 'Team Vitality', 'Gentle Mates', 'A', 'Le Blue Wall.', 4),
('Combien de joueurs composent une équipe complète sur Valorant ?', '4', '5', '6', 'B', 'Un format classique en 5 contre 5.', 4),
('Quel agent contrôleur utilise des fumigènes célestes depuis une carte tactique ?', 'Omen', 'Brimstone', 'Viper', 'B', 'Il possède aussi un laser orbital.', 4),
('Quelle arme de poing est gratuite au début de chaque manche ?', 'Ghost', 'Classic', 'Sheriff', 'B', 'Elle possède un tir secondaire en rafale.', 4),
('Combien de temps prend le désamorçage complet du Spike (en deux moitiés) ?', '5 secondes', '7 secondes', '10 secondes', 'B', '3,5 secondes par moitié.', 4),
('Quelle carte de Valorant se déroule sous l''océan dans un dôme vitré ?', 'Pearl', 'Breeze', 'Fracture', 'A', 'Une carte située au Portugal.', 4),
('Quel est le rang le plus élevé sur Valorant ?', 'Immortel', 'Radiant', 'Ascendant', 'B', 'Le top 500 de chaque région.', 4),

--Salle 5
('Quel est le nom du système anti-triche développé par Riot Games ?', 'BattleEye', 'Vanguard', 'Easy Anti-Cheat', 'B', 'Il démarre au lancement de Windows.', 5),
('À quel niveau du système d''exploitation fonctionne le pilote de Vanguard ?', 'Espace utilisateur', 'Noyau (Kernel / Ring 0)', 'Navigateur web', 'B', 'Le niveau le plus profond du système.', 5),
('Comment appelle-t-on une attaque qui trompe l''utilisateur via un faux email ?', 'Brute Force', 'Phishing (Hameçonnage)', 'SQL Injection', 'B', 'Une technique de pêche aux identifiants.', 5),
('Quel procédé transforme un mot de passe en une empreinte irréversible ?', 'Le chiffrement AES', 'Le hachage (Hash)', 'La compression ZIP', 'B', 'Impossible de retrouver le mot de passe d''origine à partir du résultat.', 5),
('Comment appelle-t-on une faille de sécurité encore inconnue de l''éditeur ?', 'Zero-Day', 'Backdoor', 'Firewall', 'A', 'Les développeurs ont zéro jour pour la corriger.', 5),
('Quelle attaque consiste à injecter du code SQL malveillant dans un formulaire ?', 'Cross-Site Scripting', 'Injection SQL', 'Man-in-the-Middle', 'B', 'C''est pour l''éviter qu''on utilise Parameters.AddWithValue().', 5),
('Que signifie l''acronyme 2FA pour sécuriser un compte joueur ?', 'Two-Factor Authentication', 'Two-Firewall Access', 'Total File Authorization', 'A', 'Authentification à deux facteurs.', 5),
('Quel logiciel malveillant chiffre les fichiers d''un serveur pour exiger une rançon ?', 'Un Spyware', 'Un Ransomware', 'Un Adware', 'B', 'Ransom signifie rançon en anglais.', 5),
('Quel dispositif filtre le trafic réseau entrant et sortant d''un serveur ?', 'Un pare-feu (Firewall)', 'Un proxy', 'Un compilateur', 'A', 'Un mur contre les intrusions.', 5),
('Comment appelle-t-on un logiciel de triche qui vise automatiquement la tête ?', 'Wallhack', 'Aimbot', 'Triggerbot', 'B', 'Aim signifie viser en anglais.', 5),
('Quel protocole sécurise les échanges web avec un cadenas dans le navigateur ?', 'HTTP', 'HTTPS', 'Telnet', 'B', 'Le S final signifie Secure.', 5),
('Quelle bonne pratique consiste à mettre à jour un logiciel pour corriger une faille ?', 'Appliquer un patch (correctif)', 'Formater le disque', 'Overclocker le CPU', 'A', 'Les développeurs sortent des notes de patch.', 5);
";
insertInQuestions.ExecuteNonQuery();

Console.WriteLine("Quel est votre pseudo ?");

string pseudo = "";

while (string.IsNullOrWhiteSpace(pseudo))
{
   pseudo = Console.ReadLine() ?? "";
   
   if (string.IsNullOrWhiteSpace(pseudo))
   {
     Console.WriteLine("Le pseudo ne peut pas être vide"); 
   }
}

Console.WriteLine("Quel est votre mot de passe ?");

string mdp = "";

while (string.IsNullOrWhiteSpace(mdp) || mdp.Length <= 4)
{
   mdp = Console.ReadLine() ?? "";

   if (string.IsNullOrWhiteSpace(mdp))
   {
      Console.WriteLine("Le mot de passe ne peut pas être vide");
   }

   if (mdp.Length <= 4)
   {
      Console.WriteLine("Le mot de passe doit faire plus de 4 caractères");
   }
}

//Boucle de connexion

int idJoueur;
bool aPartie = false;

SqliteCommand verifcompte = connection.CreateCommand();
verifcompte.CommandText = @"select Id_joueur from Joueur 
                 where Pseudo = @pseudo
                 and Mot_de_passe = @mdp;";

verifcompte.Parameters.AddWithValue("@pseudo", pseudo);
verifcompte.Parameters.AddWithValue("@mdp", mdp);

object resultat = verifcompte.ExecuteScalar();

if (resultat != null)
{
   idJoueur = Convert.ToInt32(resultat);
   
   SqliteCommand verifparties = connection.CreateCommand();
   verifparties.CommandText = @"select Id_joueur from Partie where Id_joueur = @idJoueur;";
      verifparties.Parameters.AddWithValue("@idJoueur", idJoueur);
      
      object resultatParties = verifparties.ExecuteScalar();

      if (resultatParties == null)
      {
         aPartie = false;
         Console.WriteLine("");
         Console.WriteLine($"Bon retour dans Escape the Valorant Data Center {pseudo}");
         Console.WriteLine("Que voulez vous faire ?");
         Console.WriteLine("");
         Console.WriteLine("====================");
         Console.WriteLine("1- Demarrer une nouvelle partie");
         Console.WriteLine("====================");
      }
      else
      {
         aPartie = true;
         Console.WriteLine("");
         Console.WriteLine($"Bon retour dans Escape the Valorant Data Center {pseudo}");
         Console.WriteLine("");
         Console.WriteLine("Que voulez vous faire ?");
         Console.WriteLine("");
         Console.WriteLine("====================");
         Console.WriteLine("1- Demarrer une nouvelle partie");
         Console.WriteLine("2- Reprendre la partie en cour");
         Console.WriteLine("3- Historique des parties");
         Console.WriteLine("====================");
      }
   
   
}
else
{
   SqliteCommand insertjoueur  = connection.CreateCommand();
   insertjoueur.CommandText =@"insert into Joueur (Pseudo , Mot_de_passe) 
                               values (@pseudo, @mdp);";
   insertjoueur.Parameters.AddWithValue("@pseudo", pseudo);
   insertjoueur.Parameters.AddWithValue("@mdp", mdp);
   insertjoueur.ExecuteNonQuery();
   
   SqliteCommand getLastId = connection.CreateCommand();
   getLastId.CommandText = "SELECT last_insert_rowid();";
   idJoueur = Convert.ToInt32(getLastId.ExecuteScalar());
   
   aPartie = false;
   Console.WriteLine("");
   Console.WriteLine($"Bienvenue dans Escape the Valorant Data Center {pseudo}");
   Console.WriteLine("Que voulez vous faire ?");
   Console.WriteLine("");
   Console.WriteLine("====================");
   Console.WriteLine("1- Demarrer une nouvelle partie");
   Console.WriteLine("====================");
}

string menuResult = "";

if (aPartie)
{
   while (menuResult != "1" && menuResult != "2" && menuResult != "3")
   {
      menuResult = Console.ReadLine() ??"";
      
      if (menuResult == "1")
      {
         
      }
      
      if (menuResult == "2")
      {
         
      }
      
      if (menuResult == "3")
      {
         
      }
   }
}
else
{
   while (menuResult != "1")
   {
      menuResult = Console.ReadLine() ??"";

      if (menuResult == "1")
      {
         
      }
   }
}











