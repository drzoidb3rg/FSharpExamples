module Maybe


//type MaybeMonad() =
//  member t.Bind(m,f) =
//    match m with
//    | Some v -> f v
//    | None -> None
//  member t.Return v = Some v
type MaybeMonad() =
  member t.Bind(m,f) = Option.bind f m
  member t.Return v = Some v

let maybe = MaybeMonad()

let (>>=) m f = Option.bind f m

let log a b = System.Console.WriteLine(sprintf "-Log: %A (%A)" a b)

let foo x    = try 2*x |> Some with ex -> log x ex; None
let bar x    = try 2+x |> Some with ex -> log x ex; None
let foobar x = try 2/x |> Some with ex -> log x ex; None

2 |> Some >>= foo >>= bar >>= foobar
