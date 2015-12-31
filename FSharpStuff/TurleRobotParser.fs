module TurleRobotParser

open FParsec

type arg = int
type command =
  | Forward of arg
  | Turn of arg
  | Right of arg
  | Left of arg
  | Repeat of arg * command list

let forward:Parser<float,unit> = pstring "forward" >>. spaces1 >>. pfloat

//let pforward = forward |>> fun n -> Forward(int n)

let pforward:Parser<command,unit> = (pstring "fd" <|> pstring "forward")
                                      >>. spaces1
                                      >>. pfloat
                                      |>> fun n -> Forward(int n)

let pleft:Parser<command,unit> = (pstring "left" <|> pstring "lt") >>. spaces1 >>. pfloat
                                   |>> fun x -> Left(int -x)

let pright:Parser<command,unit> = (pstring "right" <|> pstring "right") >>. spaces1 >>. pfloat
                                   |>> fun x -> Right(int x)


let pcommand = pforward <|> pleft <|> pright


let pcommands = many (pcommand .>> spaces)

let block = between (pstring "[") (pstring "]") pcommands


let prepeat = pstring "repeat" >>. spaces1 >>. pfloat .>> spaces .>>. block
                 |>> fun (n, commands) -> Repeat(int n, commands)

let q1 =  "repeat 36 [forward 10 right 10]"


let parse code =
  match run prepeat code with
   | Success(result,_,_) -> result
   | Failure(msg,_,_) -> failwith msg


let answer = q1 |> parse

