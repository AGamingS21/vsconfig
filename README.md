# vscode-config

## TODO
- profiles
    - ~~install/uninstall extensions to profile~~
    - ~~keybinds~~
    - ~~settings~~
    - ~~tasks~~
    - ~~snippets~~
    - ~~create relevant folders if they do not exist~~
    - ~~edit storage.json~~
- CLI Commands
    - ~~Help~~
    - ~~Default which should just be help~~
    - ~~verion~~
    - ~~run~~
    - ~~option for changing which one default code~~
- Parameterize:
    - ~~Based on release vs debug select different directories to read files from~~
- Different cli tools:
    - ~~VSCode-Oss~~
    - ~~vscode~~
    - ~~vscodium~~
- Better Comments
- Console outputs
- Fix:
    - gloablStorage folder needs to exist on first install
- MakeFile for publish
- docs
- tests


- Possible features
    - two way sync: make it possible for changes in vscode to be made within config file
        - when uninstalling extensions remove extensions that were previously installed but are nolonger in vs code from config
        - when installing add extensions to config that were not in config but are now in vscode
    - create config based on what is already installed on vscode
    - xvg data directory


## Logic
### Installing
- If ext is in config file but not in vscode then install add to output file
- if ext is installed and not in config then write it back to the config file
- if ext is already installed then do not attempt to install and include in output file
- if ext is installed in config and installed in output then do not try and reinstall


### Uninstalling
- if ext is not in the config but in output file then uninstall
- if ext is in config and output file as installed successfully but is not in vscode then remove from both files
- if already uninstalled then do not attmpt to uninstall and remove from config file.