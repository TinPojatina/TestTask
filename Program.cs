using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.WriteLine("Upisite A za example tekst ili B kako bi ste unijeli svoj tekst");
        string pick = Console.ReadLine()?.ToUpper().Trim();

        if (pick == "A") DefaultText();
        else if (pick == "B") InputText();
        else
        {
            Console.WriteLine("Neispravan unos. Unesite A ili B.");
            return;
        }
    }

    static void DefaultText()
    {
        string text = "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?";
        AlterText(text);
    }

    static void InputText()
    {
        Console.WriteLine("Unesite svoj tekst:");
        string text = Console.ReadLine();
        AlterText(text);
    }

    static void AlterText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            Console.WriteLine("Tekst ne moze biti null.");
            InputText();
        }

        Console.WriteLine($"\n\nOriginal: \n{text}\n\n\n");
        Console.WriteLine($"Scrambled Words: \n{ScrambleWords(text)}\n\n\n");
        Console.WriteLine($"Reversed Words: \n{ReverseWords(text)}\n\n\n");
        Console.WriteLine($"Reversed Text: \n{ReverseText(text)} \n\n\n");
        Console.WriteLine($"Reversed Sentences: \n{ReverseSentences(text)} \n\n\n");
        VowelStatistics(text);
    }

    static string ScrambleWords(string sentence)
    {
        var rand = new Random();
        string[] words = sentence.Split(' ');
        string result = "";

        foreach (var word in words)
        {
            if (word.Length > 3)
            {
                var middle = word.Substring(1, word.Length - 2).ToCharArray();
                result += word[0] + new string(middle.OrderBy(c => rand.Next()).ToArray()) + word[^1] + " ";
            }
            else result += word + " ";
        }

        return CapitalizeFirstLetterEachSentence(result.TrimEnd());
    }

    static string ReverseWords(string text)
    {
        text = text.ToLower();
        string[] sentences = Regex.Split(text, @"(?<=[\.!\?])\s+");
        string result = "";

        foreach (var sentence in sentences)
        {
            string[] words = Regex.Split(sentence, @"\W+").Where(w => !string.IsNullOrEmpty(w)).ToArray();
            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];
                string reversedWord = new string(word.Reverse().ToArray());
                result += reversedWord + " ";
            }
            Match match = Regex.Match(sentence, @"[\.!\?]$");
            if (match.Success)
            {
                result = result.TrimEnd();
                result += match.Value + " ";
            }
        }
        return CapitalizeFirstLetterEachSentence(result.TrimEnd());
    }


    static string ReverseText(string text)
    {
        text = text.ToLower();
        var sentences = Regex.Split(text, @"(?<=[\.!\?])\s+");
        Array.Reverse(sentences);
        string result = "";

        foreach (var sentence in sentences)
        {
            var words = sentence.Split(' ');
            Array.Reverse(words);
            result += string.Join(" ", words) + ". ";
        }
        return CapitalizeFirstLetterEachSentence(result.TrimEnd());
    }

    static string ReverseSentences(string text)
    {
        var sentences = Regex.Split(text, @"(?<=[\.!\?])\s+");
        Array.Reverse(sentences);
        return string.Join(" ", sentences);
    }

    static void VowelStatistics(string text)
    {
        const string vowels = "aeiou";
        var vowelCount = new Dictionary<char, int>();
        int totalChars = 0, totalVowels = 0;

        foreach (var vowel in vowels) vowelCount[vowel] = 0;
        foreach (var c in text.Where(char.IsLetter))
        {
            totalChars++;
            char lowerC = char.ToLower(c);
            if (vowels.Contains(lowerC))
            {
                totalVowels++;
                vowelCount[lowerC]++;
            }
        }

        Console.WriteLine("\n\n\nSamoglasnici:\n");
        foreach (var kvp in vowelCount)
        {
            if (kvp.Value > 0)
            {
                double percentage = (double)kvp.Value / totalChars * 100;
                Console.WriteLine($"{char.ToUpper(kvp.Key)}/{kvp.Key}: {kvp.Value} pojavljanja, {percentage:F2}%");
            }
        }
        double totalPercentage = (double)totalVowels / totalChars * 100;
        Console.WriteLine($"Ukupno samoglasnika: {totalVowels}, {totalPercentage:F2}%");
    }

    static string CapitalizeFirstLetterEachSentence(string text)
    {
        var sentences = Regex.Split(text, @"(?<=[\.!\?])\s+");
        var result = new List<string>();
        foreach (var sentence in sentences)
        {
            if (string.IsNullOrWhiteSpace(sentence))
                continue;
            result.Add(char.ToUpper(sentence[0]) + sentence.Substring(1));
        }
        return string.Join(" ", result);
    }

}
