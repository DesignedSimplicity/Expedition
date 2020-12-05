# Expedition `.NET Framework, C#, Console, WinForm`

## A simple set of applications to take a snapshot of a file system

Keeping track of multiple backup copies of file system directories can be difficult.  These simple utilities streamline that process for my personal workflow. Utilities are named along the theme of an Expedition which itself a play on the Windows Explorer name.

![Expedition](./Documentation/Navigator/Main.png)

### [Scout](./Documentation/Scout/README.md)

#### Create/verify md5 and sh1 file content hashes from the command line

The primary goal was to be able to create and/or verify file hashes to ensure the integrity of file copy/move operations.  Scout recursively creates a list of md5 or sha1 hashes for all files in a directory.

The secondary goal was to create an easily consumable report of the hashing or verification process.  Scout can create an Excel document which details the file properties and status for troubleshooting or future reference.

### [Navigator](./Documentation/Navigator/README.md)

#### Generate and browse snapshots of file metadata (size/date/etc)

Additional goals will be accomplished with new utilities built on an as needed basis.  Lookout is a simple Windows Forms application to visualize a file system tree while providing functionality to recursively flatten and regroup the files contained in a directory and its subdirectories. This is used to strategize backup procedures and refactor file system structures.

##### TODO

1. Optionally output summary into footer of MD5 file

1. Service to compare two MD5 files for add/remove/change

1. Test/support passing in a file name to create file (ie: Prefix_YYYYMMDD)

1. Display detailed aggregate console report at end - total dir/file size/type/count, min/max/avg size + speed - top 10 largest dirs/files

1. Build multithreaded windows list view UI to report real time console output in data grid with columns and constantly updated aggregated report with a progressbar/timeline/graph
