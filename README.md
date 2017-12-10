# Expedition
A simple set of applications to take a snapshot of a file system and create or verify md5/sh1 file hashes


Scout
=====
Create and verify md5 and sh1 file content hashes from the command line

Lookout
=======
Manage creation and verification output with simple Windows user interface

Navigator
=========
Load and save snapshots of file metadata (size/date/etc) for offline backups

Commander
=========
Compare file system snapshots against each other or to current live file data



Useage
~~~~~~
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