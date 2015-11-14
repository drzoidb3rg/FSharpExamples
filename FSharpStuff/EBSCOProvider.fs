module EBSCOProvider
open FSharp.Data
open System
open System.Text

type EBSCOSearchResponseTypeProvider = XmlProvider<"""<rec recordID="1">
        <pdfLink />
        <plink>https://search.ebscohost.com/login.aspx?direct=true&amp;db=c8h&amp;AN=109631477&amp;site=ehost-live</plink>
        <header shortDbName="c8h" uiTerm="109631477" longDbName="CINAHL with Full Text" uiTag="AN">
          <controlInfo>
            <bkinfo>
              <btl>Associations between parental chronic -</btl>
            </bkinfo>
            <dissinfo />
            <jinfo>
              <jid type="issn">14712458</jid>
              <jid type="mid">1CIK</jid>
              <jtl>BMC Public Health</jtl>
              <issn>14712458</issn>
              <maglogo>N</maglogo>
            </jinfo>
            <pubinfo>
              <dt year="2015" month="12" day="01">2015</dt>
              <vid>15</vid>
              <pub>BioMed Central</pub>
            </pubinfo>
            <artinfo>
              <ui>109631477</ui>
              <ui type="cinahl">26296339</ui>
              <ui type="doi">10.1186/s12889-015-2164-9</ui>
              <ui type="pmid">26296339</ui>
              <ui type="other">PMC4546097</ui>
              <ui type="mfs" code="rwh">109631477</ui>
              <ppf>817</ppf>
              <ppct>1</ppct>
              <formats />
              <tig>
                <atl>Associations between parentaly.</atl>
              </tig>
              <aug>
                <au>Kaasbøll, Jannike</au>
                <au>Ranøyen, Ingunn</au>
                <au>Nilsen, Wendy</au>
                <au>Lydersen, Stian</au>
                <au>Indredavik, Marit S</au>
              </aug>
              <sug />
              <ab>Background: Parental chronic pain has been associated </ab>
              <pubtype>Academic Journal</pubtype>
              <doctype>journal article</doctype>
              <ougenre>Article</ougenre>
            </artinfo>
            <language>English</language>
            <refInfo />
            <holdings islocal="N" />
          </controlInfo>
        </header>
      </rec>""", Global=true>


let sample = EBSCOSearchResponseTypeProvider.GetSample()

type Author =
       {
           Id : string
           GivenName : string //firstname
           FamilyName : string //lastname
           Initials : string
           Name : string
       }
       static member empty =
          {Id="";GivenName="";FamilyName="";Initials=""; Name=""}


type Author with
  static member from (x:string) =
    let words = x.Split([|", "|], StringSplitOptions.RemoveEmptyEntries)
    let author = {Author.empty with Name=x}

    printfn "words is : %A" words 

    match words.Length with
      | 2 ->{author with GivenName = words.[1]; FamilyName = words.[0] } 
      | _ -> author

let getAuthors =
  sample.Header.ControlInfo.Artinfo.Augs |> Array.map(Author.from)

