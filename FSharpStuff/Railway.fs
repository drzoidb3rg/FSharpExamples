module Railway
//http://fsharpforfunandprofit.com/posts/recipe-part2/

type Result<'TSuccess,'TFailure> = 
    | Success of 'TSuccess
    | Failure of 'TFailure

type Request = {name:string; email:string}

let bind switchFunction twoTrackInput = 
    match twoTrackInput with
    | Success s -> switchFunction s
    | Failure f -> Failure f

 
let (>>=) twoTrackInput switchFunction = 
    bind switchFunction twoTrackInput 

let validate1 input =
   if input.name = "" then Failure "Name must not be blank"
   else Success input

let validate2 input =
   if input.name.Length > 50 then Failure "Name must not be longer than 50 chars"
   else Success input

let validate3 input =
   if input.email = "" then Failure "Email must not be blank"
   else Success input

let combinedValidation x = 
    x 
    |> validate1   // normal pipe because validate1 has a one-track input
                   // but validate1 results in a two track output...
    >>= validate2  // ... so use "bind pipe". Again the result is a two track output
    >>= validate3   // ... so use "bind pipe" again. 


let input1 = {name="xxx"; email=""}
combinedValidation input1 
|> printfn "Result1=%A"


//uderstan a 2 track function vs switch function

//using switch composition

let (>=>) switch1 switch2 x = 
    match switch1 x with
    | Success s -> switch2 s
    | Failure f -> Failure f 

let combinedValidationWithSwitchComposition = 
    validate1 
    >=> validate2 
    >=> validate3 



let canonicalizeEmail input =
   { input with email = input.email.Trim().ToLower() }


// convert a normal function into a switch
let switch f x = 
    f x |> Success


let usecase = 
    validate1 
    >=> validate2 
    >=> validate3 
    >=> switch canonicalizeEmail


// convert a normal function into a two-track function
let map oneTrackFunction twoTrackInput = 
    match twoTrackInput with
    | Success s -> Success (oneTrackFunction s)
    | Failure f -> Failure f


let mappedFunction input = map canonicalizeEmail input

let usecase2 = 
    validate1 
    >=> validate2 
    >=> validate3 
    >> map canonicalizeEmail

let tee f x = 
    f x |> ignore
    x

// a dead-end function    
let updateDatabase input =
   ()   // dummy dead-end function for now

let usecase3 = 
    validate1 
    >=> validate2 
    >=> validate3 
    >=> switch canonicalizeEmail
    >=> switch (tee updateDatabase)

let usecase4 = 
    validate1 
    >> bind validate2 
    >> bind validate3 
    >> map canonicalizeEmail   
    >> map (tee updateDatabase)

let tryCatch f x =
    try
        f x |> Success
    with
    | ex -> Failure ex.Message

let usecase5 = 
    validate1 
    >=> validate2 
    >=> validate3 
    >=> switch canonicalizeEmail
    >=> tryCatch (tee updateDatabase)


//********* Functions with two-track input ****************

let doubleMap successFunc failureFunc twoTrackInput =
    match twoTrackInput with
    | Success s -> Success (successFunc s)
    | Failure f -> Failure (failureFunc f)


let log twoTrackInput = 
    let success x = printfn "DEBUG. Success so far: %A" x; x
    let failure x = printfn "ERROR. %A" x; x
    doubleMap success failure twoTrackInput 

let usecase6 = 
    validate1 
    >=> validate2 
    >=> validate3 
    >=> switch canonicalizeEmail
    >=> tryCatch (tee updateDatabase)
    >> log

let succeed x = 
    Success x

let fail x = 
    Failure x


//*******************Combining functions in parallel***************************
//let plus switch1 switch2 x = 
//    match (switch1 x),(switch2 x) with
//    | Success s1,Success s2 -> Success (s1 + s2)
//    | Failure f1,Success _  -> Failure f1
//    | Success _ ,Failure f2 -> Failure f2
//    | Failure f1,Failure f2 -> Failure (f1 + f2)

let plus addSuccess addFailure switch1 switch2 x = 
    match (switch1 x),(switch2 x) with
    | Success s1,Success s2 -> Success (addSuccess s1 s2)
    | Failure f1,Success _  -> Failure f1
    | Success _ ,Failure f2 -> Failure f2
    | Failure f1,Failure f2 -> Failure (addFailure f1 f2)

// create a "plus" function for validation functions
let (&&&) v1 v2 = 
    let addSuccess r1 r2 = r1 // return first
    let addFailure s1 s2 = s1 + "; " + s2  // concat
    plus addSuccess addFailure v1 v2 


//add all the 3 validatio funcs together, they all pass or fail as one, but we get
//error messages concated together
let combinedValidation2 = 
    validate1 
    &&& validate2 
    &&& validate3 


//now we can use this combined validator in our workflow
let usecase7 = 
    combinedValidation2
    >=> switch canonicalizeEmail
    >=> tryCatch (tee updateDatabase)

//*****************Dynamic injection of functions******************

type Config = {debug:bool}


let debugLogger twoTrackInput = 
    let success x = printfn "DEBUG. Success so far: %A" x; x
    let failure = id // don't log here
    doubleMap success failure twoTrackInput 

let injectableLogger config = 
    if config.debug then debugLogger else id


let usecase8 config = 
    combinedValidation 
    >> map canonicalizeEmail
    >> injectableLogger config

