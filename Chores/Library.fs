namespace Chores

open System
type Chore = 
    {
        Name: string
        DueDate: DateTime
        Completed: DateTime option
    }

type Person = 
    {
        Name: string
        Chores: Chore seq 
    }

module Data = 
    let persons = seq {
        { 
            Name = "Markus"
            Chores = seq { 
                { Name = "Clean House"; DueDate = DateTime.Today; Completed = Some DateTime.MaxValue}
                { Name = "Feed the cat Milou"; DueDate = DateTime.Today; Completed = None }
            }
        }
        {
            Name = "Milou"
            Chores = Seq.empty
        }
    }
