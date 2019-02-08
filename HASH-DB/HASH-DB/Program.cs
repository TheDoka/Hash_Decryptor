/*
 * Created by SharpDevelop.
 * User: Doka
 * Date: 26/07/2018
 * Time: 15:57
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HASH_DB
{
	class Program
	{
		static Int64 current_block;
        static bool empty_list = false;

		public static void Main(string[] args)
		{

            Console.Title = "Hash Decryptor";
			
			#region UI
			write_color(@"
	/***                                                      
	 *     #     #                         ######                                                    ***\       
	 *     #     #   ##    ####  #    #    #     # ######  ####  #####  #   # #####  #####  ####  #####  
	 *     #     #  #  #  #      #    #    #     # #      #    # #    #  # #  #    #   #   #    # #    # 
	 *     ####### #    #  ####  ######    #     # #####  #      #    #   #   #    #   #   #    # #    # 
	 *     #     # ######      # #    #    #     # #      #      #####    #   #####    #   #    # #####  
	 *     #     # #    # #    # #    #    #     # #      #    # #   #    #   #        #   #    # #   #  
	 *     #     # #    #  ####  #    #    ######  ######  ####  #    #   #   #        #    ####  #    # 
	 *                                                        
	\*    #1.101                                                                                       */                                                    
 						Assembled by @TheDoka
 			",1,ConsoleColor.Yellow);

			#endregion
			
			// -c 	|| -compute HASH
			// -mk 	|| -make 	PASS_FILE TYPE OUTPUT
			// -f	|| -find 	DB_FILE HASH_FILE
            // -w   || -web     HASH_FILE TIMEOUT OUTPUT KP HP
			try {
				
			string[] commands = new string[] {"-c", "-compute", "-mk", "-make", "-f", "-find", "-w", "-web", "-?"}; // #1.02
            string[] support_hash = new string[] { "MD5", "SHA1", "SHA256", "SHA512" };

			if (args.Length >=1 && commands.Any(args.Contains)) {

                #region ARGS compute
                    if (args[0].Equals("-c") || args[0].Equals("-compute"))
				{
					string sucess = null;
					switch (args[1])
					{
					case "MD5": Console.WriteLine(sucess = MD5Hash(args[2])); 	    break;
					case "SHA1": Console.WriteLine(sucess = SHA1Hash(args[2])); 	break;
					case "SHA256": Console.WriteLine(sucess = SHA256Hash(args[2])); break;
					case "SHA512": Console.WriteLine(sucess = SHA256Hash(args[2])); break;
					}
					if (sucess.Equals("1")) {go_error();}
				 }
				#endregion
				
				#region ARGS MAKE
				// -mk, -make       {PASS_FILE, MD5 SHA1 SHA256, SHA512, OUTPUT} 
				if (args[0].Equals("-mk") || args[0].Equals("-make"))
				{
                    
                    if (!File.Exists(args[1])) { go_error(); }
                    if (!support_hash.Any(args[2].Equals)) { go_error(); }         //#1.1

                    int sucess = MakeDictionnary(args[1],args[2],args[3]);
					if (sucess != 0) {go_error();}

				}
                #endregion

                #region ARGS FIND
                //-f,  -find       {DB_FILE, HASH_FILE, OUTPUT,           |       Try to reverse hash from DB.
                //			       KEEP LINE NUMBER, USE H:P FORMAT}
                if ((args[0].Equals("-f") || args[0].Equals("-find")))
                {
                    if (!File.Exists(args[1]) || !File.Exists(args[2])) { go_error(); }
                    bool low_memory = false;
                    //foreach (string arg in args) { if (arg == "--low_memory") { low_memory = true; } }        //CHECK ARG LOW MEMORY 
                    int sucess = CheckDictionnary(args[1], args[2], args[3], int.Parse(args[4]), int.Parse(args[5]), low_memory);
					if (sucess != 0) {go_error();}
                }
                #endregion

                #region FROM_WEB
                if ((args[0].Equals("-w") || args[0].Equals("-web")))
                    {
                        
                        //-w HASH_FILE.txt 100 XX.TXT 0 1
                        if (!File.Exists(args[1])) { go_error(); }
                        string hash_file = args[1];
                        int timeout = int.Parse(args[2]);
                        string output = args[3];
                        int keep_line_number = int.Parse(args[4]);
                        int hp_format = int.Parse(args[5]);

                        List<string> hashes = new List<string>();

                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write("\r [*] Importing {0}...", hash_file);
                        var lines = File.ReadLines(hash_file);
                        Int64 startup_hash_count = lines.Count();
                        foreach (string line in lines) { hashes.Add(line); }        // AJOUTE TOUT LES HASHES A L'ARRAY
                        Console.Write("\r [+] Importing done.                                                            ");
                        Console.WriteLine();

                        Console.WriteLine(" [*] Now Waiting for {0} responses... \n",startup_hash_count);
                        Console.ResetColor();
                        var stopw = Stopwatch.StartNew();
                        

                            var x = Task.Factory.StartNew(() => monitor_ui(startup_hash_count));
                                Parallel.ForEach(hashes, hash =>
                                {
                                    FROM_WEB(hash, output, keep_line_number, hp_format);
                                    Interlocked.Increment(ref current_block);
                                    System.Threading.Thread.Sleep(timeout);
                                });

                        var toFileTotalMs = stopw.Elapsed;
                        while (x.Status.ToString() != "RanToCompletion") { } //Secure text

                        Console.WriteLine("\n\n[+] All hashes have been checked in {0}.", toFileTotalMs.ToString("mm\\:ss\\.ff"));
                        Console.ReadKey();
                        System.Environment.Exit(1);

                    }
                #endregion

            //      END
            
			Console.Beep();
			Console.ResetColor();
			Console.ReadKey();
			} else { go_error(); }
			
			} catch (Exception) {go_error(); }          // GLOBAL UNEXEPTED ERROR

        }

        #region Calcule de HASH

        public static string MD5Hash(string tohash)
	{
			try {
					
				StringBuilder hashy = new StringBuilder();
				MD5CryptoServiceProvider MD5P = new MD5CryptoServiceProvider();
				byte[] bytes = MD5P.ComputeHash(new UTF8Encoding().GetBytes(tohash));
				
				for (int i =0; i < bytes.Length; i++)
				{
					hashy.Append(bytes[i].ToString("x2"));
				}
				return hashy.ToString();
									
			} catch (Exception) {
				return tohash; //SI ERREUR RETOURNE HASH
			}
				
	}
	
			public static string SHA1Hash(string tohash)
	{
			try {
					
				StringBuilder hashy = new StringBuilder();
				SHA1CryptoServiceProvider XX = new SHA1CryptoServiceProvider();
				byte[] bytes = XX.ComputeHash(new UTF8Encoding().GetBytes(tohash));
				
				for (int i =0; i < bytes.Length; i++)
				{
					hashy.Append(bytes[i].ToString("x2"));
				}
				return hashy.ToString();
				
			} catch (Exception) {
				return tohash; //SI ERREUR RETOURNE HASH
			}
				
	}
			
			public static string SHA256Hash(string tohash)
	{
			try{
					
				StringBuilder hashy = new StringBuilder();
				SHA256CryptoServiceProvider XX = new SHA256CryptoServiceProvider();
				byte[] bytes = XX.ComputeHash(new UTF8Encoding().GetBytes(tohash));
				
				for (int i =0; i < bytes.Length; i++)
				{
					hashy.Append(bytes[i].ToString("x2"));
				}
				return hashy.ToString();
				
			} catch (Exception) {
					return tohash; //SI ERREUR RETOURNE HASH
			}
				
	}

			public static string SHA512Hash(string tohash)
	{
			try{
					
				StringBuilder hashy = new StringBuilder();
				SHA512CryptoServiceProvider XX = new SHA512CryptoServiceProvider();
				byte[] bytes = XX.ComputeHash(new UTF8Encoding().GetBytes(tohash));
				
				for (int i =0; i < bytes.Length; i++)
				{
					hashy.Append(bytes[i].ToString("x2"));
				}
				return hashy.ToString();
				
			} catch (Exception) {
					return tohash; //SI ERREUR RETOURNE HASH
			}
				
	}

            public static void FROM_WEB(string hash, string output, int keep_line_number, int hp_format)
            {
            string left_block = null;
                try
                {
                    WebClient client = new WebClient();
                    string downloadString = client.DownloadString("https://lea.kz/api/hash/" + hash);
                    string[] info = downloadString.Split('"');

                #region display_match
                string msg = string.Format(" [+] {0} == {1}", hash, info[3]);
                if (msg.Length < 120) { left_block = string.Concat(Enumerable.Repeat(" ", 120 - msg.Length)); }
                write_color(string.Format("\r" + msg + left_block + "\n"), 0, ConsoleColor.DarkGray);
                #endregion

                if (hp_format != 1)
                    {
                       log_it(output, info[3]);        // LOG NORMAL
                    }
                    else { log_it(output, string.Format("{0}:{1}", hash, info[3])); }   // LOG HASH:EQUALS
                
                }catch (Exception)
           

            {
                //Console.WriteLine(" [-] {0}:?", hash);
                //if (keep_line_number == 1) { log_it(output, null); }
            } // SI FAUT MET SAUTE UNE LIGNE FORMAT HP



        }

        #endregion

        #region Gestion des dictionnaires

        public static int MakeDictionnary(string file, string type, string output)
        {

            // FILE : TXT TO TRANSLATED		
            // TYPE : MD5, SHA1, SHA256
            //OUTPUT: OUTPUT FILE

            try
            {

                List<string> import = new List<string>();

                using (StreamWriter txt = new StreamWriter(output))
                {

                    Console.ForegroundColor = ConsoleColor.Magenta;

                    Console.Write("\r [*] Scanning {0}...",file);
                    var lines = File.ReadLines(file);
                    int all_lines = lines.Count();
                    //foreach (string line in lines) { import.Add(line); } // IMPORT DANS L'ARRAY, Si fichier trop lourd c'est la merde mec
                    Console.Write("\r [+] Scanning done...                                                         ");
                    Console.Write("\n [+] Now translating {0} txt...                                                         ",all_lines);
                    Console.WriteLine("\n");
                    Console.ResetColor();

                    int calculed = 0;
                    var x = Task.Factory.StartNew(() => monitor_ui(all_lines));
                    var stopw = Stopwatch.StartNew();

                    string hash = null;

                    foreach (string line in lines)
                    {

                        switch (type)
                        {
                            case "MD5": hash = MD5Hash(line); break;
                            case "SHA1": hash = SHA1Hash(line); break;
                            case "SHA256": hash = SHA256Hash(line); break;
                            case "SHA512": hash = SHA512Hash(line); break;
                        }

                        calculed++; 
                        current_block = calculed;
                        txt.WriteLine("{0}:{1}", hash, line);

                        //string quote = @"'";
                        //if (!line.Contains(quote))
                        //{
                        //    txt.WriteLine("('{0}', '{1}'),", hash, line); // SQL FORMAT }
                        //}
                        
                    }


                    #region DONE.
                    var toFileTotalMs = stopw.Elapsed;
                    Console.WriteLine("\n[+] Generation done in {0}, converting {1} hashes.", toFileTotalMs.ToString("mm\\:ss\\.ff"), lines.Count());
                    #endregion
                }
                return 0;

            } catch (Exception) { return 1; }



        }

        public static int CheckDictionnary(string db_file, string hash_file, string output, int keep_line_number, int hp_format,bool low_memory)
		{

            // DB_FILE : DB FILE
            // HASH_FILE : FILE CONTENING HASHES

            try {

                if ((File.Exists(db_file) & File.Exists(hash_file)) == false) { return 1; }


                List<string> all_hash = new List<string>();
                List<string> database = new List<string>();

                Console.ForegroundColor = ConsoleColor.Magenta;

                #region IMPORTS

                    Console.Write("\r [*] Importing hash file: '{0}'...", hash_file);

                        var lines = File.ReadLines(hash_file);
                        foreach (string line in lines) { all_hash.Add(line); }        // AJOUTE TOUT LES HASHES A L'ARRAY

                        // Supprime les doublons
                        all_hash = all_hash.Distinct().ToList();
                        Int64 startup_hash_count = all_hash.Count;

                    Console.Write("\r [+] Hash file: '{0}' imported.                                                                ",hash_file);
                    Console.WriteLine();
                    Console.Write("\r [*] Scanning database '{0}'...", db_file);

                        var db_lines = File.ReadLines(db_file);
                        Int64 all_db = db_lines.Count(x => !string.IsNullOrWhiteSpace(x));

                    Console.WriteLine("\r [+] Scanning done.                                                            ");
                    Console.ResetColor();
                    Console.WriteLine();

                #endregion

                var stopw = Stopwatch.StartNew();

                    
                    var y = Task.Factory.StartNew(() => monitor_ui(all_db)); // MONITORING UI.


                                Int64 calculed = 0;
                                Int64 pourcentage_done;

                    foreach (string line in db_lines)
                    {

                        //TANT QU'IL Y A DES HASHES
                        if (all_hash.Count != 0)
                        {

                            Interlocked.Increment(ref calculed);
                            current_block = calculed; // ACTUALISE L'UI.

                            //SI MATCH DANS L'ARRAY	
                            if (all_hash.Any(line.Split(':')[0].Equals))
                            {

                                // #1.02 DISPLAY MATCH
                                    #region display_match
                                    
                                        //string msg = string.Format(" [+] {0} == {1} at {2}/{3}", line.Split(':')[0], line.Split(':')[1], calculed, all_db);
                                        //if (msg.Length < 120) { left_block = string.Concat(Enumerable.Repeat(" ", 120 - msg.Length)); }
                                        //write_color(string.Format("\r" + msg + left_block + "\n"), 0, ConsoleColor.DarkGray);

                                    #endregion

                                    if (hp_format != 1)
                                    {
                                        log_it(output, line.Split(':')[1]);
                                    } else {
                                        log_it(output, string.Format("{0}:{1}", line.Split(':')[0], line.Split(':')[1]));
                                    }

                                    all_hash.Remove(line.Split(':')[0]); //Supprime le hash de l'array. #1.01

                            } else {

                                    //NO MATCH
                                    //Console.WriteLine("No Match: {0}", line);
                                    if (keep_line_number == 1) { log_it(output, null); }

                            } 

                        } else { empty_list = true; break; } // NO HASH LEFT

                    }


                    var toFileTotalMs = stopw.Elapsed;
                    Int64 pourcentage = ((calculed * 100) / all_db);

                    //#1.02 FIX POURCENTAGE, SI N'A PAS BOUGER = AUCUN HASH 1/1 0%, SINON CALCULE SUR LE FAIT/TOTAL
                    if (all_hash.Count == startup_hash_count)
                        {

                            pourcentage_done = 0;

                        } else {

                                    if (all_hash.Count != 0)
                                    {
                                        pourcentage_done = 100 - ((all_hash.Count-1) * 100) / startup_hash_count;
                                    } else {
                                        pourcentage_done = 100;

                                    }

                        }

                    while (y.Status.ToString() != "RanToCompletion") { } //Secure text

                    Console.WriteLine("\n\n[+] All have been checked in {0}. Found {1}% of hashes. {2}% of database has been checked.", toFileTotalMs.ToString("mm\\:ss\\.ff"), pourcentage_done, pourcentage);

                    return 0;

            



            } catch (Exception) {return 1;}

        }
		
		#endregion
		
		#region TOOLS
		
        public static void write_color(string text, int type, ConsoleColor color, int position = 0)
        {
            Console.ForegroundColor = color;
            switch (type)
            {
                case 0: Console.Write(text);        break;
                case 1: Console.WriteLine(text);    break;
                case 2:
                    Console.SetCursorPosition(0, Console.CursorTop - position);
                    Console.Write("\r" + text);
                    break;
                case 3:
                    Console.SetCursorPosition(0, Console.CursorTop - position);
                    Console.WriteLine("\r" + text);
                    break;
            }
            Console.ResetColor();
            
        }

		public static void go_error()
		{
			Console.WriteLine("[!] An error as occured, please check arguments.");
            Console.WriteLine(@"
        ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
        ║                                                                                                    ║
        ║    Calcul the given hash.                                                                          ║
        ║        -c , -compute	 ENCRYPTION                                                                  ║
        ║                                                                                                    ║
        ║    Convert the given pass file into DB.                                                            ║
        ║       -mk, -make  PASS_FILE, ENCRYPTION, OUTPUT                                                    ║
        ║                                                                                                    ║
        ║    Import the whole database in memory and try to reverse the hash.                                ║
        ║        -f,  -find 	 DB_FILE, HASH_FILE, OUTPUT,  KEEP LINE NUMBER, USE H:P FORMAT               ║
        ║                                                                                                    ║
        ║    Search hashes on LEA.KZ.                                                                        ║
        ║        -w,  -web        HASH_FILE, TIMEOUT(ms), OUTPUT, KEEP LINE NUMBER, USE H:P FORMAT           ║
        ║                                                                                                    ║
        ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝

 *Avaible encryption methods : MD5, SHA1, SHA256, SHA256, SHA512*
					");
        
            Console.Beep();
			Console.ResetColor();
			System.Environment.Exit(1);
		}
		
		public static void monitor_ui(Int64 end_block)
		{
            string block = null;
            string msg = null;
            string left_block = null;

            Int64 pourcentage;
            long end_block_ = Convert.ToInt32(end_block);
            bool completed = false;

            do
            {
                if (empty_list == true) { return; }

                 pourcentage = ((current_block * 100) / end_block_);
                 block = string.Concat(Enumerable.Repeat("█", Convert.ToInt32(pourcentage))) + string.Concat(Enumerable.Repeat("░", 100 - (Convert.ToInt32(pourcentage))));
                 msg = string.Format("\r[{0}] {1}% {2}/{3}", block, pourcentage, current_block.KiloFormat(), end_block_.KiloFormat());
                if (msg.Length < 120) { left_block = string.Concat(Enumerable.Repeat(" ", 120 - msg.Length)); }
                Console.Write(msg + left_block);
                if (pourcentage == 100) { completed = true; }

            } while (completed != true);

            return;
		}

        private static object locker = new Object();

        public static void log_it(string filename, string content)
		{
            lock (locker)
            {
                using (FileStream file = new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.Read))
                using (StreamWriter writer = new StreamWriter(file, Encoding.UTF8))
                {
                    writer.WriteLine(content.ToString());
                }
            }
        }
		
    	} // CLASS PROGRAM

    public static class Extensions
		{

            public static string[] TrySplit(this string to_split, char split_char)
            {
            try { return to_split.Split(split_char); } catch (Exception) { return null; }

            }

    		public static string KiloFormat(this Int64 num)
    		{
        		if (num >= 100000000)
            		return (num / 1000000).ToString("#,0M");

       		 if (num >= 10000000)
           		 return (num / 1000000).ToString("0.#") + "M";

      		  if (num >= 100000)
          		  return (num / 1000).ToString("#,0K");

       		 if (num >= 10000)
          		  return (num / 1000).ToString("0.#") + "K";
		
      		  return num.ToString("#,0");
   		 	} 
		}
    
    #endregion



}