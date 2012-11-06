
all: RawParser.cs

RawParser.cs: RawParser.rl
	ragel -A RawParser.rl -o RawParser.cs

.PHONY: clean

clean:
	rm -f NewParser.cs
