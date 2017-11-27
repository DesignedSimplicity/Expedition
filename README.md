# Expedition
A simple application to take a snapshot of a file system and create or verify md5/sh1 file hashes

Goals of this project:
1. Create and verify md5 and sh1 file content hashes
2. Snapshot file metadata (size/date/etc) for offline backups


Usage
Expedition.exe C:\Test\	-md5						Creates an md5 file named C:\Test\_YYYYMMDDHHMMSS.md5 for all files in directory recursively
Expedition.exe C:\Test\ -sh1						Uses the sh1 algorithm and output file type

Expedition.exe C:\Test\ C:\MyFileName.md5			Creates an md5 file named C:\MyFileName.md5 and makes the paths relative to this location
Expedition.exe C:\Test\ C:\MyFileName.sh1			Uses the sh1 algorithm and output file type

Expedition.exe										Looks for an md5 or sh1 file in the current directory to verify
Expedition.exe C:\MyFileName.md5					Verifies the md5 or sh1 file referenced against the current directory
Expedition.exe C:\MyFileName.md5 C:\Test