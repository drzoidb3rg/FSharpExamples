module Types
  
type Provider = 
  | Proquest
  | Ebsco

type Database = 
  | CINAHL
  | HBE
  | Medline
  | BNI

let resolveDatbase x = 
    match x with
      | "cinahl" -> Some (CINAHL,Proquest)
      | "bni" -> Some (BNI, Ebsco)
      | _ -> None





