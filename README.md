# Prepend
```
            Usage: prepend [Command] 
                    --folder-path=<value> 
                    --prepend-text=<value>
                    --file-number-seed=<value>

            Commands
                --help                        Print help

                --remove                      Removes matching prepend-text

            Parameters
                --folder-path=<value>         Folder path/File pattern for affected files

                --prepend-text=<value>        Prepend text pattern

                --file-number-seed=<value>    Will replace any instances of '#' in 
                                                the filenmae with a number that is incremented 
                                                for each file within the folder.

                                              Default = 1


            Examples:
                prepend --folder-path=D:\Files\*.mkv --prepend-text=""s01e### - "" --file-number-seed=0
                prepend --folder-path=D:\Files\*.mkv --prepend-text=""s01e### - "" --remove
```
