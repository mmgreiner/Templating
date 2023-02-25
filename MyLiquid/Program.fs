module MyLiquid.Program

open System

open DotLiquid

let templateString = """
<ul>
{% for person in Persons %}
    <li>
    <p>{{person.Name}} has the following tasks to do:</p>
    <ul>
        {% for task in person.Chores %}
            <li>{{ task.Name }} by {{task.DueDate | Date: "%Y-%m-%d"}} {%if task.Completed %}, completed {{task.Completed.Value | Date: "%Y-%m-%d"}} {%endif%}
            </li>
        {% else %}Free afternoon
        {% endfor %}
    </ul>
{%endfor%}
</ul>
"""

open Chores
open FSharp.Reflection

// Helper function to register types
let RegisterRecordType (typ: Type) =
    let members (typ: Type) =
        if FSharpType.IsRecord typ then
            FSharpType.GetRecordFields(typ)
            |> Seq.map (fun f -> f.Name)
            |> Seq.toArray
        else
            Array.empty

    Template.RegisterSafeType(typ, members typ)

type PersonList = { Persons: Person seq }
[ typeof<Chores.Person>; typeof<Chore>; typeof<PersonList> ] 
    |> List.iter RegisterRecordType

// DateTime option is not a record type, so the above registration does not work
Template.RegisterSafeType(typeof<Option<DateTime>>, [|"Value"|])

// own naming convention
type CamelCaseNamingConvention() =
    let UpperFirstLetter (str: string) = 
        string (Char.ToUpperInvariant(str.[0])) + str.[1..]
    let LowerFirstLetter (str: string) = 
        string (Char.ToLowerInvariant(str.[0])) + str.[1..]

    interface NamingConventions.INamingConvention with
        member val StringComparer: StringComparer = StringComparer.Ordinal
        
        member this.GetMemberName name = name

        member this.OperatorEquals(testedOperator, referenceOperator): bool =
            UpperFirstLetter testedOperator = referenceOperator 
            || LowerFirstLetter testedOperator = referenceOperator
            || testedOperator = LowerFirstLetter referenceOperator

// install new naming convention
//Template.NamingConvention <- new CamelCaseNamingConvention()
Template.NamingConvention <- new NamingConventions.CSharpNamingConvention()

// Date format
Liquid.UseRubyDateFormat <- true

let data = { Persons = Chores.Data.persons }

let template = Template.Parse(templateString)
let txt = template.Render(Hash.FromAnonymousObject(data))

printfn "%s" txt
