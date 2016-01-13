module Catamorphism

//http://fsharpforfunandprofit.com/posts/recursive-types-and-folds/#basic-recursive-type

type Book = {title: string; price: decimal}

type ChocolateType = Dark | Milk | SeventyPercent
type Chocolate = {chocType: ChocolateType ; price: decimal}

type WrappingPaperStyle = 
    | HappyBirthday
    | HappyHolidays
    | SolidColor

type Gift =
    | Book of Book
    | Chocolate of Chocolate 
    | Wrapped of Gift * WrappingPaperStyle
    | Boxed of Gift 
    | WithACard of Gift * message:string

//book
let wolfHall = {title="Wolf Hall"; price=20m}

// a Chocolate
let yummyChoc = {chocType=SeventyPercent; price=5m}

//gift
let birthdayPresent = WithACard (Wrapped (Book wolfHall, HappyBirthday), "Happy Birthday")

//gift
let christmasPresent = Wrapped (Boxed (Chocolate yummyChoc), HappyHolidays)


let rec description gift =
    match gift with 
    | Book book -> 
        sprintf "'%s'" book.title 
    | Chocolate choc -> 
        sprintf "%A chocolate" choc.chocType
    | Wrapped (innerGift,style) -> 
        sprintf "%s wrapped in %A paper" (description innerGift) style
    | Boxed innerGift -> 
        sprintf "%s in a box" (description innerGift) 
    | WithACard (innerGift,message) -> 
        sprintf "%s with a card saying '%s'" (description innerGift) message

let rec totalCost gift =
    match gift with 
    | Book book -> 
        book.price
    | Chocolate choc -> 
        choc.price
    | Wrapped (innerGift,style) -> 
        (totalCost innerGift) + 0.5m
    | Boxed innerGift -> 
        (totalCost innerGift) + 1.0m
    | WithACard (innerGift,message) -> 
        (totalCost innerGift) + 2.0m

//let x = totalCost christmasPresent

let rec whatsInside gift =
    match gift with 
    | Book book -> 
        "A book"
    | Chocolate choc -> 
        "Some chocolate"
    | Wrapped (innerGift,style) -> 
        whatsInside innerGift
    | Boxed innerGift -> 
        whatsInside innerGift
    | WithACard (innerGift,message) -> 
        whatsInside innerGift

//let x = whatsInside christmasPresent


//let rec cataGift fBook fChocolate fWrapped fBox fCard gift =
//    match gift with 
//    | Book book -> 
//        fBook book
//    | Chocolate choc -> 
//        fChocolate choc
//    | Wrapped (innerGift,style) -> 
//        let innerGiftResult = cataGift fBook fChocolate fWrapped fBox fCard innerGift
//        fWrapped (innerGiftResult,style)
//    | Boxed innerGift -> 
//        let innerGiftResult = cataGift fBook fChocolate fWrapped fBox fCard innerGift
//        fBox innerGiftResult 
//    | WithACard (innerGift,message) -> 
//        let innerGiftResult = cataGift fBook fChocolate fWrapped fBox fCard innerGift
//        fCard (innerGiftResult,message) 

let rec cataGift fBook fChocolate fWrapped fBox fCard gift :'r =
    let recurse = cataGift fBook fChocolate fWrapped fBox fCard
    match gift with 
    | Book book -> 
        fBook book
    | Chocolate choc -> 
        fChocolate choc
    | Wrapped (gift,style) -> 
        fWrapped (recurse gift,style)
    | Boxed gift -> 
        fBox (recurse gift)
    | WithACard (gift,message) -> 
        fCard (recurse gift,message) 


let totalCostUsingCata gift =
    let fBook (book:Book) = 
        book.price
    let fChocolate (choc:Chocolate) = 
        choc.price
    let fWrapped  (innerCost,style) = 
        innerCost + 0.5m
    let fBox innerCost = 
        innerCost + 1.0m
    let fCard (innerCost,message) = 
        innerCost + 2.0m
    // call the catamorphism
    cataGift fBook fChocolate fWrapped fBox fCard gift



let x = birthdayPresent |> totalCostUsingCata 