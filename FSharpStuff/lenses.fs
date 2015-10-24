module Lenses

        type Car = { 
            Make: string 
            Model: string 
            Mileage: int 
        }

        type Editor = { 
            Name: string 
            Salary: int 
            Car: Car 
        }

        type Book = { 
            Name: string 
            Author: string 
            Editor: Editor 
        }

        let aBook = {Name = "name"; Author = "auth"; 
                    Editor = {Name = "editor"; Salary = 10; 
                    Car ={Make = "make"; Model="mod"; Mileage = 100}}}


        //let mileage = aBook.Editor.Car.Mileage

        //let book2 = { aBook with Editor = 
        //                { aBook.Editor with Car = 
        //                    { aBook.Editor.Car with Mileage = 1000 } } }


        //a' is record type. b' is property type
        type Lens<'a,'b> = {
            Get: 'a -> 'b
            Set: 'b -> 'a -> 'a
        }
        with member l.Update f a = 
            let value = l.Get a 
            let newValue = f value 
            l.Set newValue a


        type Car with
            static member mileage = 
                { Get = fun (c: Car) -> c.Mileage
                  Set = fun mileage (car: Car) -> { car with Mileage = mileage } }

        type Editor with
            static member car = 
              { Get = fun (e: Editor) -> e.Car
                Set = fun (car: Car) (e:Editor) -> { e with Car = car}}

        type Book with
            static member editor =
            { Get = fun (b: Book) -> b.Editor
              Set = fun (e:Editor) (b:Book) -> {b with Editor = e}}

        let inline (>>|) (l1: Lens<_,_>) (l2: Lens<_,_>) = 
            { Get = l1.Get >> l2.Get 
              Set = l2.Set >> l1.Update }


        let bookEditorCarMileage = Book.editor >>| Editor.car >>| Car.mileage

        let mileage = bookEditorCarMileage.Get aBook

        let book2 = aBook |> bookEditorCarMileage.Set 1000

        let inline (+=) (l: Lens<_,_>) v = l.Update ((+) v)

        let book3 = aBook |> bookEditorCarMileage += 1000