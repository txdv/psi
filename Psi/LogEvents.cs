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
  public abstract class Base
  {
    public Base(DateTime dt)
    {
      DateTime = dt;
    }
    public DateTime DateTime { get; set; }
    public IDictionary<string, string> Options { get; set; }

    public static string FormatOptionsString(IDictionary<string, string> dict) {
      System.Text.StringBuilder sb = new System.Text.StringBuilder();
      foreach (KeyValuePair<string, string> kv in dict) {
        if (kv.Value == null) {
          sb.Insert(0, string.Format(" ({0})", kv.Key));
        } else {
          sb.Insert(0, string.Format(" ({0} \"{1}\")", kv.Key, kv.Value));
        }
      }
      return sb.ToString();
    }

    public string OptionsString {
      get {
        return FormatOptionsString(Options);
      }
    }

    public string DateLog {
      get {
        return string.Format("L {0:00}/{1:00}/{2:0000} - {3:00}:{4:00}:{5:00}: ",
                             DateTime.Month,
                             DateTime.Day,
                             DateTime.Year,
                             DateTime.Hour,
                             DateTime.Minute,
                             DateTime.Second);
      }
    }

    public string Log {
      get {
        return string.Format("{0}{1}{2}", DateLog, ToString(), OptionsString);
      }
    }
  }

  public abstract class PlayerBase : Base
  {
    public PlayerBase(DateTime dt, Player player)
      : base(dt)
    {
      Player = player;
    }

    public Player Player { get; set; }
  }

  public class Player
  {
    public Player(string name, string connid, string authid, string team)
    {
      Name = name;
      ConnectionId = int.Parse(connid);
      AuthId = authid;
      Team = team;
    }
    public string Name         { get; set; }
    public int    ConnectionId { get; set; }
    public string AuthId       { get; set; }
    public string Team         { get; set; }

    public override string ToString ()
    {
      return string.Format ("{0}<{1}><{2}><{3}>", Name, ConnectionId, AuthId, Team);
    }
  }

  public class PlayerEnteredGame : PlayerBase
  {
    public PlayerEnteredGame(DateTime dt, Player player)
      : base(dt, player)
    {
    }

    public override string ToString ()
    {
      return string.Format("\"{0}\" entered the game", Player);
    }
  }

  public class Attack : PlayerBase
  {
    public Attack(DateTime dt, Player attacker, Player victim, string weapon)
      : base(dt, attacker)
    {
      Victim = victim;
      Weapon = weapon;
    }

    public Player Victim { get; set; }
    public string Weapon { get; set; }

    public override string ToString ()
    {
      return string.Format("\"{0}\" attacked \"{1}\" with \"{2}\"", Player, Victim, Weapon);
    }
  }

  public abstract class Talk : PlayerBase
  {
    public Talk(DateTime dt, Player player, string message)
      : base(dt, player)
    {
      Message = message;
    }

    public string Message { get; set; }
  }

  public class Say : Talk
  {
    public Say(DateTime dt, Player player, string message)
      : base(dt, player, message)
    {
    }

    public override string ToString()
    {
      return string.Format("\"{0}\" say \"{1}\"", Player, Message);
    }
  }

  public class SayTeam : Talk
  {
    public SayTeam(DateTime dt, Player player, string message)
      : base(dt, player, message)
    {
    }

    public override string ToString()
    {
      return string.Format("\"{0}\" say_team \"{1}\"", Player, Message);
    }
  }

  public class UserValidated : PlayerBase
  {
    public UserValidated(DateTime dt, Player player)
      : base(dt, player)
    {
    }

    public override string ToString()
    {
      return string.Format("\"{0}\" STEAM USERID validated", Player);
    }
  }

  public class PlayerTrigger : PlayerBase
  {
    public PlayerTrigger(DateTime dt, Player player, string trigger)
      : base(dt, player)
    {
      Trigger = trigger;
    }

    public string Trigger { get; set; }

    public override string ToString()
    {
      return string.Format("\"{0}\" triggered \"{1}\"", Player, Trigger);
    }
  }

  /// <summary>
  /// amxmodx introduced this ugly construct
  /// </summary>
  public class PlayerTriggerAgainst : PlayerTrigger
  {
    public PlayerTriggerAgainst(DateTime dt, Player player, string trigger, Player target)
      : base(dt, player, trigger)
    {
      Target = target;
    }

    public Player Target { get; set; }

    public override string ToString ()
    {
      return string.Format ("{0} against \"{1}\"", base.ToString(), Target);
    }
  }

  public class JoinTeam : PlayerBase
  {
    public JoinTeam(DateTime dt, Player player, string team)
      : base(dt, player)
    {
      Team = team;
    }

    public string Team { get; set; }

    public override string ToString ()
    {
      return string.Format ("\"{0}\" joined team \"{1}\"", Player, Team);
    }
  }

  public class Disconnected : PlayerBase
  {
    public Disconnected(DateTime dt, Player player)
      : base(dt, player)
    {
    }

    public override string ToString()
    {
      return string.Format("\"{0}\" disconnected", Player);
    }
  }

  public class NameChanged : PlayerBase
  {
    public NameChanged(DateTime dt, Player player, string name)
      : base(dt, player)
    {
      Name = name;
    }

    public string Name { get; set; }

    public override string ToString()
    {
      return string.Format("\"{0}\" changed name to \"{1}\"", Player, Name);
    }
  }

  public class Connected : PlayerBase
  {
    public Connected(DateTime dt, Player player, string ipstring)
      : base(dt, player)
    {
      IPString = ipstring;
    }

    public string IPString { get; set; }

    public override string ToString()
    {
      return string.Format("\"{0}\" connected, address \"{1}\"", Player, IPString);
    }
  }

  public class Suicide : PlayerBase
  {
    public Suicide(DateTime dt, Player player, string trigger)
      : base(dt, player)
    {
      Trigger = trigger;
    }

    public string Trigger { get; set; }

    public override string ToString()
    {
      return string.Format("\"{0}\" committed suicide with \"{1}\"", Player, Trigger);
    }

  }

  public class StartedMap : Base
  {
    public StartedMap(DateTime dt, string map)
      : base(dt)
    {
      Map = map;
    }

    public string Map { get; set; }

    public override string ToString ()
    {
      return string.Format("Started map \"{0}\"", Map);
    }
  }

  public class ServerCvarsStart : Base
  {
    public ServerCvarsStart(DateTime dt)
      : base(dt)
    {
    }

    public override string ToString()
    {
      return string.Format("Server cvars start");
    }
  }

  public class ServerCvarsEnd : Base
  {
    public ServerCvarsEnd(DateTime dt)
      : base(dt)
    {
    }

    public override string ToString ()
    {
      return "Server cvars end";
    }
  }

  public class ServerCvarSet : Base
  {
    public ServerCvarSet(DateTime dt, string cvar, string val)
      : base(dt)
    {
      CVar = cvar;
      Value = val;

    }

    public string CVar { get; set; }
    public string Value { get; set; }

    public override string ToString()
    {
      return string.Format("Server cvar \"{0}\" = \"{1}\"", CVar, Value);
    }
  }

  public class TeamTrigger : Base
  {
    public TeamTrigger(DateTime dt, string team, string trigger)
      : base(dt)
    {
      Team = team;
      Trigger = trigger;
    }

    public string Team { get; set; }
    public string Trigger { get; set; }

    public override string ToString()
    {
      return string.Format ("Team \"{0}\" triggered \"{1}\"", Team, Trigger);
    }
  }

  public class WorldTrigger : Base
  {
    public WorldTrigger(DateTime dt, string trigger)
      : base(dt)
    {
      Trigger = trigger;
    }

    public string Trigger { get; set; }

    public override string ToString ()
    {
      return string.Format ("World triggered \"{0}\"", Trigger);
    }
  }

  public abstract class LogFile : Base
  {
    public LogFile(DateTime dt)
      : base(dt)
    {
    }
  }

  public class LogFileStarted : LogFile
  {
    public LogFileStarted(DateTime dt)
      : base(dt)
    {
    }

    public override string ToString ()
    {
      return "Log file started";
    }
  }

  public class LogFileClosed : LogFile
  {
    public LogFileClosed(DateTime dt)
      : base(dt)
    {
    }

    public override string ToString ()
    {
      return "Log file closed";
    }
  }
}

