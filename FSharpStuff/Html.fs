module Html
open FSharp.Data
open System
open FSharp.Data
open FSharp.Data.HtmlNode
open FSharp.Data.HtmlAttribute

let (>>=) m f = Option.bind f m

let doc = 
    """<html>
           <body>
               <img src="myimg.jpg">
               <table title="table" id="results">
                   <tr><th class="ai">Column 1</th><th>Column 2</th></tr>
                   <tr><td class="ai"><span>int 1</span><span>int 2</span></td><td>yes</td></tr>
                   <tr><td class="ai"><span>int 1</span><span>int 2</span></td><td>yes</td></tr>
               </table>
           </body>
       </html>""" 
       |> HtmlDocument.Parse
       |> HtmlDocument.elements
       |> Seq.head

let optionalList list =  
    match list with 
    | [] -> None
    | _ -> Some list 


type HtmlHelper () =
  
  static let getHtmlNode node n c = node |> HtmlNode.hasClass c && node |> HtmlNode.hasName n 

  static member ById n id =
    n |> HtmlNode.descendants false (HtmlNode.hasId id) |> Seq.tryPick Some 

  static member TDByClass n c =
    getHtmlNode n "td" c



type OvidAuthorData  =
   {
     AuthorInitials : string list option
   }  
   static member from (x:HtmlNode) =
     let resultNode = HtmlHelper.ById x "results"

     if resultNode = None then None else
     
     let initialPred node = HtmlHelper.TDByClass node "ai"

     let initials = resultNode.Value
                            |> HtmlNode.descendants false initialPred 
                            |> Seq.collect(HtmlNode.descendants false (HtmlNode.hasName "span"))
                            |> Seq.map(HtmlNode.innerText)
                            |> Seq.toList 
                            |> optionalList

     Some {AuthorInitials = initials}

                                         
                
let results = doc |> OvidAuthorData.from

let answer = results

let x = answer