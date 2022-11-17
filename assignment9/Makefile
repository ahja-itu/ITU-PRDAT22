docker-build:
	docker build -t prdat .

docker-run:
	docker run --name prdat --rm -it -v "$(shell pwd)":/hygge prdat

docker: docker-build docker-run

compile:
	gcc -Wall -g -o listmachine ./ListC/ListVM/ListVM/listmachine.c

run: compile
	./listmachine ./ListC/ex35.out 1000
