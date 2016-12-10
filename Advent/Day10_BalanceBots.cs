using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent
{
    public class Day10_BalanceBots
    {
        public static BalanceBots ProcessInstructions(IEnumerable<string> instructions, int[] chipsToCheck = null)
        {
            BalanceBots bots = new BalanceBots(chipsToCheck);
            foreach (string instruction in instructions)
            {
                bots.LoadInstructions(instruction);
            }
            bots.ProcessInstructions();
            return bots;
        }

        public class BalanceBots
        {
            private Regex _ValuePattern = new Regex(@"value (?<value>\d+) goes to bot (?<bot>\d+)");
            private Regex _TransferPattern = new Regex(@"bot (?<bot>\d+) gives low to (?<lowtype>output|bot) (?<lowid>\d+) and high to (?<hightype>output|bot) (?<highid>\d+)");

            private Dictionary<string, Bot> _Bots = new Dictionary<string, Bot>();
            private Dictionary<string, Bot> _Output = new Dictionary<string, Bot>();
            private List<KeyValuePair<string, int>> _ValueSetup = new List<KeyValuePair<string, int>>();

            public int[] ImportantChipComparison { get; private set; }
            public List<Bot> BotsWhoDidImportantComparison { get; private set; }

            public BalanceBots(int[] chipsToCheck)
            {
                if (chipsToCheck != null)
                {
                    ImportantChipComparison = chipsToCheck.OrderBy(i => i).ToArray();
                }
                BotsWhoDidImportantComparison = new List<Bot>();
            }

            public Bot GetBin(string id)
            {
                return _Output[id];
            }

            public void LoadInstructions(string instruction)
            {
                Match match = _ValuePattern.Match(instruction);
                if (match.Success)
                {
                    int value = int.Parse(match.Groups["value"].Value);
                    string id = match.Groups["bot"].Value;
                    _ValueSetup.Add(new KeyValuePair<string, int>(id, value));
                }
                else
                {
                    match = _TransferPattern.Match(instruction);
                    if (match.Success)
                    {
                        string botId = match.Groups["bot"].Value;
                        BotType lowType = ParseType(match.Groups["lowtype"].Value);
                        string lowId = match.Groups["lowid"].Value;
                        BotType highType = ParseType(match.Groups["hightype"].Value);
                        string highId = match.Groups["highid"].Value;

                        if (lowType == BotType.None) throw new Exception("Invalid low type");
                        if (highType == BotType.None) throw new Exception("Invalid high type");

                        Bot bot = GetBot(BotType.Bot, botId);
                        bot.LowerBot = GetBot(lowType, lowId);
                        bot.HigherBot = GetBot(highType, highId);
                    }
                }
            }

            public void ProcessInstructions()
            {
                foreach (var pair in _ValueSetup)
                {
                    _Bots[pair.Key].AddChipValue(pair.Value);
                }
            }

            private Bot GetBot(BotType type, string id)
            {
                Bot result = null;
                switch (type)
                {
                    case BotType.Bot:
                        if (_Bots.ContainsKey(id))
                        {
                            result = _Bots[id];
                        }
                        else
                        {
                            result = new Bot(this, type, id);
                            _Bots[id] = result;
                        }
                        break;

                    case BotType.Output:
                        if (_Output.ContainsKey(id))
                        {
                            result = _Output[id];
                        }
                        else
                        {
                            result = new Bot(this, type, id);
                            _Output[id] = result;
                        }
                        break;
                }
                return result;
            }

            private BotType ParseType(string value)
            {
                switch (value.ToLower())
                {
                    case "bot": return BotType.Bot;
                    case "output": return BotType.Output;
                }
                return BotType.None;
            }
        }

        public enum BotType
        {
            Bot, Output, None
        }

        public class Bot
        {
            private BalanceBots _Parent = null;

            public string BotId { get; private set; }

            public BotType BotType { get; private set; }

            public List<int> ChipValues { get; private set; }

            public Bot LowerBot { get; set; }

            public Bot HigherBot { get; set; }

            public Bot(BalanceBots parent, BotType type, string id)
            {
                _Parent = parent;
                BotType = type;
                BotId = id;
                ChipValues = new List<int>();
            }

            public void AddChipValue(int value)
            {
                ChipValues.Add(value);
                if (ChipValues.Count == 2)
                {
                    ChipValues.Sort();
                    int lowValue = ChipValues[0];
                    int highValue = ChipValues[1];
                    if (_Parent.ImportantChipComparison != null)
                    {
                        if (_Parent.ImportantChipComparison[0] == lowValue &&
                            _Parent.ImportantChipComparison[1] == highValue)
                        {
                            _Parent.BotsWhoDidImportantComparison.Add(this);
                        }
                    }
                    LowerBot.AddChipValue(lowValue);
                    HigherBot.AddChipValue(highValue);
                }
            }
        }
    }
}
