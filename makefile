default:	
	rm -f ~/.local/bin/vsconfig
	dotnet publish ./src/vsconfig.csproj -c Release --self-contained true
	cp ./src/bin/Release/net9.0/linux-x64/publish/vsconfig ~/.local/bin
