# Hash_Decryptor
A simple software I made, very convenient and useful. Basicly It's for reverse any MD5, SHA1, SHA256, SHA512 hash to text.
I'll may be release the source code if you guys want, but nothing that interesting. I can easily upgrade it to support any encryption type.
It's single thread, so it's a bit slow, but there not that needs to upgrade. 

It can translate 14M passwords to SHA256 hashes in 35 minutes producing a file of around 1go. With an easier encryption, MD5, it took around 2 minutes to translate 14M passwords resulting a 600mo file.

For searching hashes, comparing 2kh*14M hashes DB, it's long. It took around 1h30 to finish all comparison.

By the way you may encounter UI issues, just increase the window size a bit. 

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
        \*                                                                                                 */           
                                                Assembled by @TheDoka


                       /`Commands`\                                            /`Usage`\
                       
        -c , -calcul     {MD5,SHA1,SHA256}                      |       Calcul the given hash.
        -mk, -make       {PASS_FILE, MD5 SHA1 SHA256, SHA512, OUTPUT}   |       Convert the given pass file into DB.
        -f,  -find       {DB_FILE, HASH_FILE, OUTPUT,           |       Try to reverse hash from DB.
                            KEEP LINE NUMBER, USE H:P FORMAT}
