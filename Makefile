
all: lexer.exe

lexer.cs: lexer.rl
	ragel -A lexer.rl -o lexer.cs

lexer.exe: lexer.cs main.cs
	gmcs -debug lexer.cs main.cs -out:lexer.exe

.PHONY: run

run: lexer.exe
	mono --debug lexer.exe /home/bentkus/Projects/goldsrc/logs 10000000
