# Expedition `.NET Framework, C#, Console, WinForm`

## A simple set of applications to take a snapshot of a file system

Keeping track of multiple backup copies of file system directories can be difficult.  These simple utilities streamline that process for my personal workflow.

The primary goal was to be able to create and/or verify file hashes to ensure the integrity of file copy/move operations.  Scout recursively creates a list of md5 or sha1 hashes for all files in a directory.

The secondary goal was to create an easily consumable report of the hashing or verification process.  Scout can create an Excel document which details the file properties and status for troubleshooting or future reference.

Additional goals will be accomplished with new utilities built on an as needed basis.  Lookout is a simple Windows Forms application to visualize a file system tree while providing functionality to recursively flatten and regroup the files contained in a directory and its subdirectories. This is used to strategize backup procedures and refactor file system structures.

![Expedition](./Documentation/Lookout/Main.png)

### [Scout](./Documentation/Scout/README.md)
Create and/or verify md5 and sh1 file content hashes from the command line.

### [Lookout](./Documentation/Lookout/README.md)
Manage creation and verification output with simple Windows user interface.

#### Work in progress

##### Navigator
Load and save snapshots of file metadata (size/date/etc) for offline backups.

##### Commander
Compare file system snapshots against each other or to current live file data.
