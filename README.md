# Hash_Decryptor !1.03!
A simple software I made, very convenient and useful. Basicly It's for reverse any MD5, SHA1, SHA256, SHA512 hash to text, but not magical it'll use a database create by his own.
I'll may be release the source code if you guys want, but nothing that interesting. I can easily upgrade it to support any encryption type.
It's single thread and CPU based, don't worry it wouldn't drain your CPU that much.

It can translate 14M passwords to SHA256 hashes in 35 minutes producing a file of around 1go. With an easier encryption, MD5, it took around 2 minutes to translate 14M passwords resulting a 600mo file.

For searching hashes, comparing 2.5K hashes * 14M hashes database. It took around 15 minutes to finish all comparisons! Note that you don't always need to compare the whole database, if there no hashes left.

By the way you may encounter UI issues due of the progressbar, just increase the window size a bit. 

Configuration used for the tests : WIN10, I7 920 @3.5GHz, 12GB ~667MHz.

        /***
         *     #     #                         ######                                                    ***\
         *     #     #   ##    ####  #    #    #     # ######  ####  #####  #   # #####  #####  ####  #####
         *     #     #  #  #  #      #    #    #     # #      #    # #    #  # #  #    #   #   #    # #    #
         *     ####### #    #  ####  ######    #     # #####  #      #    #   #   #    #   #   #    # #    #
         *     #     # ######      # #    #    #     # #      #      #####    #   #####    #   #    # #####
         *     #     # #    # #    # #    #    #     # #      #    # #   #    #   #        #   #    # #   #
         *     #     # #    #  ####  #    #    ######  ######  ####  #    #   #   #        #    ####  #    #
         *
	\*    #1.03                                                                                               */           
                                                Assembled by @TheDoka

			/`Commands`\						/`Usage`\
			
	-c , -calcul	 {ENCRYPTION}			        |	Calcul the given hash.
                                                                |
	-mk, -make	 {PASS_FILE, ENCRYPTION, OUTPUT}	|  	Convert the given pass file into DB.
                                                                |
	-f,  -find 	 {DB_FILE, HASH_FILE, OUTPUT, 		|	Try to reverse hash from DB.
			    KEEP LINE NUMBER, USE H:P FORMAT}	|  
                                                                |
	-w,  -web        {HASH_FILE, TIMEOUT(ms), OUTPUT,       |       Search hash on lea.kz.
			    KEEP LINE NUMBER, USE H:P FORMAT}   |

 *Avaible encryption methods : MD5, SHA1, SHA256, SHA256, SHA512*

