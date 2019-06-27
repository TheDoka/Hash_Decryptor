# Hash_Decryptor !1.1!

	/***                                                      
	 *     #     #                         ######                                                    ***\       
	 *     #     #   ##    ####  #    #    #     # ######  ####  #####  #   # #####  #####  ####  #####  
	 *     #     #  #  #  #      #    #    #     # #      #    # #    #  # #  #    #   #   #    # #    # 
	 *     ####### #    #  ####  ######    #     # #####  #      #    #   #   #    #   #   #    # #    # 
	 *     #     # ######      # #    #    #     # #      #      #####    #   #####    #   #    # #####  
	 *     #     # #    # #    # #    #    #     # #      #    # #   #    #   #        #   #    # #   #  
	 *     #     # #    #  ####  #    #    ######  ######  ####  #    #   #   #        #    ####  #    # 
	 *                                                        
	\*    #1.102                                                                                        */                                                    
 						Assembled by @TheDoka

	╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
	║                                                                                                    ║
	║    Calcul the given hash.                                                                          ║
	║        -c , -compute	 ENCRYPTION                                                                  ║
	║                                                                                                    ║
	║    Convert the given pass file into DB.                                                            ║
	║       -mk, -make  PASS_FILE, ENCRYPTION, OUTPUT                                                    ║
	║                                                                                                    ║
	║    Import the whole database in memory and try to reverse the hash.                                ║
	║        -f,  -find 	 DB_FILE, HASH_FILE, OUTPUT,  KEEP LINE NUMBER, USE H:P FORMAT		     ║
	║                                                                                                    ║
	║    Search hashes on LEA.KZ.                                                                        ║
	║        -w,  -web        HASH_FILE, TIMEOUT(ms), OUTPUT, KEEP LINE NUMBER, USE H:P FORMAT           ║
	║                                                                                                    ║
	╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
	*Avaible encryption methods : MD5, SHA1, SHA256, SHA256, SHA512*
 
A simple software that I made, very convenient and useful. Basicly it reverse any MD5, SHA1, SHA256, SHA512 hash to text, it'll use a  database that the software will create on his own to acomplish this.

It's CPU based, it won't use lot of CPU but it definitly will use your RAM. 

It can translate 14M passwords to SHA256 hashes in 23 minutes producing a file of around 1go. 

With an easier encryption, MD5, it took around 2 minutes to translate 14M passwords resulting a 600mo file.

Comparing 2.5K hashes to a 63M hashes database, so 157 billions of operations took about one hour.
Note that you don't always need to compare the whole database, if there no hashes left.

By the way you may encounter UI issues due to the progressbar, just increase the window size a bit. 

Configuration used for the tests : WIN10, I7 920 @3.5GHz, 12GB ~667MHz.

<details>
	<summary>Making a 14M MD5 dictionnary:</summary>
	
![MAKING 14M MD5 DICTIONNARY](https://i.imgur.com/nndEEmZ.png)

![MAKING MD5 DONE](https://i.imgur.com/KXWw2Hn.png)

</details>

 <details>
 <summary>Making a 14M SHA256 dictionnary:</summary>
	
![MAKING 14M SHA256 DICTIONNARY](https://i.imgur.com/zrC6LJv.png)

While making dictionnary:

![MAKING SHA256 PERFORMANCE](https://i.imgur.com/k4lTqNb.png)

...

![MAKING SHA256 END](https://i.imgur.com/Dr9o6XG.png)

</details>

 <details>
 <summary>Searching 2.5K 10ms timeout with LEA.KZ API:</summary>
	
[OLD VERSION BUT STILL ACURATE]
![SEARCHING FROMWEB LEA.KZ](https://i.imgur.com/zKdpznj.png)

...

![SEARCHING FROMWEB LEA.KZ-END](https://i.imgur.com/S7aEfyk.png)
</details>


