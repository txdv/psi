
all: lexer.exe

lexer.cs: lexer.rl
	ragel -A lexer.rl -o lexer.cs

lexer.exe: lexer.cs main.cs
	gmcs -sdk:4.5 lexer.cs main.cs -out:lexer.exe

.PHONY: run clean

run: lexer.exe
	mono lexer.exe /home/bentkus/Projects/goldsrc/logs 104857600

clean:
	rm -f lexer.cs *.exe *.mdb
