:: Replaces the stripped system library files of Gorilla Tag
:: with the unstripped ones in the Managed folder
:: 
xcopy "Managed\*.dll" "..\Gorilla Tag_Data\Managed\" /K /H /Y
