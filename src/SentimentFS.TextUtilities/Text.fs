namespace SentimentFS.TextUtilities

module Text =
    open System
    open System.Text.RegularExpressions

    let chop count (text:string) =
        text.Remove(text.Length-count)

    let skip count (text:string) =
        text.Substring(count, text.Length-count)

    let skipPrefix prefix (text:string) =
        if text.StartsWith prefix then
            skip prefix.Length text
        else
            text

    let removeSuffix suffix (text:string) =
        if text.EndsWith suffix then
            chop suffix.Length text
        else
            text

    let replaceSuffix suffix (replacement:string) (text:string) =
        if text.EndsWith suffix then
            (chop suffix.Length text) + replacement
        else
            text

    let replacePrefix prefix (replacement:string)  (text:string) =
        if text.StartsWith prefix then
            replacement + (skip prefix.Length text)
        else
            text

    let replace (pattern: string)(replacement: string)(word: string) =
        Regex.Replace(word, pattern, replacement)

    let split([<ParamArray>]patterns: char array)(text: string) =
        text.Split patterns

    let toLower(word: string) =
        word.ToLower()

    let endsWith (word: string) ([<ParamArray>] args: string array) =
        args |> Array.exists(fun x -> word.EndsWith(x))

    let startsWith (word: string) ([<ParamArray>] args: string array) =
        args |> Array.exists(fun x -> word.StartsWith(x))

    let tf(word: string)(words: string list) =
        let count = words |> List.filter(fun x -> x = word) |> List.length |> float
        let length = words |> List.length |> float
        count / length

    let idf(word: string) (wordsList: string list list) =
        let numberContaining = wordsList |> List.sumBy(fun x -> if (x |> List.contains(word)) then 1 else 0) |> float
        if(numberContaining <> 0.0) then
            System.Math.Log10((wordsList |> List.length |> float) / (numberContaining))
        else
            System.Math.Log10((wordsList |> List.length |> float) / 1.0)

    let tfidf(word: string)(words: string list)(wordsList: string list list) =
        (tf(word)(words)) * (idf(word)(wordsList))

module Regex =
    open System.Text.RegularExpressions

    let (|Regex|_|) pattern input =
        let m = Regex.Match(input, pattern)
        if m.Success then
            Some(List.tail [ for g in m.Groups -> g.Value ])
        else
            None

    let (|FirstMatch|_|) pattern input =
        let m = Regex.Match(input, pattern)
        if m.Success then
            Some(m.Value)
        else
            None

    let (|SuffixMatch|_|) pattern input =
        let m = Regex.Match(input, pattern + "$")
        if m.Success then
            Some(m.Value)
        else
            None

    let replace (pattern: string)(replacement: string)(word: string) =
        Regex.Replace(word, pattern, replacement)
