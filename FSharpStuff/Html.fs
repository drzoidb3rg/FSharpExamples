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
                   <tr>
                     <td xmlns="http://www.w3.org/1999/xhtml" class="_index">
					    <a class="uri" href="/ovidws/resultsets/emef/nlp/pain/2">2</a>
				     </td>
                     <td class="ai"><span>int 1</span><span>int 2</span></td><td class="al"><span>name one</span><span>name two</span></td>
                   </tr>
                   <tr>
                     <td xmlns="http://www.w3.org/1999/xhtml" class="_index">
					    <a class="uri" href="/ovidws/resultsets/emef/nlp/pain/2">2</a>
				     </td>
                     <td class="ai"><span>int 1</span><span>int 2</span></td><td class="al"><span>name one</span><span>name two</span></td>
                   </tr>
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

  static member TRs n =
    n |> HtmlNode.descendants false (HtmlNode.hasName "tr") 

  static member ByClass n c =
    n |> HtmlNode.descendants false (HtmlNode.hasClass c) 

  static member TDByClass n c =
    getHtmlNode n "td" c

  static member AByClass n c =
    getHtmlNode n "a" c


type OvidAuthorData  =
   {
     Index : string
     AuthorInitials : string list option
     AuthorSurname : string list option
   }  
   static member from (x:HtmlNode) =
     let resultNode = HtmlHelper.ById x "results"


     //row >>= index >>= index number >>= build author record
     let anchorClass n node = HtmlHelper.AByClass node n

     let index row = row |> HtmlNode.descendants false (anchorClass "uri") 
                         |> Seq.tryPick Some


     if resultNode = None then None else

     let tdClass n node = HtmlHelper.TDByClass node n


     let spannie p = resultNode.Value
                            |> HtmlNode.descendants false p
                            |> Seq.collect(HtmlNode.descendants false (HtmlNode.hasName "span"))
                            |> Seq.map(HtmlNode.innerText)
                            |> Seq.toList 
                            |> optionalList

     let spannie2 n p = n |> HtmlNode.descendants false p
                          |> Seq.collect(HtmlNode.descendants false (HtmlNode.hasName "span"))
                          |> Seq.map(HtmlNode.innerText)
                          |> Seq.toList 
                          |> optionalList

      //need to get list of rows, pass each into spannie
     let rows = resultNode.Value |> HtmlHelper.TRs 
                                 |> Seq.map(fun n -> index n)
                                 |> Seq.toList


     Some {Index = ""; AuthorInitials = tdClass "ai" |> spannie; AuthorSurname = tdClass "al" |> spannie}

                                        
let results = doc |> OvidAuthorData.from

let answer = results

let x = answer