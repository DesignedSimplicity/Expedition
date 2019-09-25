![Start](./Documentation/Scout/1-Start.png)
![Create MD5](./Documentation/Scout/2-Create.png)
![Verify MD5](./Documentation/Scout/3-Verify.png)
![Excel Report](./Documentation/Scout/4-Report.png)

Scout.exe						Prompts to verify any/all md5|sha1 files in current directory then prompts to create a new md5 file of current directory

Scout.exe .						Auto-creates a new md5 file of the current directory
Scout.exe C:\Folder\			Creates a new md5 file of the specified directory relative to the current directory

Scout.exe File.md5				Verifies an existing md5 file against relative to current directory, prompt to create if does not exist
Scout.exe File.sha1				Verifies an existing sha1 file against relative to current directory, prompt to create if does not exist

Switches
--------
-sha1							Uses the sha1 hash and file output type (-1)
-abolute						Creates absolute file paths in hash file (-a)
-preview						Executes the request without generating hashes (-p)
-verbose						Ouputs detailed summary after hash creation or verification (-v)



Scout.exe

Scout /new					Creates a recursive xpdb file for everything in current directory
Scout /md5					Creates a recursive md5 hash file for everything in current directory
Scout /sha1					Creates a recursive sha1 hash file for everything in current directory
Scout /tree					Creates a recursive xpdb file for everything in current directory but does not generate hashes (only stores file system info)



Scout /v					Auto verifies most recently created md5, sha1 or xpdb and displays summary in console

Scout input.xpdb			Verifies contents of input.xpdb and displays summary in console
Scout input.md5 /r			Verifies contents of input.md5 and creates detailed xpdb report output

Scout /e					Write detailed error log
