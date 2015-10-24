module DataStructures


type Book =
         { Name : string;
            AuthorName : string;
            Rating : int option;
            ISBN : string }

let shitBook =
        { Name = "some name";
            AuthorName = "joe";
            Rating = Some 2;
            ISBN = "eee";
            }

let printRating book =
    match book.Rating with
    | Some rating ->
        printfn "I gave this %d stars" rating
    | None ->
        printfn "not read book"


type MushroomColor =
    | Red
    | Green
    | Purple

type PowerUp = 
| FireFlower
| Mushroom of MushroomColor
| Start

let powerup = Mushroom Red

let printPowerup p =
    match p with
    | FireFlower ->
        printfn "this is fire flower"
    | Mushroom color -> match color with
        | Red ->
            printfn "this is a red mushroom"
        | Green ->
            printfn "this is a green mushroom"
        | Purple ->
            printfn "this is a purple mushroom"
    | Start ->
        printfn "this is start"

