﻿namespace SentimentFS.TextUtilities.Tests

module Program =
    
    open Expecto
    
    [<EntryPoint>]
    let main argv =
        Tests.runTestsInAssembly defaultConfig argv
