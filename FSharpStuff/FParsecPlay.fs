module FParsecPlay



open System.Text
open FParsec
open System.Text.RegularExpressions

//http://blog.fogcreek.com/fparsec/

type UserState = unit // doesn't have to be unit, of course
type Parser<'t> = Parser<'t, UserState>

let (<!>) (p: Parser<_,_>) label : Parser<_,_> =
  fun stream ->
    printfn "%A: Entering %s" stream.Position label
    let reply = p stream
    printfn "%A: Leaving %s (%A) (%A)" stream.Position label reply.Status reply.Result
    reply


type Term = Term of string
type Field =
  | Title
  | Abstract
  | Author
  | Generic of string

type TermExpression =
  {
    Term : Term
    FieldList : Field list
   }
with
  static member from (x:Term * Field list) =
    let t,l = x
    {Term = t; FieldList = l}


let test p str =
  match run p str with
   | Success(result, _, _)   -> printfn "%A" result
   | Failure(errorMsg, _, _) -> printfn "Failure: %s" errorMsg


let testReturn p str =
  match run p str with
    | Success(result, _, _)   -> Some result
    | Failure(errorMsg, _, _) -> printfn "Failure: %s" errorMsg
                                 None

let str s = pstring s
let ws = spaces

//sepBy (pstring "int" <|> pstring "str") (skipString ",")
let notSpace c = c <> ' '
let word:Parser<string,unit> = many1Satisfy notSpace
//let someString:Parser<string,unit> = manyChars anyChar

let pdot:Parser<char,unit> = pchar '.'
let pbeforeDot:Parser<string,unit> = manyCharsTill anyChar pdot
let pterm = pbeforeDot |>> Term 


let ptitle = stringReturn "ti" Title <!> "title"
let pabstract = stringReturn "ab" Abstract <!> "abstract"
let pauthor = stringReturn "au" Author <!> "author"
let pgenericField = anyString 2 |>> (fun x -> Generic x)

//needs to fail if is string greater than 2 !!!!
//some sort of satisfy predicate base on lenght 2
let pfield = ptitle <|> pabstract <|> pauthor <|> pgenericField <!> "field"

let pfieldList = sepBy1 pfield (pstring ",") <!> "field list"


let ptermAndFields = pterm .>>. pfieldList |>> TermExpression.from

let praw = pterm .>>. pfieldList

let termExpression = test ptermAndFields "monkey.ti,ab,au"

test praw "arse.ti,ab,au"

let fields = test pfieldList "ti,ab,au,xx"

let title = test ptitle "ti"

//let pword:Parser<_> = many1 asciiLower
//let monkey = test pword "abcd,ef"
//http://fsharpforfunandprofit.com/posts/recursive-types-and-folds-1b/


let pfieldList2 = sepBy1 pfield (pstring ",") <!> "field list"
test pfieldList2 "ti,ab"

let floatList = ws >>. sepBy1 pfloat (str ",") .>> ws

//issue is any wierd char, breaks the sep by, it expects ',' and then stops
test floatList "3x,2,6"


let psimpleString =  many1Satisfy isAsciiLetter //>>. ptitle2
let pstringToTitle = psimpleString .>> ptitle
test psimpleString "ti"
test pstringToTitle "ti"



//let twoChar = anyString 2
let anyStringList = sepBy1 psimpleString  (str ",")
let pfieldList3 = anyStringList >>. ptitle
test pfieldList3 "ti,ab,au"

//5.7 Looking ahead and backtracking
//5.7.2 Parser predicates
let onlyTwo = manyMinMaxSatisfyL 2 2 isLetter "should be 2 chars" //|>> Title
let onlyTwoTitle = onlyTwo >>. pfield
test onlyTwoTitle "ti"


//pipe example
let float_ws = pfloat .>> ws
let str_ws s = pstring s .>> ws
let product = pipe2 float_ws (str_ws "*" >>. float_ws) (fun x y -> x * y)
test product "3 * 5"


//pfield 2 needs to limit on 2 chars
//must satisfy predicate ???
let pfield2 = onlyTwo >>. (ptitle <|> pabstract <|> pauthor <|> pgenericField )

//this is what I want, need to fail if element is not 2. abx, gets parsed as abstract 
let anyFieldList = sepBy1 pfield2  (str ",")
test anyFieldList "ti,xx,au,xxx"



//this needs to fail because it has 3 letters
test anyFieldList "ti,ab,xxx"


//this seems to wort, I can parse each item
let aorb = str "a" <|> str "b"
let anyCharList = sepBy1 aorb  (str ",")
let charListBetweenBrackets = str "[" >>. anyCharList .>> str "]"
test charListBetweenBrackets "[a,b,b]"


let aorb2 = (pstring "a" >>. pstring "b")
let tryme = attempt (pstring "a" >>. pstring "b")
test tryme "ac"
test aorb2 "ac"


let ab = str "a" .>>. str "b"
let ac = str "a" .>>. str "c"
run (ab <|> ac) "ac"
run ((attempt ab) <|> ac) "ac"



let bInBrackets = str "[" >>. str "b" .>> str "]"
run ((attempt (str "a" .>>. bInBrackets)) <|> ac) "a[B]"


run ((attempt (str "a" .>>. bInBrackets)) <|> attempt ac) "a[B]"

run (str "a" .>>.? bInBrackets <|> ac) "a[B]";


let numberInBrackets = str "[" >>. pint32 .>> str "]" .>> ws
run (many numberInBrackets >>. str "[c]") "[1] [2] [c]"



let numberInBrackets2 = str "[" >>? pint32 .>> str "]" .>> ws
run (many numberInBrackets2 .>> str "[c]") "[1] [2] [c]"


let sepEndBy1_ p sep =
  pipe2 p (many (sep >>? p)) (fun hd tl -> hd::tl) .>> opt sep

run (sepEndBy1_ pint32 (str ";")) "1;2;3"

run (sepEndBy1_ pint32 (str ";")) "1;2;3;"


//The error occurred at the end of the input stream
//means you have skipped and ran off the end

let p1 = followedBy (satisfy ((<>) '0')) >>. pint32
run p1 "123"


let p2 = notFollowedBy (pstring "0") >>. pint32
run p2 "123"

let resultSatisfies predicate msg (p: Parser<_,_>) : Parser<_,_> =
    let error = messageError msg
    fun stream ->
      let state = stream.State
      let reply = p stream
      if reply.Status <> Ok || predicate reply.Result then reply
      else
          stream.BacktrackTo(state) // backtrack to beginning
          Reply(Error, error)


let positiveInt = pint32 |> resultSatisfies (fun x -> x > 0)
                                            "The integer must be positive."
run positiveInt "3"



let isOnlyTwo  = psimpleString |> resultSatisfies (fun x -> x.Length = 2) "not 2 chars" <!> "is only two"
run isOnlyTwo "ab"

//this is close to what I want, runs both together
let pfieldWithLengthCheck = followedByL isOnlyTwo "this field is not 2 characters long" >>. pfield 
test pfieldWithLengthCheck "ti"

//this runs off the end
let ptitle2 = ptitle .>> isOnlyTwo
test ptitle2 "ti"

let anyFieldList2 = sepBy1 pfieldWithLengthCheck  (str ",")
test anyFieldList2 "ti,ab,au,xx"

//this needs to fail because it has 3 letters
test anyFieldList2 "ti,ab,xx"





