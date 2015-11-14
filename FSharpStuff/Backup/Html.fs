module Html
open FSharp.Data
open System
open FSharp.Data
open FSharp.Data.HtmlNode
open FSharp.Data.HtmlAttribute

let (>>=) m f = Option.bind f m

let optionalList list =  
    match list with 
    | [] -> None
    | _ -> Some list 

//let optionPositiveInt int =
//   match intvwith
//    | int = 0 -> None
//    | _ -> Some int

let bob f (x:int) = 
 if (f x) then Some x
 else None

let fred = bob (fun x -> x > 0)


System.Environment.SetEnvironmentVariable ("k", "v")


let doc = 
    """<html>
           <body>
               <img src="myimg.jpg">
               <table title="table" id="results">
                   <tr><th class="ai">Column 1</th><th>Column 2</th></tr>
                   <tr>
                     <td xmlns="http://www.w3.org/1999/xhtml" class="_index">
					    <a class="uri" href="/ovidws/resultsets/emef/nlp/pain/2">2</a>
				     </td>
                     <td class="ai"><span>int 1</span><span>int 2</span></td><td class="al"><span>name one</span><span>name two</span></td>
                   </tr>
                   <tr>
                     <td xmlns="http://www.w3.org/1999/xhtml" class="_index">
					    <a class="uri" href="/ovidws/resultsets/emef/nlp/pain/3">3</a>
				     </td>
                     <td class="ai"><span>int 1</span><span>int 2</span></td><td class="al"><span>name one</span><span>name two</span></td>
                   </tr>
               </table>
           </body>
       </html>""" 
       |> HtmlDocument.Parse
       |> HtmlDocument.elements
       |> Seq.head




type HtmlHelper () =
  
  static let getHtmlNode node n c = node |> HtmlNode.hasClass c && node |> HtmlNode.hasName n 

  static member ById n id =
    n |> HtmlNode.descendants false (HtmlNode.hasId id) |> Seq.tryPick Some 

  static member TRs n =
    n |> HtmlNode.descendants false (HtmlNode.hasName "tr") 

  static member ByClass n c =
    n |> HtmlNode.descendants false (HtmlNode.hasClass c) 

  static member TDByClass c n =
    getHtmlNode n "td" c

  static member AByClass c n =
    getHtmlNode n "a" c


type OvidAuthorData  =
   {
     Index : int
     AuthorInitials : string list 
     AuthorSurname : string list 
   }  
   static member from (x:HtmlNode) =

     let resultNode = HtmlHelper.ById x "results"

     if resultNode = None then None else

     let index row = row |> HtmlNode.descendants false (HtmlHelper.AByClass "uri") 
                         |> Seq.tryPick Some

     let spannie p = resultNode.Value
                            |> HtmlNode.descendants false p
                            |> Seq.collect(HtmlNode.descendants false (HtmlNode.hasName "span"))
                            |> Seq.map(HtmlNode.innerText)
                            |> Seq.toList 
     
     let authorInitials = HtmlHelper.TDByClass "ai" |> spannie
     let authorSurname = HtmlHelper.TDByClass "al" |> spannie
     
     let combinded = List.zip authorInitials authorSurname



     let build n index =
       Some { Index=index |> int; AuthorInitials = HtmlHelper.TDByClass "ai" |> spannie; AuthorSurname = HtmlHelper.TDByClass "al" |> spannie}

     
     let doRow n = n |> index |> Option.map HtmlNode.innerText >>= build n


     resultNode.Value |> HtmlHelper.TRs 
                      |> Seq.map(doRow)
                      |> Seq.toList 
                      |> List.choose id
                      |> Some
         
         
                          
let results = doc |> OvidAuthorData.from

let answer = results.Value

let x = answer