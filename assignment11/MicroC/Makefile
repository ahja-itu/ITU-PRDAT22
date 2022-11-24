compile:
	gcc -Wall -g -o machine ./MicroC/machine.c

build:
	~/bin/fsharp/fslex --unicode CLex.fsl
	~/bin/fsharp/fsyacc --module CPar CPar.fsy

run: build
	dotnet fsi -r ~/bin/fsharp/FsLexYacc.Runtime.dll Absyn.fs CPar.fs CLex.fs Parse.fs Machine.fs Contcomp.fs ParseAndContcomp.fs 

# run: compile
# 	./machine ./MicroC/ex16.out 1000
