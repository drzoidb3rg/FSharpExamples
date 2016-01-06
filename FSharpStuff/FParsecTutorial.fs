module FParsecTutorial 
open FParsec



let test p str =
    match run p str with
    | Success(result, _, _)   -> printfn "Success: %A" result
    | Failure(errorMsg, _, _) -> printfn "Failure: %s" errorMsg

//test pfloat "1.25"

let str s = pstring s
let floatBetweenBrackets = str "[" >>. pfloat .>> str "]"

//test floatBetweenBrackets "[1.0]"

//let betweenStrings s1 s2 p = str s1 >>. p .>> str s2

let between pBegin pEnd p  = pBegin >>. p .>> pEnd
let betweenStrings s1 s2 p = p |> between (str s1) (str s2)

test (many floatBetweenBrackets) ""

test (many floatBetweenBrackets) "[2][3][4]"

//many1 : at least one element

test (many1 (floatBetweenBrackets <?> "float between brackets")) "(1)"


let floatList = str "[" >>. sepBy pfloat (str ",") .>> str "]"

test floatList "[1,2,3]"

let ws = spaces

let str_ws s = pstring s .>> ws

let float_ws = pfloat .>> ws

let numberList = str_ws "[" >>. sepBy float_ws (str_ws ",") .>> str_ws "]"

test numberList @"[ 1 ,
                    2 ] "


let numberListFile = ws >>. numberList //.>> eof

test numberListFile " [5, 6, 7] [8]"

let identifier =
  let isIdentifierFirstChar c = isLetter c || c = '_'
  let isIdentifierChar c = isLetter c || isDigit c || c = '_'
  
  many1Satisfy2L isIdentifierFirstChar isIdentifierChar "identifier monkey"
    .>> ws 

test identifier "test1="

let unescape c = match c with
                     | 'n' -> '\n'
                     | 'r' -> '\r'
                     | 't' -> '\t'
                     | c   -> c

let stringLiteral =
    let normalChar = satisfy (fun c -> c <> '\\' && c <> '"')

    let escapedChar = pstring "\\" >>. (anyOf "\\nrt\"" |>> unescape)
    between (pstring "\"") (pstring "\"")
            (manyChars (normalChar <|> escapedChar))

test stringLiteral "\"a \nb c\""

//let stringLiteral2 =
//    let normalCharSnippet = many1Satisfy (fun c -> c <> '\\' && c <> '"')
//    let escapedChar = pstring "\\" >>. (anyOf "\\nrt\"" |>> function
//                                                            | 'n' -> "\n"
//                                                            | 'r' -> "\r"
//                                                            | 't' -> "\t"
//                                                            | c   -> string c)
//    between (pstring "\"") (pstring "\"")
//            (manyStrings (normalCharSnippet <|> escapedChar))


let product = pipe2 float_ws (str_ws "*" >>. float_ws) (fun x y -> x * y)

test product "3 * 5"

type StringConstant = StringConstant of string * string

let stringConstant = pipe3 identifier (str_ws "=") stringLiteral
                           (fun id _ str -> StringConstant(id, str))

test stringConstant "myString = \"stringValue\""

test (float_ws .>>. (str_ws "," >>. float_ws)) "123,456"  

let boolean =  (stringReturn "true"  true)
              <|> (stringReturn "false" false)    
 
test boolean "false"            


type UserState = unit // doesn't have to be unit, of course
type Parser<'t> = Parser<'t, UserState>

let p : Parser<_> = pstring "test"

