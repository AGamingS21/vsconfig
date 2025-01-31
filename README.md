# vsconfig
This cli is designed to configure vscode based on readible files. This is an alternative to the settings sync that vscode offers. This is for the people that want to store their text editor configs in a repo with other dotfiles.

Note: Be careful with storing any sensitive information in your settings.json file such as server names, usernames, passwords, etc. Some extensions may store these details and if you chose to have your dotfiles public then those settings may be stored. To prevent this issue make sure that none of those secrets are in the profiles.

## Installation
### Prerequisites
This has only been tested on Linux Mint and Arch Linux. There will be support for other operating systems in the future.

### From source
The only way to install right now is to clone this repository and run ```make``` this will publish the app as a single binary and then move it to ~./local/bin.

Once done just run ```vsconfig``` to get started

## Configuration
vsconfig uses json files(1 per profile) to build any profiles extensions, keybindings, settings, name. To see some examples check out the [homelab](./src/profiles/homelab.json) and [dotnet](./src/profiles/dotnet.json) json files.

There can be as many json files in the folder as required.

The default location for these config files is located at ~/.config/vsconfig/. That can be overwritten using the --path flag.

## Contributing
Create a github issue or feel free to put in a pr. All feedback is welcome.

## Roadmap
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
- automated deployment/release
- improved docs