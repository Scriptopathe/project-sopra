import sys
import shutil

print("Configuration switch utility.")

mode = input("mode [save | restore]: ")
suffix = input("suffix: ")

if mode == "save":
	print("Copy current configuration to " + suffix)
else:
	print("Replace current configuration by " + suffix)

confirm = input("type 'ok' then press enter to confirm: ")
if(confirm != "ok"):
        input("Operation canceled, press enter to exit.")
        sys.exit()

proj = "SopraProject.csproj"
packages = "packages.config"
web1 = "Web.config"
web2 = "Views/Web.config"

files = [proj, packages, web1, web2]

if(mode == "save"):
	for file in files:
		shutil.copy(file, file + "." + suffix)
else:
	for file in files:
		shutil.copy(file, file + ".backup")
		shutil.copy(file + "." + suffix, file)
		
