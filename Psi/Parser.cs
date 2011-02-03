/*
  This file is part of Psi.

  Copyright (C) 2011 Andrius Bentkus

  Psi is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  Foobar is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with Psi.  If not, see <http://www.gnu.org/licenses/>.
*/


using System;
using System.Collections.Generic;

namespace Psi
{
  public static class Parser
  {
    private static string str;
    private static int strLen;
    private static int pos;
    private static DateTime dateTime;

    private static bool CheckPrefix()
    {
      return str[0]  == 'L'
          && str[1]  == ' '
          && str[4]  == '/'
          && str[7]  == '/'
          && str[12] == ' '
          && str[13] == '-'
          && str[14] == ' '
          && str[17] == ':'
          && str[20] == ':'
          && str[23] == ':'
          && str[24] == ' ';
    }

    private static bool CheckPrefixNumbers()
    {
      // year
      return str[8]  >= '0' && str[8] <= '9'
          && str[9]  >= '0' && str[9] <= '9'
          && str[10] >= '0' && str[10] <= '9'
          && str[11] >= '0' && str[11] <= '9';
      // TODO: month and day

    }

    private static void GetDate()
    {
      dateTime =
             new DateTime(int.Parse(str.Substring(8, 4)),
                          int.Parse(str.Substring(2, 2)),
                          int.Parse(str.Substring(5, 2)),
                          int.Parse(str.Substring(15, 2)),
                          int.Parse(str.Substring(18, 2)),
                          int.Parse(str.Substring(21, 2)));
    }

    private static string nick;
    private static string connid;
    private static string authid;
    private static string team;
    private static void ReadPlayer()
    {
      if (str[pos] == '"')
        pos++;

      int nickPos = pos;
      int connPos = -1;
      int authPos = -1;
      int teamPos = -1;

      char c;
      int count = 0;
      while ((c = str[pos]) != '"') {
        if (c == '<') {
          switch (count) {
          case 0:
            connPos = pos;
            break;
          case 1:
            authPos = pos;
            break;
          case 2:
            teamPos = pos;
            break;
          default:
            connPos = authPos;
            authPos = teamPos;
            teamPos = pos;
            break;
          }
          count++;
        }
        pos++;
      }

      bool invalid = (nickPos == -1) || (connPos == -1) || (authPos == -1) || (teamPos == -1);
      if (invalid) {
        throw new Exception();
      }

      nick   = str.Substring(nickPos    , connPos - nickPos);
      connid = str.Substring(connPos + 1, authPos - connPos - 2);
      authid = str.Substring(authPos + 1, teamPos - authPos - 2);
      team   = str.Substring(teamPos + 1, pos     - teamPos - 2);

      pos++;
    }

    private static bool Require(string val)
    {
      int start = pos;
      int len = val.Length;
      for (int i = 0; i < len; i++) {
        if (pos >= strLen) {
          pos = start;
          return false;
        }
        if (val[i] != str[pos]) {
          pos = start;
          return false;
        }
        pos++;
      }
      return true;
    }

    private static Player GetPlayer()
    {
      return new Player(nick, connid, authid, team);
    }

    private static string ReadString()
    {
      if (str[pos] == '"') {
        pos++;
      }

      int start = pos;
      while (str[pos] != '"') {
        pos++;
      }

      pos++;

      return str.Substring(start, pos - start - 1);
    }

    private static int ReadUntil(string token)
    {
      while (token[0] != str[pos]) {
        if (pos >= strLen)
          return -1;
        pos++;
      }

      bool match = false;
      while (!match) {
        match = true;
        for (int i = 0; i < token.Length; i++) {
          if (str[pos] != token[i]) {
            match = false;
            pos++;
            break;
          }
          pos++;
          if (pos > strLen) {
            return -1;
          }
        }
      }

      return pos - token.Length;
    }

    private static int endpos = -1;
    private static IDictionary<string, string> ReadOptionsBackwards()
    {
      Dictionary<string, string> dict = new Dictionary<string, string>();

      endpos = strLen;
      endpos--;

      while (true) {

        if (str[endpos] != ')')
          break;

        endpos--;


        string val = null;
        if (str[endpos] == '\"') {
          int endValue = endpos;
          endpos--; // "text ") wont get matched
          while (!(str[endpos-1] == ' ' && str[endpos] == '\"')) {
            endpos--;
          }
          val = str.Substring(endpos + 1, endValue - endpos - 1);
          endpos--;
        } else {
          endpos++; // we shouldn't point right after the key
        }

        int endKey = endpos;

        while (str[endpos] != '(') {
          endpos--;
        }
        string key = str.Substring(endpos + 1, endKey - endpos - 1);
        dict[key] = val;
        endpos--;


        if (str[endpos] != ' ') {
          throw new Exception();
        }
        endpos--;
      }
      return dict;
    }

    private static string ReadValue()
    {
      int length = endpos - ++pos;
      string ret = str.Substring(pos, length);
      pos += length + 1;
      return ret;
    }
    public static Base UnsafeParse(string s)
    {
      str = s;
      strLen = s.Length;
      if (strLen < 25)
        return null;

      pos = 25;

      CheckPrefix();
      CheckPrefixNumbers();
      GetDate();

      Base log = null;

      IDictionary<string, string> dict = ReadOptionsBackwards();

      switch (str[pos]) {
      case '"':
        pos++;
        ReadPlayer();
        pos++;

        if        (Require("attacked ")) {
          Player player = GetPlayer();
          ReadPlayer();
          if (Require(" with ")) {
            log = new Attack(dateTime, player, GetPlayer(), ReadValue());
          }
        } else if (Require("say ")) {
          log = new Say(dateTime, GetPlayer(), ReadValue());
        } else if (Require("say_team ")) {
          log = new SayTeam(dateTime, GetPlayer(), ReadValue());
        } else if (Require("STEAM USERID validated")) {
          log = new UserValidated(dateTime, GetPlayer());
        } else if (Require("triggered ")) {

          Player player = GetPlayer();
          string trigger = ReadString();

          if (Require(" against ")) {
            ReadPlayer();
            Player target = GetPlayer();
            log = new PlayerTriggerAgainst(dateTime, player, trigger, target);
          } else  {
            log = new PlayerTrigger(dateTime, player, trigger);
          }

        } else if (Require("killed ")) {
        } else if (Require("joined team ")) {
          log = new JoinTeam(dateTime, GetPlayer(), ReadValue());
        } else if (Require("entered the game")) {
          log = new PlayerEnteredGame(dateTime, GetPlayer());
        } else if (Require("disconnected")) {
          log = new Disconnected(dateTime, GetPlayer());
        } else if (Require("changed name to ")) {
          log = new NameChanged(dateTime, GetPlayer(), ReadValue());
        } else if (Require("connected, address ")) {
          log = new Connected(dateTime, GetPlayer(), ReadValue());
        } else if (Require("committed suicide with ")) {
          log = new Suicide(dateTime, GetPlayer(), ReadValue());
        }
        break;
      case 'S':
        if        (Require("Server cvars start")) {
          log = new ServerCvarsStart(dateTime);
        } else if (Require("Server cvars end")) {
          log = new ServerCvarsEnd(dateTime);
        } else if (Require("Server cvar ")) {
          string cvar = ReadString();
          if (Require(" = ")) {
            log = new ServerCvarSet(dateTime, cvar, ReadValue());
          }
        } else if (Require("Started map ")) {
          log = new StartedMap(dateTime, ReadValue());
        }
        break;
      case 'T':
        if (Require("Team ")) { // triggered
          string team = ReadString();
          if (Require(" triggered ")) {
            log = new TeamTrigger(dateTime, team, ReadValue());
          }
        }
        break;
      case 'W':
        if (Require("World triggered ")) {
          log = new WorldTrigger(dateTime, ReadValue());
        }
        break;
      case 'L':
        if (Require("Log file closed")) {
          log = new LogFileClosed(dateTime);
        } else if (Require("Log file started")) {
          log = new LogFileStarted(dateTime);
        }
        break;
      case '[':
        break;
      case 'K':
        if (Require("Kick: ")) {
          // TODO: implement this?
          // Problem: double \" in argument
        }
        break;
      case 'B':
        if (Require("Bad Rcon: ")) {
          // TODO: implement this
          // Problem: imba amount of \"
        }
        break;
      default:
        break;
      }

      // No need for option checking
      if (log == null)
        return null;

      log.Options = dict;

      if (--pos != endpos) {
        throw new Exception();
      }

      return log;
    }

    public static Base Parse(string s)
    {
      try {
        return UnsafeParse(s);
      } catch {
        return null;
      }
    }
  }
}

