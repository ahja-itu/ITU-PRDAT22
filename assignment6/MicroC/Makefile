.PHONY: build
build:
	~/bin/fsharp/fslex --unicode CLex.fsl
	~/bin/fsharp/fsyacc --module CPar CPar.fsy

.PHONY: run
run: build
	fsharpi -r FsLexYacc.Runtime.dll Absyn.fs CPar.fs CLex.fs Parse.fs Interp.fs ParseAndRun.fs
