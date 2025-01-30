# vsconfig

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
- ~~Better Comments~~
- ~~Console outputs~~
- ~~MakeFile for publish~~
- docs
- ~~change name to vsconfig~~


- roadmap
    - two way sync: make it possible for changes in vscode to be made within config file
        - when uninstalling extensions remove extensions that were previously installed but are nolonger in vs code from config
        - when installing add extensions to config that were not in config but are now in vscode
    - create config based on what is already installed on vscode
    - xvg data directory
    - tests
    - override settings for Default profile
    - error handling
    - multiple profiles per json file
    - support for multiple config files: json, toml, yaml
    - support mac, windows and linux
    - automated deployment
